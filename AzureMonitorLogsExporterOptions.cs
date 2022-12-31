using CommandLine;
using System.Diagnostics;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    [Verb("azureMonitorLogs", HelpText = "Specify the options required to test AzureMonitorLogs exporter")]
    public class AzureMonitorLogsExporterOptions
    {
        public AzureMonitorLogsExporterOptions()
        {
            BatchExportProcessorOptions = new();
        }

        [Option("sharedKey", HelpText = "Please specify shared key required to authenticate", Required = true)]
        public string SharedKey { get; set; }

        [Option("tableName", HelpText = "Please specify destination table", Required = true)]
        public string TableName { get; set; }

        [Option("workspaceId", HelpText = "Please specify destination workspace immutable id", Required = true)]
        public Guid WorkspaceId { get; set; }

        [Option("endpoint", HelpText = "Please specify destination backend endpoint", Required = true)]
        public Uri? EndPoint { get; set; }

        public BatchExportProcessorOptions<Activity> BatchExportProcessorOptions { get; set; }
    }
}
