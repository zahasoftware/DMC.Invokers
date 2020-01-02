using DMC.Invokers.Attributes;
using DMC.Invokers.Domains;
using DMC.Invokers.Exceptions;
using NetXP.NetStandard.Reflection;
using NetXP.NetStandard.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMC.Invokers.Implementations
{
    public class Invoker : IInvoker
    {
        private readonly IReflector reflector;
        private readonly ISerializerFactory serializerFactory;
        private readonly ISerializer serializerJson;

        public Invoker(
            IReflector reflector,
            ISerializerFactory serializerFactory
            )
        {
            this.reflector = reflector;
            this.serializerFactory = serializerFactory;
            serializerJson = serializerFactory.Resolve(SerializerType.Json);
        }

        public List<InvokerInfo> GetInvokers()
        {
            var invokersInterfaces = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(o => o.GetTypes())
                .Where(
                    o => o.IsInterface && o.CustomAttributes.Any(a => a.AttributeType.Name.Equals(nameof(InvokerAttribute)))
                );

            var invokersInfoes = new List<InvokerInfo>();
            foreach (var invokerInterface in invokersInterfaces)
            {
                var invokerAttributes = invokerInterface.CustomAttributes.SingleOrDefault(o => o.AttributeType.Name.Equals(nameof(InvokerAttribute)));

                var helpValue = invokerAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Help")).TypedValue.Value;
                var nameValue = invokerAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Name")).TypedValue.Value;
                var orderValue = invokerAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Order")).TypedValue.Value;
                var osValue = invokerAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("OS")).TypedValue.Value;
                var idValue = invokerAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Id")).TypedValue.Value;
                var osValueCasted = (osValue as System.Collections.ObjectModel.ReadOnlyCollection<System.Reflection.CustomAttributeTypedArgument>);

                var osValueAsString = "";
                if (osValueCasted != null)
                {
                    osValueAsString = string.Join(",", osValueCasted?.Select(o => o.Value.ToString()));
                }

                int.TryParse(orderValue?.ToString(), out int order);

                var name = invokerInterface.Name.Split('.').Last().Replace("Business", "");
                if (name[0].ToString().ToLower() == "i")
                {
                    name = name.Substring(1).ToLower();
                }

                var invokerInfo = new InvokerInfo()
                {
                    InterfaceType = invokerInterface,
                    Help = helpValue?.ToString(),
                    Alias = nameValue?.ToString() ?? name,
                    Order = order,
                    Name = name,
                    Methods = new List<DInvokerMethodInfo>(),
                    OS = osValueAsString,
                    Id = idValue?.ToString() ?? name
                };
                invokersInfoes.Add(invokerInfo);

                var methods = invokerInterface.GetMethods().Where(o => o.CustomAttributes.Any(a => a.AttributeType.Name.Equals(nameof(InvokerMethodAttribute))));
                if (methods.Count() > 0)
                {
                    foreach (var method in methods)
                    {
                        var methodAttributes = method.CustomAttributes.SingleOrDefault(o => o.AttributeType.Name.Equals(nameof(InvokerMethodAttribute)));

                        helpValue = methodAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Help")).TypedValue.Value;
                        nameValue = methodAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Name")).TypedValue.Value;
                        orderValue = methodAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Order")).TypedValue.Value;
                        idValue = methodAttributes.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Id")).TypedValue.Value;

                        var invokerMethodInfo = new DInvokerMethodInfo()
                        {
                            //MethodInfo = method,//!important parameters method data is send in this object using map
                            Help = helpValue?.ToString(),
                            Alias = nameValue?.ToString(),
                            Order = Convert.ToInt32(orderValue),
                            Id = idValue.ToString(),
                            Name = method.Name,
                            ParameterInfoes = new List<DInvokerMethodParameterInfo>()
                        };

                        invokerInfo.Methods.Add(invokerMethodInfo);

                        var invokerParameterInfos = new List<DInvokerMethodParameterInfo>();


                        var parameters = method.GetParameters();
                        foreach (var parameter in parameters)
                        {
                            var parameterAttributes = parameter.CustomAttributes.SingleOrDefault(o => o.AttributeType.Name.Equals(nameof(InvokerPropertyAttribute)));
                            helpValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Help")).TypedValue.Value;
                            nameValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Name")).TypedValue.Value;
                            orderValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Order")).TypedValue.Value;
                            idValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Id")).TypedValue.Value;
                            var maxLength = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("MaxLength")).TypedValue.Value;
                            var minLength = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("MinLength")).TypedValue.Value;
                            var regex = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Regex")).TypedValue.Value;

                            var invokerParameterInfo = new DInvokerMethodParameterInfo()
                            {
                                Name = parameter.Name,
                                Alias = nameValue?.ToString() ?? parameter.Name,
                                Order = parameters.ToList().IndexOf(parameter) + 1,
                                Id = idValue?.ToString() ?? parameter.Name,
                                Help = helpValue?.ToString() ?? "",
                                PropertiesInfoes = parameter.ParameterType.IsValueType ? null : new List<DInvokerMethodParameterInfo>(),
                                MaxLength = maxLength != null ? Convert.ToInt32(maxLength) : (int?)null,
                                MinLength = minLength != null ? Convert.ToInt32(minLength) : (int?)null,
                                Type = parameter.ParameterType,
                                Regex = regex?.ToString(),

                            };
                            invokerMethodInfo.ParameterInfoes.Add(invokerParameterInfo);

                            invokerParameterInfos.Add(invokerParameterInfo);

                            if (!parameter.ParameterType.IsValueType)
                            {
                                foreach (var property in parameter.ParameterType.GetProperties())
                                {
                                    parameterAttributes = property.CustomAttributes.SingleOrDefault(o => o.AttributeType.Name.Equals(nameof(InvokerPropertyAttribute)));
                                    helpValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Help")).TypedValue.Value;
                                    nameValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Name")).TypedValue.Value;
                                    orderValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Order")).TypedValue.Value;
                                    idValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Id")).TypedValue.Value;
                                    maxLength = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("MaxLength")).TypedValue.Value;
                                    minLength = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("MinLength")).TypedValue.Value;
                                    regex = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Regex")).TypedValue.Value;

                                    invokerParameterInfo.PropertiesInfoes.Add(new DInvokerMethodParameterInfo
                                    {
                                        Name = property.Name,
                                        Alias = nameValue?.ToString() ?? property.Name,
                                        Order = Convert.ToInt32(orderValue?.ToString() ?? "0"),
                                        Id = idValue?.ToString() ?? property.Name,
                                        Help = helpValue?.ToString(),
                                        Type = property.PropertyType,
                                        MaxLength = maxLength != null ? Convert.ToInt32(maxLength) : (int?)null,
                                        MinLength = minLength != null ? Convert.ToInt32(minLength) : (int?)null,
                                        Regex = regex?.ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }

            foreach (var invokerInfo in invokersInfoes)
            {
                invokerInfo.Methods = invokerInfo.Methods.OrderBy(o => o.Order).ToList();
            }
            invokersInfoes = invokersInfoes.OrderBy(o => o.Order).ToList();

            return invokersInfoes.Count > 0 ? invokersInfoes : null;
        }

        public object Invoke(DInvokerMethodParam param)
        {
            try
            {
                //TODO: typeof(IInfokerBusiness should change to receive dynamic namespace to get plugins)
                var method = reflector.ReflectMethod(typeof(IInvoker), param.Interface, param.Method);

                var methodParameter = method.GetParameters();
                var parametersToInvoke = new object[param.Params.Length];

                if (methodParameter.Length != param.Params.Length)
                {
                    throw new InvokerException("Invalid number of parameters", InvokerExceptionType.Warn);
                }

                for (int i = 0; i < param.Params.Length; i++)
                {
                    var serializedParam = param.Params[i].SerializedData;
                    var paraType = methodParameter[i];

                    foreach (var property in paraType.ParameterType.GetProperties())//Mayuzculization
                    {
                        var parameterAttributes = property.CustomAttributes.SingleOrDefault(o => o.AttributeType.Name.Equals(nameof(InvokerPropertyAttribute)));
                        var nameValue = parameterAttributes?.NamedArguments.SingleOrDefault(o => o.MemberInfo.Name.Equals("Name")).TypedValue.Value;

                        var aux = serializedParam;
                        serializedParam = serializedParam.Replace($"\"{property.Name.ToLower()}\"", $"\"{property.Name}\"");

                        if (aux == serializedParam)
                        {
                            serializedParam = serializedParam.Replace($"\"{nameValue}\"", $"\"{property.Name}\"");
                        }
                    }

                    var parameterObject = serializerJson.Deserialize(paraType.ParameterType, Encoding.UTF8.GetBytes(serializedParam));
                    parametersToInvoke[i] = parameterObject;

                }

                var data = reflector.InvokeMethod(typeof(IInvoker), param.Interface, param.Method, parametersToInvoke);
                return data;
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                if (tie.InnerException is InvokerException ex)
                {
                    return new InvokerExceptionBDM
                    {
                        Message = ex.Message,
                        InvokerExceptionType = ex.InvokerExceptionType
                    };
                }
                else if (tie.InnerException is NotImplementedException nie)
                {
                    return new InvokerExceptionBDM
                    {
                        Message = nie.Message,
                        InvokerExceptionType = InvokerExceptionType.Warn
                    };
                }
                else
                    throw tie.InnerException;
            }
            catch (InvokerException ex)
            {
                return new InvokerExceptionBDM
                {
                    Message = ex.Message,
                    InvokerExceptionType = ex.InvokerExceptionType
                };
            }
        }
    }
}
