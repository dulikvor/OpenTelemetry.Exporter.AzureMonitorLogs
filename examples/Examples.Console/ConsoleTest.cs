using OpenTelemetry;
using OpenTelemetry.Exporter.AzureMonitorLogs;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Examples.Console
{
    internal class ConsoleTest
    {
        public static void RunTest(AzureMonitorLogsExporterDataCollectorOptions options)
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                        .AddSource("Samples.SampleServer")
                        .AddAzureMonitorLogsExporter(o =>
                        {
                            o.WorkspaceId = options.WorkspaceId;
                            o.SharedKey = options.SharedKey;
                            o.TableName = options.TableName;
                        })
                        .Build();
            {
                using var source = new ActivitySource("Samples.SampleServer");
                using var span = source.StartActivity("SomeSpan", ActivityKind.Internal);
                span!.AddEvent(new ActivityEvent("SomeEvent"));
                span.SetTag("sometag1", "tag1");
                span.SetTag("sometag2", "tag2");
            }

            Thread.Sleep(TimeSpan.FromSeconds(10));
            System.Console.WriteLine("Press ENTER to stop.");
            System.Console.ReadLine();
        }

        public static void RunTest(AzureMonitorLogsExporterIngestionOptions options)
        {
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                        .AddSource("Samples.SampleServer")
                        .AddAzureMonitorLogsExporter(o =>
                        {
                            o.WorkspaceId = options.WorkspaceId;
                            o.ClientId = options.ClientId;
                            o.ClientSecret = options.ClientSecret;
                            o.TenantId = options.TenantId;
                            o.AuthorityBaseUri = options.AuthorityBaseUri;
                            o.Audience = options.Audience;
                            o.DceUri = options.DceUri;
                            o.DcrImmutableId = options.DcrImmutableId;
                            o.TableName = options.TableName;
                        })
                        .Build();
            {
                using var source = new ActivitySource("Samples.SampleServer");
                using var span = source.StartActivity("SomeSpan", ActivityKind.Internal);
                span!.AddEvent(new ActivityEvent("SomeEvent"));
                span.SetTag("sometag1", "tag1");
                span.SetTag("sometag2", "tag2");
            }

            Thread.Sleep(TimeSpan.FromSeconds(10));
            System.Console.WriteLine("Press ENTER to stop.");
            System.Console.ReadLine();
        }
    }
}
