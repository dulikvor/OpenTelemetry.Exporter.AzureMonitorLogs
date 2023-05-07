using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    internal interface IAzureMonitorLogsServiceClient : IDisposable
    {
        Task PostRecordsAsync(IEnumerable<Record> records);
    }
}