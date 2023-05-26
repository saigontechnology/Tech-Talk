using System;

namespace TAuth.IDP
{
    public static class AuthConstants
    {
        public const string IDPName = "TAuth";

        public static class IIS
        {
            public const string AuthDisplayName = "Windows";
        }

        public static class AuthSchemes
        {
            public const string Facebook = nameof(Facebook);
            public const string Google = nameof(Google);
            public const string Windows = nameof(Windows);
            public const string IdentityMfa = nameof(IdentityMfa);
        }

        public static class Mfa
        {
            public static readonly TimeSpan DefaultExpireTime = TimeSpan.FromMinutes(5);

            public const int OTPLength = 6;
            public const int OTPIntervalSeconds = 60;
            public const int AuthenticatorAppSecretKeyLength = 64;
            public const string OTPSecretKeyName = "TOTP";
        }
    }
}
