namespace TAuth.ResourceAPI.Auth.Policies
{
    public static class PolicyNames
    {
        public static class Resource
        {
            public const string CanDeleteResource = nameof(CanDeleteResource);
        }

        public const string WorkerOnly = nameof(WorkerOnly);
        public const string IsOwner = nameof(IsOwner);
        public const string IsLucky = nameof(IsLucky);
        public const string IsAdmin = nameof(IsAdmin);
    }
}
