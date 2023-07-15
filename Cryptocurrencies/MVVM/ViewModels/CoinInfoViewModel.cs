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
        private CoinCapService _coinCapService =new CoinCapService(new HttpClient());
        private CoinGeckoService _coinGeckoService = new CoinGeckoService(new HttpClient());
        public PlotModel PlotModel { get; set; }
        public Coin Coin {get; set; }
        public async void DrawPlot()
        {
            await Task.Run(() =>
            {
                //Create PlotModel and Set styles
                PlotModel = new PlotModel
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
                    TicklineColor =OxyColors.Black
                };
                PlotModel.Axes.Add(dateTimeAxis);

                var valueAxis = new LinearAxis
                {
                    Title = "Price (USD)",
                    Position = AxisPosition.Left,
                    TitleFontSize = 0,
                    TicklineColor = OxyColors.Black
                };
                PlotModel.Axes.Add(valueAxis);

                //Add Series and Values
                var Series = new LineSeries();

                foreach (var item in Coin.Prices)
                {
                    Series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.Date), item.PriceUsd));
                }

                PlotModel.Series.Add(Series);
            });
        }

        public async void LoadPrice()
        {
            if (Coin != null)
            {
                Coin.Prices = await _coinCapService.GetCryptocurrencyPrice(Coin.Id);
                DrawPlot();
            }
        }

        public async void LoadImage()
        {
            if (Coin != null)
            {
                Coin.Image = await _coinGeckoService.GetCryptocurrencyImage(Coin.Id);
            }
        }
    }

}

