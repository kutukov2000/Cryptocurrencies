using Cryptocurrencies.Core;
using PropertyChanged;

namespace Cryptocurrencies.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    class MainViewModel
    {
        public RelayCommand CoinsViewCommand { get; set; }
        public RelayCommand AllCoinsViewCommand { get; set; }
        public RelayCommand CoinInfoViewCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

        public CoinsViewModel CoinsVM { get; set; }
        public CoinInfoViewModel CoinInfoVM { get; set; }

        public object CurrentView { get;set; }

        public string SearchText { get; set; }

        public MainViewModel()
        {
            CoinsVM = new CoinsViewModel();

            CoinInfoVM = new CoinInfoViewModel();

            CurrentView = CoinsVM;

            CoinsViewCommand = new RelayCommand(o =>
            {
                CoinsVM.Limit = 10;
                CurrentView = CoinsVM;
            });

            AllCoinsViewCommand = new RelayCommand(o =>
            {
                CoinsVM.Limit = 200;
                CurrentView = CoinsVM;
            });

            CoinInfoViewCommand = new RelayCommand(o =>
            {
                CurrentView = CoinInfoVM;
                CoinInfoVM.Coin = CoinsVM.SelectedCoin;
                CoinInfoVM.LoadImage();
                CoinInfoVM.LoadPrice();
            });

            SearchCommand = new RelayCommand(o =>
            {
                if (CoinsVM.Search(SearchText))
                {
                    CurrentView = CoinInfoVM;
                    CoinInfoVM.Coin  = CoinsVM.SelectedCoin;
                    CoinInfoVM.LoadImage();
                    CoinInfoVM.LoadPrice();
                }
            });
        }
    }
}
