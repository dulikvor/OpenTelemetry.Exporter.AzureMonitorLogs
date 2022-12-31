using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal static class HttpClientHelperExtension
    {
        public static async Task<TResponse> PostWithJsonAsync<TResponse>(
            this HttpClient httpClient,
            string uri,
            object body,
            Tuple<string, string>[] requestHeadersPairs = null!)
        {
            HttpResponseMessage response = null!;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, new Uri(httpClient.BaseAddress!, uri));
                foreach (var headerPair in requestHeadersPairs ?? Array.Empty<Tuple<string, string>>())
                {
                    request.Headers.Add(headerPair.Item1, headerPair.Item2);
                }

                request.Content = GetHttpStringContent(body);
                response = await httpClient.SendAsync(request);

                await ValidateResponseAsync(uri, response);
                return await response.StreamWithJsonAsync<TResponse>();
            }
            finally
            {
                response?.Dispose();
            }

        }

        private static StringContent GetHttpStringContent(object body)
        {
            if (body == null)
            {
                return null!;
            }

            return new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
        }

        private static async Task ValidateResponseAsync(string uri, HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var detailedErrorMessage = await response.GetContentStringAsync();
                detailedErrorMessage = string.IsNullOrEmpty(detailedErrorMessage) ? string.Empty : $", due to reason - [{detailedErrorMessage}]";

                throw new HttpResponseException(
                    response.StatusCode,
                    $"HttpClient: Response status code does not indicate success - {(int)response.StatusCode} ({response.ReasonPhrase}), for request - {uri}{detailedErrorMessage}.");
            }
        }
    }
}
