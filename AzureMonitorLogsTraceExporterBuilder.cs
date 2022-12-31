using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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

            var serviceClientOptions = new AzureMonitorLogsServiceClientOptions()
            {
                DestinationTable = options.TableName,
                AuthorizationSecret = options.SharedKey,
                EndPoint = options.EndPoint
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
