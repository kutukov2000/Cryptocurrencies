using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrencies
{
    public class CoinGeckoService
    {
        private readonly HttpClient _httpClient;

        public CoinGeckoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
        }

        public async Task<string> GetCryptocurrencyImage(string cryptocurrencyId)
        {
            string apiUrl = $"coins/{cryptocurrencyId}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject obj = JObject.Parse(responseBody);

                return (string)obj["image"]["large"];
            }
            else
            {
                return @"https://cdn-icons-png.flaticon.com/128/7542/7542854.png";
            }

            throw new Exception("Failed to retrieve cryptocurrency image.");
        }
    }
}
