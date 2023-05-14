using System;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal abstract class ServiceClientAuthorizationOptions
    {
        public string ClientSecret { get; set; }
        public Guid ClientId { get; set; }
    }

    internal class ServiceClientDataCollectorAuthorizationOptions : ServiceClientAuthorizationOptions
    {
        public string AuthorizationSignature { get; set; }
    }

    internal class ServiceClientAadAuthorizationOptions : ServiceClientAuthorizationOptions
    {
        public ServiceClientAadAuthorizationOptions()
        {
            AuthorityBaseUri = new Uri("https://monitor.azure.com");
            Audience = $"{AuthorityBaseUri}//.default";
        }

        public Guid TenantId { get; set; }
        public Uri AuthorityBaseUri { get; set; }
        public string Audience { get; set; }
    }
}
