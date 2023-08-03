using Cryptocurrencies.MVVM.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrencies
{
    public class CoinCapService
    {
        private readonly HttpClient _httpClient;

        public CoinCapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.coincap.io/v2/");
        }
        public async Task<ObservableCollection<Coin>> GetCryptocurrencyData(int limit)
        {
            string apiUrl = $"assets?limit={limit}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseBody);
                var coinsData = jsonResponse["data"].ToObject<ObservableCollection<Coin>>();

                return coinsData;
            }

            throw new Exception("Failed to retrieve cryptocurrency data.");
        }

        public async Task<List<Price>> GetCryptocurrencyPrice(string cryptocurrencyId)
        {
            string apiUrl = $"assets/{cryptocurrencyId}/history?interval=d1";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                var prices = JsonConvert.DeserializeObject<JObject>(responseBody)?["data"];
                if (prices != null)
                {
                    var result = new List<Price>();

                    foreach (var item in prices.Children())
                    {
                        var priceUsdToken = item.SelectToken("priceUsd");
                        var dateToken = item.SelectToken("date");

                        if (priceUsdToken != null && dateToken != null)
                        {
                            var price = new Price
                            {
                                PriceUsd = priceUsdToken.Value<double>(),
                                Date = dateToken.Value<DateTime>()
                            };
                            result.Add(price);
                        }
                    }

                    return result;
                }
            }

            throw new Exception("Failed to retrieve cryptocurrency price.");
        }
    }
}
