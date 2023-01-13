using CommandLine;
using Examples.Console;
using OpenTelemetry.Exporter.AzureMonitorLogs;

class TestClass
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<AzureMonitorLogsExporterOptions>(args)
            .WithParsed<AzureMonitorLogsExporterOptions>(o =>
            {
                ConsoleTest.RunTest(o);
            });
    }
}