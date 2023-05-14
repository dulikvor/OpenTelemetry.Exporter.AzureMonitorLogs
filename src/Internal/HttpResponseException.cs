using System.Net;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public HttpResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
