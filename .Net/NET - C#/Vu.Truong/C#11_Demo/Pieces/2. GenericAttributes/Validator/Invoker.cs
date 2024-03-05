namespace C_11_Demo.Pieces.Validator;
internal class Invoker
{
    public static void InvokeAction(Delegate action, params object[] args)
    {
        var method = action.Method;
        var validatorAttrType = typeof(MyValidationAttribute<>);
        var attr = method.CustomAttributes.FirstOrDefault(x => x.AttributeType.GetGenericTypeDefinition() == validatorAttrType);

        if (attr is not null)
        {
            var validatorInfo = attr.AttributeType.GetProperty("Validator");
            var attribute = method.GetCustomAttribute(attr.AttributeType);
            var validator = validatorInfo?.GetValue(attribute);

            if (validator is not null)
            {
                var vType = validatorInfo.PropertyType;
                var validatorType = typeof(BaseValidator<>);
                if (vType.BaseType != null && vType.BaseType.IsGenericType
                    && vType.BaseType.GetGenericTypeDefinition() == validatorType)
                {
                    var genericVType = vType.BaseType.GetGenericArguments().FirstOrDefault();

                    if (genericVType is not null)
                    {
                        var informArgs = args?.Where(x => x.GetType() == genericVType)?.ToArray();

                        if (informArgs is null)
                            throw new ArgumentNullException();

                        var isValid = (bool?)vType.InvokeMember("Validate",
                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.InvokeMethod,
                            null, validator, informArgs);

                        if (isValid != true)
                        {
                            throw new Exception($"{genericVType.Name} is invalid");
                        }
                    }
                }
            }
        }

        action.DynamicInvoke(args);
    }
}