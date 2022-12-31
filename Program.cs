using OpenTelemetry;
using OpenTelemetry.Exporter.AzureMonitorLogs;
using OpenTelemetry.Trace;
using System.Diagnostics;

class TestClass
{
    static void Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                    .AddSource("Samples.SampleClient", "Samples.SampleServer")
                    .AddAzureMonitorLogsExporter(options =>
                    {
                        options.EndPoint = new Uri("https://127.0.0.1:8080");
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