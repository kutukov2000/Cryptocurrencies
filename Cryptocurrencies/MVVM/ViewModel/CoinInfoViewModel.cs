using Cryptocurrencies.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Cryptocurrencies.MVVM.ViewModel
{
    public class Price
    {
        public double priceUsd { get; set; }
        public DateTime date { get; set; }
    }

    public class Root2
    {
        public class Datum
        {
            public string priceUsd { get; set; }
            public object time { get; set; }
            public DateTime date { get; set; }
        }
        public List<Datum> data { get; set; }
        public long timestamp { get; set; }
    }
    class CoinInfoViewModel : ObservableObject
    {
        private PlotModel _plotModel;

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                OnPropertyChanged(nameof(PlotModel));
            }
        }

        public List<Price> Prices { get; set; }
        private Coin coin;
        public Coin Coin
        {
            get { return coin; }
            set
            {
                coin = value;
                OnPropertyChanged();
            }
        }
        public CoinInfoViewModel()
        {
            Coin = new Coin();
            Prices = new List<Price>();
        }
        public async void DrawPlot()
        {
            await Task.Run(() =>
            {
                //Create PlotModel and Set styles
                PlotModel = new PlotModel
                {
                    TextColor = OxyColors.Black,
                    PlotAreaBorderColor = OxyColors.Black,
                    //DefaultColors = OxyPalettes.Rainbow(10).Colors
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

                foreach (var item in Prices)
                {
                    Series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(item.date), item.priceUsd));
                }

                PlotModel.Series.Add(Series);
            });
        }

        public async void LoadPrice()
        {
            if (Coin != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync($@"https://api.coincap.io/v2/assets/{Coin.id}/history?interval=d1");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();

                            Root2 myDeserializedClass = JsonConvert.DeserializeObject<Root2>(responseBody);

                            Prices = new List<Price>();
                            foreach (var item in myDeserializedClass.data)
                            {
                                Prices.Add(new Price
                                {
                                    priceUsd = double.Parse(item.priceUsd.Substring(0, item.priceUsd.IndexOf("."))),
                                    date = item.date
                                });
                            }
                            DrawPlot();
                        }
                        else
                        {
                            MessageBox.Show($"Price Error, coin={Coin.id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Price Error, coin={Coin.id}");
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
            }
        }

        public async void LoadImage()
        {
            if (Coin != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($@"https://api.coingecko.com/api/v3/coins/{Coin.id}?tickers=false&market_data=false&community_data=false&developer_data=false&sparkline=false");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        JObject obj = JObject.Parse(responseBody);

                        Coin.Image = (string)obj["image"]["large"];
                    }
                    else
                    {
                        MessageBox.Show($"Image Error, coin={Coin.id}");
                        Coin.Image = @"https://cdn-icons-png.flaticon.com/128/7542/7542854.png";
                    }
                }
            }
        }
    }

}

