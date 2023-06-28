using Cryptocurrencies.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
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
                    }
                    else
                    {
                        MessageBox.Show("Error");
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
