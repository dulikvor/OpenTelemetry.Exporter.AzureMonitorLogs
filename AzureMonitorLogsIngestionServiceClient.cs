using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;
using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public class AzureMonitorLogsIngstionServiceClient : IAzureMonitorLogsServiceClient
    {
        private readonly HttpClient _httpClient;
        public const string MediaType = "application/json";

        public AzureMonitorLogsIngstionServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task PostRecordsAsync(IEnumerable<Record> records)
        {
            using var scope = SuppressInstrumentationScope.Begin();
            await _httpClient.PostWithJsonAsync($"?api-version=2021-11-01-preview", records.Select(record => record.Values));
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
