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

        private CoinsViewModel _coinsVM { get; set; }
        private CoinInfoViewModel _coinInfoVM { get; set; }

        public object CurrentView { get; set; }

        public string SearchText { get; set; }

        public MainViewModel()
        {
            _coinsVM = new CoinsViewModel();

            _coinInfoVM = new CoinInfoViewModel();

            CurrentView = _coinsVM;

            CoinsViewCommand = new RelayCommand(o =>
            {
                _coinsVM.Limit = 10;
                CurrentView = _coinsVM;
            });

            AllCoinsViewCommand = new RelayCommand(o =>
            {
                _coinsVM.Limit = 200;
                CurrentView = _coinsVM;
            });

            CoinInfoViewCommand = new RelayCommand(o =>
            {
                CurrentView = _coinInfoVM;
                _coinInfoVM.Coin = _coinsVM.SelectedCoin;
                _coinInfoVM.LoadImage();
                _coinInfoVM.LoadPrice();
            });

            SearchCommand = new RelayCommand(o =>
            {
                if (_coinsVM.Search(SearchText))
                {
                    CurrentView = _coinInfoVM;
                    _coinInfoVM.Coin = _coinsVM.SelectedCoin;
                    _coinInfoVM.LoadImage();
                    _coinInfoVM.LoadPrice();
                }
            });
        }
    }
}
