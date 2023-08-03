using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace Cryptocurrencies.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    class CoinsViewModel
    {
        private CoinCapService _coinCapService = new CoinCapService(new HttpClient());
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
            _coinCapService = new CoinCapService(new HttpClient());
            Limit = 10;
        }
        public bool Search(string query)
        {
            if (query != null && query != "")
            {
                Coin searchCoin = Coins.Where(c => c.Id == query.ToLower() || c.Name == query || c.Rank == query || c.Symbol == query.ToUpper()).FirstOrDefault();
                if (searchCoin != null)
                {
                    SelectedCoin = searchCoin;
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
            Coins = await _coinCapService.GetCryptocurrencyData(Limit);
            SelectedCoin = Coins.FirstOrDefault();
        }
    }
}
