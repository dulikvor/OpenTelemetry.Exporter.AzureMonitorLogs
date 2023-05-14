using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal static class HttpClientBuilderHelperExtension
    {
        public static IHttpClientBuilder AddAzureLogAnalyticsDataCollectorAuthorization(this IHttpClientBuilder builder)
        {
            return builder.AddHttpMessageHandler(sp =>
            {
                var serviceClientOptions = sp.GetRequiredService<IOptions<AzureMonitorLogsServiceClientOptions>>().Value;
                return new AzureMonitorLogsDataCollectorAuthorizationHandler(serviceClientOptions);
            });
        }

        public static IHttpClientBuilder AddAzureLogAnalyticsAadAuthorization(this IHttpClientBuilder builder)
        {
            return builder.AddHttpMessageHandler(sp =>
            {
                var serviceClientOptions = sp.GetRequiredService<IOptions<AzureMonitorLogsServiceClientOptions>>().Value;
                return new AzureMonitorLogsAadAuthorizationHandler(serviceClientOptions);
            });
        }
    }
}
