using System;
using System.Security.Cryptography;

namespace AuthNET.Sharing.WebApp
{
    public static class AppHelper
    {
        public static string GetRandomSecretKey()
        {
            using SHA256Managed hasher = new SHA256Managed();

            var bytes = Guid.NewGuid().ToByteArray();

            byte[] keyBytes = hasher.ComputeHash(bytes);

            return Convert.ToBase64String(keyBytes);
        }
    }
}
