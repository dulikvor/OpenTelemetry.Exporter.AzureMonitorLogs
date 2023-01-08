using OpenTelemetry;
using OpenTelemetry.Exporter.AzureMonitorLogs;
using OpenTelemetry.Trace;
using System.Diagnostics;

class TestClass
{
    static void Main(string[] args)
    {
        GenerateInstrumentation();
        Thread.Sleep(TimeSpan.FromMinutes(5));
    }

    private static void GenerateInstrumentation()
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                    .AddSource("Samples.SampleClient", "Samples.SampleServer")
                    .AddAzureMonitorLogsExporter(options =>
                    {
                        options.WorkspaceId = Guid.Empty;//Guid.Parse(some workspace immutable id);
                        options.SharedKey = "";//Workspace shared key.
                        options.TableName = "BabyShark";
                    })
                    .Build();
        {
            using var source = new ActivitySource("Samples.SampleServer");
            using var span = source.StartActivity("SomeSpan", ActivityKind.Internal);
            span!.AddEvent(new ActivityEvent("SomeEvent"));
            span.SetTag("sometag1", "tag1");
            span.SetTag("sometag2", "tag2");
        }
    }
}