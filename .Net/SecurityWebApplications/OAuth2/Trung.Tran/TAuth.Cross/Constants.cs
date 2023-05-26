namespace TAuth.Cross
{
    public static class CookieConstants
    {
        public const string SharedCookieName = ".TAuth.SharedCookie";
    }

    public static class OpenIdConnectConstants
    {
        public static class PropertyNames
        {
            public const string AccessToken = "access_token";
        }

        public static class AuthSchemes
        {
            public const string Introspection = nameof(Introspection);
        }
    }
}
