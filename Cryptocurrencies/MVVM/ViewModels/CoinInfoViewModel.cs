using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocurrencies.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    class CoinInfoViewModel
    {
        private CoinCapService _coinCapService = new CoinCapService(new HttpClient());
        private CoinGeckoService _coinGeckoService = new CoinGeckoService(new HttpClient());

        private string[] _intervals = { "1m", "5m", "15m", "30m", "1h", "2h", "6h", "12h", "1d" };
        public string[] Intervals
        {
            get { return _intervals; }
        }
        private string _selectedInterval { get; set; }
        public string SelectedInterval
        {
            get
            {
                return _selectedInterval;
            }
            set
            {
                _selectedInterval = value;
                LoadPriceAsync();
            }
        }
        public string ConvertedInterval => ConvertInterval();
        private string ConvertInterval()
        {
            if (SelectedInterval.Length == 2)
            {
                return SelectedInterval.Substring(1) + SelectedInterval[0];
            }
            else if (SelectedInterval.Length == 3)
            {
                return SelectedInterval.Substring(2) + SelectedInterval.Substring(0, 2);
            }
            return "d1";
        }

        public PlotModel PlotModel { get; set; }

        private Coin _coin;

        public Coin Coin
        {
            get { return _coin; }
            set
            {
                _coin = value;
                LoadImageAsync();
                LoadPriceAsync();
            }
        }
        public CoinInfoViewModel()
        {
            SelectedInterval = "1d";
        }
        public async Task DrawPlotAsync()
        {
            if (Coin is null || Coin.Prices is null || Coin.Prices.Count == 0)
            {
                return;
            }

            await Task.Run(() =>
            {
                //Create PlotModel and Set styles
                var plotModel = new PlotModel
                {
                    TextColor = OxyColors.Black,
                    PlotAreaBorderColor = OxyColors.Black,
                };

                //Add Axes
                var dateTimeAxis = new DateTimeAxis
                {
                    Title = "Date",
                    TitleFontSize = 0,
                    Position = AxisPosition.Bottom,
                    StringFormat = "yyyy-MM-dd",
                    TicklineColor = OxyColors.Black
                };
                plotModel.Axes.Add(dateTimeAxis);

                var valueAxis = new LinearAxis
                {
                    Title = "Price (USD)",
                    Position = AxisPosition.Left,
                    TitleFontSize = 0,
                    TicklineColor = OxyColors.Black
                };
                plotModel.Axes.Add(valueAxis);

                //Add Series and Values
                var lineSeries = new LineSeries();

                foreach (var item in Coin.Prices)
                {
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), item.PriceUsd));
                }

                plotModel.Series.Add(lineSeries);

                PlotModel = plotModel;
            });
        }
        public async Task LoadPriceAsync()
        {
            if (Coin != null)
            {
                Coin.Prices = await _coinCapService.GetCryptocurrencyPrice(Coin.Id, ConvertedInterval);
                await DrawPlotAsync();
            }
        }

        public async Task LoadImageAsync()
        {
            if (Coin != null)
            {
                Coin.Image = await _coinGeckoService.GetCryptocurrencyImage(Coin.Id);
            }
        }
    }
}