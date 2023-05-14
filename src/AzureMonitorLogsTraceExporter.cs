using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;
using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;
using System.Diagnostics;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public class AzureMonitorLogsTraceExporter : BaseExporter<Activity>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TableModel _tableMode;
        private readonly ITypedHttpClientFactory<AzureMonitorLogsDataCollectorServiceClient> _dataCollectorServiceClientFactory;
        private readonly ITypedHttpClientFactory<AzureMonitorLogsIngstionServiceClient> _ingestionServiceClientFactory;
        private readonly AzureMonitorLogsServiceClientOptions _logServiceOptions;

        public AzureMonitorLogsTraceExporter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logServiceOptions = _serviceProvider.GetRequiredService<IOptions<AzureMonitorLogsServiceClientOptions>>().Value;
            _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            _dataCollectorServiceClientFactory = _serviceProvider.GetService<ITypedHttpClientFactory<AzureMonitorLogsDataCollectorServiceClient>>();
            _ingestionServiceClientFactory = _serviceProvider.GetService<ITypedHttpClientFactory<AzureMonitorLogsIngstionServiceClient>>();
            _tableMode = _serviceProvider.GetRequiredService<TableModel>();
        }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            try
            {
                var records = new List<Record>((int)batch.Count);
                foreach (var activity in batch)
                {
                    var record = Record.Create(activity);
                    _tableMode.ValidateRecord(record);
                    records.Add(record);
                }
                
                using var serviceClient = GetServiceClient();
                var task = Task.Run(async () => await serviceClient.PostRecordsAsync(records));
                task.Wait();
                return ExportResult.Success;
            }
            catch(Exception exception) //Making sure process will not abort on failure
            {
                AzureMonitorLogsEventSource.Source.SendingRecordsToDataGatewayFailed(exception.Message);
                return ExportResult.Failure;
            }
        }

        private IAzureMonitorLogsServiceClient GetServiceClient()
        {
            if(_logServiceOptions.Protocol == LogAnalyticsProtocol.DataCollector)
            {
                var httpClient = _httpClientFactory.CreateClient(typeof(AzureMonitorLogsDataCollectorServiceClient).Name);
                return _dataCollectorServiceClientFactory.CreateClient(httpClient);
            }
            else
            {
                var httpClient = _httpClientFactory.CreateClient(typeof(AzureMonitorLogsIngstionServiceClient).Name);
                return _ingestionServiceClientFactory.CreateClient(httpClient);
            }
        }
    }
}
