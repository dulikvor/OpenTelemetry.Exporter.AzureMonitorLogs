using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;
using OpenTelemetry.Exporter.AzureMonitorLogs.Internal;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public class AzureMonitorLogsTraceExporterBuilder
    {
        private IServiceCollection _services;

        public AzureMonitorLogsTraceExporterBuilder(AzureMonitorLogsExporterOptions options)
        {
            _services = new ServiceCollection();
            _services.AddAzureLogAnayticsServiceHttpClient();
            _services.AddTraceModel();

            var serviceClientOptions = new AzureMonitorLogsServiceClientOptions()
            {
                DestinationTable = options.TableName,
                AuthorizationSecret = options.SharedKey,
                AuthorizationSignature = options.WorkspaceId + ":{0}",
                EndPoint = options.EndPoint ?? new Uri($"https://{options.WorkspaceId}.ods.opinsights.azure.com")
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
