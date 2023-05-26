using Confluent.Kafka;
using System;
using System.IO;
using System.Linq;

namespace TStore.Shared.Helpers
{
    public static class KafkaClientHelper
    {
        public static void FindCertIfNotFound(this ClientConfig config)
        {
            bool exist = File.Exists(config.SslCertificateLocation);

            if (exist)
            {
                return;
            }

            string cd = Directory.GetCurrentDirectory();
            DirectoryInfo parent = Directory.GetParent(cd);

            while (parent != null)
            {
                if (parent.EnumerateDirectories("misc").Any(dir => dir.Name == "misc"))
                {
                    config.SslCaLocation = Path.Combine(parent.FullName, "misc", "certs", "kafka-broker", "ca-root.crt");
                    config.SslCertificateLocation = Path.Combine(parent.FullName, "misc", "certs", "kafka-client", "tstore.crt");
                    config.SslKeyLocation = Path.Combine(parent.FullName, "misc", "certs", "kafka-client", "tstore.key");
                    return;
                }

                parent = parent.Parent;
            }

            throw new Exception("Cannot find valid client certificate/key, " +
                "please change the appsettings.json SslCertificateLocation, SslKeyLocation values");
        }
    }
}
