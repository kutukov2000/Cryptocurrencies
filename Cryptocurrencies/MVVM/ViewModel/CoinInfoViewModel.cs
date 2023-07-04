using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Cryptocurrencies.MVVM.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    class CoinInfoViewModel 
    {
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
                        HttpResponseMessage response = await client.GetAsync($@"https://api.coincap.io/v2/assets/{Coin.Id}/history?interval=d1");
                        if (response.IsSuccessStatusCode)
                        {
                            var responseBody = await response.Content.ReadAsStringAsync();

                            PricesData myDeserializedClass = JsonConvert.DeserializeObject<PricesData>(responseBody);

                            Coin.Prices = new List<Price>();

                            foreach (var item in myDeserializedClass.data)
                            {
                                Coin.Prices.Add(new Price
                                {
                                    priceUsd = double.Parse(item.priceUsd.Substring(0, item.priceUsd.IndexOf("."))),
                                    date = item.date
                                });
                            }
                            DrawPlot();
                        }
                        else
                        {
                            MessageBox.Show($"Price Error, coin={Coin.Id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Price Error, coin={Coin.Id}");
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
                    HttpResponseMessage response = await client.GetAsync($@"https://api.coingecko.com/api/v3/coins/{Coin.Id}?tickers=false&market_data=false&community_data=false&developer_data=false&sparkline=false");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        JObject obj = JObject.Parse(responseBody);

                        Coin.Image = (string)obj["image"]["large"];
                    }
                    else
                    {
                        Coin.Image = @"https://cdn-icons-png.flaticon.com/128/7542/7542854.png";
                    }
                }
            }
        }
    }

}

