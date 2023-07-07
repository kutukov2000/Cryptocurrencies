using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace Cryptocurrencies.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    class CoinsViewModel
    {
        public ObservableCollection<Coin> Coins { get; set; }
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
                Coin searchCoin = Coins.Where(c => c.Id == query.ToLower() || c.Name == query || c.Rank == query || c.Symbol == query.ToUpper()).FirstOrDefault();
                if (searchCoin !=null)
                {
                    SelectedCoin= searchCoin;
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

                        Coins = JsonConvert.DeserializeObject<CoinsData>(responseBody).Data;

                        SelectedCoin=Coins.FirstOrDefault();
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
