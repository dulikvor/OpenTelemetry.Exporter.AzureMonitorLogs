using System.Net.Http.Headers;
using Microsoft.Identity.Client;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal class AzureMonitorLogsAadAuthorizationHandler : DelegatingHandler
    {
        private readonly ServiceClientAadAuthorizationOptions _authorizationOptions;

        public AzureMonitorLogsAadAuthorizationHandler(ServiceClientOptions options)
        {
            _authorizationOptions = (ServiceClientAadAuthorizationOptions)options.Authorization;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorityUri = new Uri(_authorizationOptions.AuthorityBaseUri, _authorizationOptions.TenantId.ToString());
            var application = ConfidentialClientApplicationBuilder.Create(_authorizationOptions.ClientId.ToString())
                .WithClientSecret(_authorizationOptions.ClientSecret)
                .WithAuthority(authorityUri)
            .Build();

            var token = await application.AcquireTokenForClient(new[] { _authorizationOptions.Audience }).ExecuteAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
