using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;
using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;
using System.IO;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public class AzureMonitorLogsTraceExporterBuilder
    {
        private IServiceCollection _services;

        public AzureMonitorLogsTraceExporterBuilder(AzureMonitorLogsExporterDataCollectorOptions options)
        {
            _services = new ServiceCollection();
            _services.AddAzureLogAnayticsDataCollectorServiceHttpClient();
            _services.AddTraceModel();

            var serviceClientOptions = new AzureMonitorLogsServiceClientOptions()
            {
                Protocol = LogAnalyticsProtocol.DataCollector,
                DestinationTable = options.TableName,
                Authorization = new ServiceClientDataCollectorAuthorizationOptions()
                {
                    ClientSecret = options.SharedKey,
                    AuthorizationSignature = options.WorkspaceId + ":{0}"
                },
                EndPoint = options.EndPoint ?? new Uri($"https://{options.WorkspaceId}.ods.opinsights.azure.com")
            };
            _services.TryAddSingleton<IOptions<AzureMonitorLogsServiceClientOptions>>(sp => new OptionsWrapper<AzureMonitorLogsServiceClientOptions>(serviceClientOptions));
        }

        public AzureMonitorLogsTraceExporterBuilder(AzureMonitorLogsExporterIngestionOptions options)
        {
            _services = new ServiceCollection();
            _services.AddAzureLogAnayticsIngestionServiceHttpClient();
            _services.AddTraceModel();

            var serviceClientOptions = new AzureMonitorLogsServiceClientOptions()
            {
                Protocol = LogAnalyticsProtocol.Ingestion,
                Authorization = new ServiceClientAadAuthorizationOptions()
                {
                    TenantId = options.TenantId,
                    ClientId = options.ClientId,
                    ClientSecret = options.ClientSecret,
                    AuthorityBaseUri = new Uri(options.AuthorityBaseUri),
                    Audience = options.Audience
                },
                EndPoint = new Uri($"{options.DceUri}/dataCollectionRules/{options.DcrImmutableId}/streams/Custom-{options.TableName}")
            };
            _services.TryAddSingleton<IOptions<AzureMonitorLogsServiceClientOptions>>(sp => new OptionsWrapper<AzureMonitorLogsServiceClientOptions>(serviceClientOptions));
        }

        public AzureMonitorLogsTraceExporter Build()
        {
            var serviceProvider = _services.BuildServiceProvider();
            _services = null!;
            return new AzureMonitorLogsTraceExporter(serviceProvider);
        }
    }
}
