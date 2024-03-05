namespace C_11_Demo.Pieces.Decorator;
internal class SceneFactory
{
    public static IAction GetActor<T>() where T : class, IAction, new()
    {
        IAction actor = new T();

        var decoratorAttrType = typeof(DecorateAttribute<>);
        var decoratorAttrData = actor.GetType().CustomAttributes.Where(x => x.AttributeType.GetGenericTypeDefinition() == decoratorAttrType)
            .Reverse()
            .ToList();

        foreach (var decoratorAttr in decoratorAttrData)
        {
            var decorateType = decoratorAttr.AttributeType.GetGenericArguments().FirstOrDefault(x => x.IsAssignableTo(typeof(IAction)));
            if (decorateType is not null)
            {
                var instance = Activator.CreateInstance(decorateType, actor);

                if (instance is not null)
                    actor = (IAction)instance;
            }
        }

        return actor;
    }
}