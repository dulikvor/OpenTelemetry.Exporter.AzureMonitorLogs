using OpenTelemetry.Trace;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public static class AzureMonitorLogsExporterHelperExtensions
    {
        public static TracerProviderBuilder AddAzureMonitorLogsExporter(
            this TracerProviderBuilder builder,
            Action<AzureMonitorLogsExporterDataCollectorOptions> configure)
        {
            AzureMonitorLogsExporterDataCollectorOptions options = new();
            configure?.Invoke(options);

            var azureMonitorLogsTraceExporterBuilder = new AzureMonitorLogsTraceExporterBuilder(options);
            var exporter = azureMonitorLogsTraceExporterBuilder.Build();
            return builder.AddProcessor(new BatchActivityExportProcessor(
                exporter,
                options.BatchExportProcessorOptions.MaxQueueSize,
                options.BatchExportProcessorOptions.ScheduledDelayMilliseconds,
                options.BatchExportProcessorOptions.ExporterTimeoutMilliseconds,
                options.BatchExportProcessorOptions.MaxExportBatchSize));
        }

        public static TracerProviderBuilder AddAzureMonitorLogsExporter(
            this TracerProviderBuilder builder,
            Action<AzureMonitorLogsExporterIngestionOptions> configure)
        {
            AzureMonitorLogsExporterIngestionOptions options = new();
            configure?.Invoke(options);

            var azureMonitorLogsTraceExporterBuilder = new AzureMonitorLogsTraceExporterBuilder(options);
            var exporter = azureMonitorLogsTraceExporterBuilder.Build();
            return builder.AddProcessor(new BatchActivityExportProcessor(
                exporter,
                options.BatchExportProcessorOptions.MaxQueueSize,
                options.BatchExportProcessorOptions.ScheduledDelayMilliseconds,
                options.BatchExportProcessorOptions.ExporterTimeoutMilliseconds,
                options.BatchExportProcessorOptions.MaxExportBatchSize));
        }
    }
}
