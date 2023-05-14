using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    [EventSource(Name = "AzureMonitorLogs")]
    internal class AzureMonitorLogsEventSource : EventSource
    {
        public static AzureMonitorLogsEventSource Source { get; } = new();
#if DEBUG
        public static AzureMonitorLogsEventListener Listener { get; } = new();
#endif
        [Event(1, Message = "An attempt to send records to azre monitor logs legacy gateway failed - {0}.", Level = EventLevel.Warning)]
        public void SendingRecordsToDataGatewayFailed(string error)
        {
            this.WriteEvent(1, error);
        }
    }

    public class AzureMonitorLogsEventListener : EventListener
    {
        private readonly List<EventSource> _eventSources = new();

        public override void Dispose()
        {
            foreach (EventSource eventSource in _eventSources)
            {
                DisableEvents(eventSource);
            }

            base.Dispose();
            GC.SuppressFinalize(this);
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            if (eventSource?.Name.Equals("AzureMonitorLogs", StringComparison.OrdinalIgnoreCase) == true)
            {
                _eventSources.Add(eventSource);
                EnableEvents(eventSource, EventLevel.Verbose, EventKeywords.All);
            }

            base.OnEventSourceCreated(eventSource!);
        }

        protected override void OnEventWritten(EventWrittenEventArgs e)
        {
            string message;
            if (e.Message != null && (e.Payload?.Count ?? 0) > 0)
            {
                message = string.Format(e.Message, e.Payload!.ToArray());
            }
            else
            {
                message = e.Message!;
            }

            Console.WriteLine($"{e.EventSource.Name} - EventId: [{e.EventId}], EventName: [{e.EventName}], Message: [{message}]");
        }
    }
}
