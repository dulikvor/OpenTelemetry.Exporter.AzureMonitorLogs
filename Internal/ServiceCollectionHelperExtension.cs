using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal static class ServiceCollectionHelperExtension
    {
        public static IServiceCollection AddAzureLogAnayticsServiceHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IAzureMonitorLogsServiceClient, AzureMonitorLogsServiceClient>(
                typeof(AzureMonitorLogsServiceClient).Name,
                (sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureMonitorLogsServiceClientOptions>>().Value;
                    client.BaseAddress = options.EndPoint;
                    client.DefaultRequestHeaders.Add("Accept", AzureMonitorLogsServiceClient.MediaType);
                    client.DefaultRequestHeaders.Add("time-generated-field", AzureMonitorLogsServiceClient.TimeGeneratedValue);
                    client.DefaultRequestHeaders.Add("Log-Type", options.DestinationTable);
                })
                .AddAzureLogAnalyticsAuthorization()
                .SetHandlerLifetime(TimeSpan.FromHours(1));

            return services;
        }
    }
}
