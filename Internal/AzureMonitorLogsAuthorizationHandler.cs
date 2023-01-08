using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal class AzureMonitorLogsAuthorizationHandler : DelegatingHandler
    {
        private readonly ServiceClientOptions _options;

        public AzureMonitorLogsAuthorizationHandler(ServiceClientOptions options)
        {
            _options = options;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow.ToString("r");
            request.Headers.Authorization = new AuthenticationHeaderValue("SharedKey", string.Format(_options.AuthorizationSignature, GenerateToken(request, currentDateTime)));
            request.Headers.Add("x-ms-date", currentDateTime);
            
            return await base.SendAsync(request, cancellationToken);
        }

        private string GenerateToken(HttpRequestMessage request, string currentDateTime)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(request.Method.Method);
            stringBuilder.Append('\n');
            if (request.Content!.Headers.ContentEncoding.Any())
            {
                stringBuilder.Append(request.Content!.Headers.ContentEncoding);
                stringBuilder.Append('\n');
            }

            if (request.Content!.Headers.ContentLength != null)
            {
                stringBuilder.Append(request.Content!.Headers.ContentLength);
                stringBuilder.Append('\n');
            }

            if (request.Content!.Headers.ContentType != null)
            {
                stringBuilder.Append(request.Content!.Headers.ContentType);
                stringBuilder.Append('\n');
            }

            stringBuilder.Append($"x-ms-date:{currentDateTime}");
            stringBuilder.Append('\n');
            stringBuilder.Append(request.RequestUri!.LocalPath);

            byte[] messageBytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            byte[] keyBytes = Convert.FromBase64String(_options.AuthorizationSecret);
            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
