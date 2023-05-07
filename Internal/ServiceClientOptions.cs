using System;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal class ServiceClientOptions
    {
        public Uri? EndPoint { get; set; }
        public ServiceClientAuthorizationOptions Authorization { get; set; }
    }
}