using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;
using PetTrack.Core.Models;

namespace PetTrack.Services.Services
{
    public class PayOSService
    {
        private readonly HttpClient _httpClient;
        private readonly PayOSOptions _options;

        public PayOSService(HttpClient httpClient, IOptions<PayOSOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api-merchant.payos.vn/");
            _options = options.Value;
        }

        public async Task<string> CreatePaymentLinkAsync(decimal amount, string description, string returnUrl)
        {
            var orderCode = new Random().Next(100000, 999999);

            var requestBody = new
            {
                orderCode = orderCode,
                amount = amount,
                description = description,
                returnUrl = "https://www.youtube.com/",
                cancelUrl = "https://www.youtube.com/",
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "v2/payment-requests")
            {
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };

            request.Headers.Add("x-client-id", _options.ClientId);
            request.Headers.Add("x-api-key", _options.ApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var payUrl = doc.RootElement.GetProperty("data").GetProperty("checkoutUrl").GetString();

            return payUrl!;
        }
    }

}
