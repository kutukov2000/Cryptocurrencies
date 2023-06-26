using Cryptocurrencies.Core;

namespace Cryptocurrencies.MVVM.ViewModel
{
    class MainViewModel:ObservableObject
    {
        public RelayCommand CoinsViewCommand { get; set; }
        public RelayCommand AllCoinsViewCommand { get; set; }
        public RelayCommand CoinInfoViewCommand { get; set; }

        public CoinsViewModel CoinsVM { get; set; }
        private CoinInfoViewModel CoinInfoVM { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            CoinsVM = new CoinsViewModel();
            CoinInfoVM = new CoinInfoViewModel();

            CurrentView = CoinsVM;

            CoinsViewCommand = new RelayCommand(o =>
            {
                CurrentView = CoinsVM;
                CoinsVM.Limit = 10;
            });

            AllCoinsViewCommand = new RelayCommand(o =>
            {
                CurrentView = CoinsVM;
                CoinsVM.Limit = 200;
            });

            CoinInfoViewCommand = new RelayCommand(o =>
            {
                CurrentView = CoinInfoVM;
            });
        }
    }
}
