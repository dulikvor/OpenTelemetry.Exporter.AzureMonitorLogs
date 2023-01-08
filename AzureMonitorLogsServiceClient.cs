using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;
using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public class AzureMonitorLogsServiceClient : IAzureMonitorLogsServiceClient
    {
        private readonly HttpClient _httpClient;
        public const string MediaType = "application/json";
        public const string TimeGeneratedValue = "";

        public AzureMonitorLogsServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task PostDataCollectorRecordsAsync(IEnumerable<Record> records)
        {
            using var scope = SuppressInstrumentationScope.Begin();
            await _httpClient.PostWithJsonAsync($"api/logs?api-version=2016-04-01", records.Select(record => record.Values));
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
