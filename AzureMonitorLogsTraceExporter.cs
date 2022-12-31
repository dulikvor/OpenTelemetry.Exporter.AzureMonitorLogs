using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Diagnostics;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public class AzureMonitorLogsTraceExporter : BaseExporter<Activity>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITypedHttpClientFactory<AzureMonitorLogsServiceClient> _serviceClientFactory;

        public AzureMonitorLogsTraceExporter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            _serviceClientFactory = _serviceProvider.GetRequiredService<ITypedHttpClientFactory<AzureMonitorLogsServiceClient>>();
        }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            //using var scope = SuppressInstrumentationScope.Begin();
            using var serviceClient = GetServiceClient();
            return ExportResult.Success;
        }

        private IAzureMonitorLogsServiceClient GetServiceClient()
        {
            var httpClient = _httpClientFactory.CreateClient(typeof(AzureMonitorLogsServiceClient).Name);
            return _serviceClientFactory.CreateClient(httpClient);
        }
    }
}
