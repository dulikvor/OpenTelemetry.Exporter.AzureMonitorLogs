using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    internal class AzureMonitorLogsServiceClientOptions : ServiceClientOptions
    {
        public string DestinationTable { get; set; }
    }
}