using OpenTelemetry;
using OpenTelemetry.Exporter.AzureMonitorLogs;
using OpenTelemetry.Exporter.AzureMonitorLogs.Monitor;
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
                        .AddSource(ActivityScope.Source)
                        .Build();
            {
                for(int idx = 0; idx < 10; idx++)
                {
                    using var activityScope = ActivityScope.Create(nameof(ConsoleTest));
                    activityScope.Monitor(() =>
                    {
                        Foo().Wait();
                    });
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(10));
            System.Console.WriteLine("Press ENTER to stop.");
            System.Console.ReadLine();
        }

        private static async Task Foo()
        {
            using var activityScope = ActivityScope.Create(nameof(ConsoleTest));
            await activityScope.Monitor(async () =>
            {
                activityScope.Activity.SetTag("sometag1", "tag1");
                activityScope.Activity.SetTag("sometag2", "tag2");
                await Boo();
            });
        }

        private static async Task Boo()
        {
            using var activitySource = new ActivitySource(ActivityScope.Source);
            using var activity = activitySource.StartActivity($"{nameof(ConsoleTest)}.Boo");
            {
                activity!.AddEvent(new ActivityEvent("SomeEvent"));
                activity.SetTag("sometag1", "tag3");
                activity.SetTag("sometag2", "tag4");
            }

            await Task.CompletedTask;
        }
    }
}
