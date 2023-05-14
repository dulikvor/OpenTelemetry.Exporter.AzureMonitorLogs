using CommandLine;
using System.Diagnostics;

namespace OpenTelemetry.Exporter.AzureMonitorLogs
{
    [Verb("datacollector", HelpText = "Specify the options required to set AzureMonitorLogs exporter targeting data collector api")]
    public class AzureMonitorLogsExporterDataCollectorOptions
    {
        public AzureMonitorLogsExporterDataCollectorOptions()
        {
            BatchExportProcessorOptions = new();
        }

        [Option("sharedkey", HelpText = "Please specify shared key required to authenticate", Required = true)]
        public string SharedKey { get; set; }

        [Option("tablename", HelpText = "Please specify destination table", Required = true)]
        public string TableName { get; set; }

        [Option("workspaceid", HelpText = "Please specify destination workspace immutable id", Required = true)]
        public Guid WorkspaceId { get; set; }

        [Option("endpoint", HelpText = "Please specify destination backend endpoint", Required = false)]
        public Uri? EndPoint { get; set; }

        public BatchExportProcessorOptions<Activity> BatchExportProcessorOptions { get; set; }
    }

    [Verb("ingestion", HelpText = "Specify the options required to set AzureMonitorLogs exporter targeting ingestion api")]
    public class AzureMonitorLogsExporterIngestionOptions
    {
        public AzureMonitorLogsExporterIngestionOptions()
        {
            BatchExportProcessorOptions = new();
        }

        [Option("tablename", HelpText = "Please specify destination table", Required = true)]
        public string TableName { get; set; }

        [Option("workspaceid", HelpText = "Please specify destination workspace immutable id", Required = true)]
        public Guid WorkspaceId { get; set; }

        [Option("clientid", HelpText = "Please specify client id", Required = true)]
        public Guid ClientId { get; set; }

        [Option("clientsecret", HelpText = "Please specify client secret", Required = true)]
        public string ClientSecret { get; set; }

        [Option("tenantid", HelpText = "Please specify the tenant id", Required = true)]
        public Guid TenantId { get; set; }

        [Option("authoritybaseuri", HelpText = "Please specify authority base uri", Required = true)]
        public string AuthorityBaseUri { get; set; }

        [Option("audience", HelpText = "Please specify audience", Required = true)]
        public string Audience { get; set; }

        [Option("dceuri", HelpText = "Please specify dce uri", Required = true)]
        public string DceUri { get; set; }

        [Option("dcrimmutableid", HelpText = "Please specify dcr immutable id", Required = true)]
        public string DcrImmutableId { get; set; }

        public BatchExportProcessorOptions<Activity> BatchExportProcessorOptions { get; set; }
    }
}
