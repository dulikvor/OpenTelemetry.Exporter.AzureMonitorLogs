using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
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
        private readonly ITypedHttpClientFactory<AzureMonitorLogsServiceClient> _serviceClientFactory;

        public AzureMonitorLogsTraceExporter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            _serviceClientFactory = _serviceProvider.GetRequiredService<ITypedHttpClientFactory<AzureMonitorLogsServiceClient>>();
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
                var task = Task.Run(async () => await serviceClient.PostDataCollectorRecordsAsync(records));
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
            var httpClient = _httpClientFactory.CreateClient(typeof(AzureMonitorLogsServiceClient).Name);
            return _serviceClientFactory.CreateClient(httpClient);
        }
    }
}
