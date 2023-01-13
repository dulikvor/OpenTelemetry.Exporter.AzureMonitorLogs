using OpenTelemetry.Trace;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    public static class AzureMonitorLogsExporterHelperExtensions
    {
        public static TracerProviderBuilder AddAzureMonitorLogsExporter(
            this TracerProviderBuilder builder,
            Action<AzureMonitorLogsExporterOptions> configure)
        {
            AzureMonitorLogsExporterOptions options = new();
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
