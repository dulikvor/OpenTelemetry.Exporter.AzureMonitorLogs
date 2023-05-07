using CommandLine;
using Examples.Console;
using OpenTelemetry.Exporter.AzureMonitorLogs;

class TestClass
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<AzureMonitorLogsExporterDataCollectorOptions, AzureMonitorLogsExporterIngestionOptions>(args)
            .WithParsed<AzureMonitorLogsExporterDataCollectorOptions>(o =>
            {
                ConsoleTest.RunTest(o);
            })
            .WithParsed<AzureMonitorLogsExporterIngestionOptions>(o =>
            {
                ConsoleTest.RunTest(o);
            });
    }
}