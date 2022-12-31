using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal class AzureMonitorLogsAuthorizationHandler : DelegatingHandler
    {
        private const string TokenTemplateFormat = "POST\n{0}\napplication/json\nx-ms-date:{1}\n/api/logs";
        private readonly ServiceClientOptions _options;
        public AzureMonitorLogsAuthorizationHandler(ServiceClientOptions options)
        {
            _options = options;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow.ToString("r");
            request.Headers.Authorization = new AuthenticationHeaderValue(GenerateToken(request, currentDateTime));
            request.Headers.Add("x-ms-date", currentDateTime);
            return await base.SendAsync(request, cancellationToken);
        }

        private string GenerateToken(HttpRequestMessage request, string currentDateTime)
        {
            var tokenToBeHashed = string.Format(TokenTemplateFormat, request.Content!.Headers.ContentLanguage, currentDateTime);
            var encoding = new ASCIIEncoding();
            byte[] keyByte = Convert.FromBase64String(_options.AuthorizationSecret);
            byte[] messageBytes = encoding.GetBytes(tokenToBeHashed);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hash = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
