using Cryptocurrencies.Core;

namespace Cryptocurrencies.MVVM.ViewModel
{
    class MainViewModel:ObservableObject
    {
        public RelayCommand CoinsViewCommand { get; set; }
        public RelayCommand AllCoinsViewCommand { get; set; }
        public RelayCommand CoinInfoViewCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }

        public CoinsViewModel CoinsVM { get; set; }
        public CoinInfoViewModel CoinInfoVM { get; set; }


        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

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
                CoinsVM.Search(SearchText);
            });
        }
    }
}
