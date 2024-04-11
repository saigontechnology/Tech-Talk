namespace efcore_demos.DataAccess.Interceptors;
internal class SetRetrievedInterceptor : IMaterializationInterceptor
{
    public InterceptionResult InitializingInstance(MaterializationInterceptionData materializationData, object entity, InterceptionResult result)
    {
        if (entity is IBaseEntity baseEntity && baseEntity.CreatedDate < DateTime.Now.AddYears(-10))
        {
            return InterceptionResult.Suppress();
        }

        Console.WriteLine("InitializedInstance");

        return result;
    }

    public object InitializedInstance(MaterializationInterceptionData materializationData, object instance)
    {
        if (instance is IHasRetrieved hasRetrieved)
        {
            hasRetrieved.Retrieved = DateTime.UtcNow;
            Console.WriteLine("InitializedInstance 2");
        }

        return instance;
    }
}
