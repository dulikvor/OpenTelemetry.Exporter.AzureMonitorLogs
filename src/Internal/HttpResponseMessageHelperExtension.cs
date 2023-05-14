using System.Text.Json;

namespace OpenTelemetry.Exporter.AzureMonitorLogs.Internal
{
    internal static class HttpResponseMessageHelperExtension
    {
        public static async Task<T> StreamWithJsonAsync<T>(this HttpResponseMessage response)
        {
            if (response?.Content != null)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();
                return JsonSerializer.Deserialize<T>(contentStream)!;
            }
            return default!;
        }

        public static async Task<string> GetContentStringAsync(this HttpResponseMessage response)
        {
            if (response.Content == null)
            {
                return null!;
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            {
                if (stream == null)
                {
                    return null!;
                }

                using (var sr = new StreamReader(stream))
                {
                    return await sr.ReadToEndAsync();
                }
            }
        }
    }
}
