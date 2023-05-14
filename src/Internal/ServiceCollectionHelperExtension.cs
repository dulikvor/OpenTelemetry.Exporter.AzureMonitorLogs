using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter.AzureMonitorLogs.DataModel;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal static class ServiceCollectionHelperExtension
    {
        public static IServiceCollection AddAzureLogAnayticsDataCollectorServiceHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IAzureMonitorLogsServiceClient, AzureMonitorLogsDataCollectorServiceClient>(
                typeof(AzureMonitorLogsDataCollectorServiceClient).Name,
                (sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureMonitorLogsServiceClientOptions>>().Value;
                    client.BaseAddress = options.EndPoint;
                    client.DefaultRequestHeaders.Add("Accept", AzureMonitorLogsDataCollectorServiceClient.MediaType);
                    client.DefaultRequestHeaders.Add("time-generated-field", AzureMonitorLogsDataCollectorServiceClient.TimeGeneratedValue);
                    client.DefaultRequestHeaders.Add("Log-Type", options.DestinationTable);
                })
                .AddAzureLogAnalyticsDataCollectorAuthorization()
                .SetHandlerLifetime(TimeSpan.FromHours(1));

            return services;
        }

        public static IServiceCollection AddAzureLogAnayticsIngestionServiceHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<IAzureMonitorLogsServiceClient, AzureMonitorLogsIngstionServiceClient>(
                typeof(AzureMonitorLogsIngstionServiceClient).Name,
                (sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<AzureMonitorLogsServiceClientOptions>>().Value;
                    client.BaseAddress = options.EndPoint;
                })
                .AddAzureLogAnalyticsAadAuthorization()
                .SetHandlerLifetime(TimeSpan.FromHours(1));

            return services;
        }

        public static IServiceCollection AddTraceModel(this IServiceCollection services)
        {
            services.AddSingleton(sp => {
                var columns = new List<ColumnModel>()
                {
                    ColumnModel.Create(TraceTableModelExtensionHelper.NameProperty, ColumnType.String),
                    ColumnModel.Create(TraceTableModelExtensionHelper.TraceIdProperty, ColumnType.String),
                    ColumnModel.Create(TraceTableModelExtensionHelper.SpanIdProperty, ColumnType.String),
                    ColumnModel.Create(TraceTableModelExtensionHelper.ParentIdProperty, ColumnType.String),
                    ColumnModel.Create(TraceTableModelExtensionHelper.StartTimeProperty, ColumnType.DateTime),
                    ColumnModel.Create(TraceTableModelExtensionHelper.EndTimeProperty, ColumnType.DateTime),
                    ColumnModel.Create(TraceTableModelExtensionHelper.AttributesProperty, ColumnType.Dynamic)
                };
                return new TableModel(columns);
            });
            return services;
        }
    }
}
