using Cryptocurrencies.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace Cryptocurrencies.MVVM.ViewModel
{
    class CoinsViewModel : ObservableObject
    {
        private Root coins { get; set; }
        public ICollection<Coin> Coins => coins.Data;
        public Coin SelectedCoin { get; set; }

        private int _limit;
        public int Limit
        {
            get { return _limit; }
            set
            {
                _limit = value;
                SetCoins();
            }
        }

        public CoinsViewModel()
        {
            Limit = 10;
        }
        public bool Search(string query)
        {
            if (query != null && query != "")
            {
                List<Coin> searchCoin = coins.Data.Where(c => c.id == query.ToLower() || c.name == query || c.rank == query || c.symbol == query.ToUpper()).ToList();
                if (searchCoin.Count > 0)
                {
                    SelectedCoin= searchCoin[0];
                    return true;
                }
                else
                {
                    MessageBox.Show("Nothing found");
                }
            }
            return false;

        }

        public async void SetCoins()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($@"https://api.coincap.io/v2/assets?limit={Limit}");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        coins = JsonConvert.DeserializeObject<Root>(responseBody);

                        SelectedCoin=coins.Data.FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
