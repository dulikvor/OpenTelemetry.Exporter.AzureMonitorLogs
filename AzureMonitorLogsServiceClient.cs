using System.Net.Http;
using System.Net.Http.Headers;

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

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
