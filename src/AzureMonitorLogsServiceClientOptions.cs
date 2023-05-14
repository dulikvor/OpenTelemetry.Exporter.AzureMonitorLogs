using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    internal enum LogAnalyticsProtocol
    {
        DataCollector,
        Ingestion
    }

    internal class AzureMonitorLogsServiceClientOptions : ServiceClientOptions
    {
        public string DestinationTable { get; set; }
        public LogAnalyticsProtocol? Protocol { get; set; }

    }
}