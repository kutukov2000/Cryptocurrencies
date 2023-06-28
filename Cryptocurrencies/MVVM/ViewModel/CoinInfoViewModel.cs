using Cryptocurrencies.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Shapes;

namespace Cryptocurrencies.MVVM.ViewModel
{
    //StackOverFlow
    //    if you need only ids, all your code you can put in one line

    //List<string> ids = JObject.Parse(response.Content)["data"]
    //                          .Select(d => (string)d["id"]).ToList();

    //    Console.WriteLine(string.Join(",", ids));
    //if you need more data you add properties you need to Coins class and show them to us, for example

    //List<Coin> coins = JObject.Parse(response.Content)["data"]
    //                          .Select(d => d.ToObject<Coin>()).ToList();

    //    public class Coin
    //    {
    //        public string id { get; set; }
    //        public string rank { get; set; }
    //        public string symbol { get; set; }
    //        public string name { get; set; }
    //        public string supply { get; set; }
    //        public string maxSupply { get; set; }
    //        public string marketCapUsd { get; set; }
    //        public string volumeUsd24Hr { get; set; }
    //        public string priceUsd { get; set; }
    //        public string changePercent24Hr { get; set; }
    //        public string vwap24Hr { get; set; }
    //        public string explorer { get; set; }
    //    }
    //    or if you want to use your code, fix the class Coins, you need a list for your json, not a dictionary

    //    public class Coins
    //    {
    //        public List<Coin> data { get; set; }
    //    }
    public class Datum
    {
        public string priceUsd { get; set; }
        public object time { get; set; }
        public DateTime date { get; set; }
    }

    public class Root2
    {
        public List<Datum> data { get; set; }
        public long timestamp { get; set; }
    }
    class CoinInfoViewModel : ObservableObject
    {
        //private PlotModel _plotModel;

        //public PlotModel PlotModel
        //{
        //    get { return _plotModel; }
        //    set
        //    {
        //        _plotModel = value;
        //        OnPropertyChanged(nameof(PlotModel));
        //    }
        //}

        public List<string> Prices { get; set; }

        private Coin coin;
        public Coin Coin
        {
            get { return coin; }
            set
            {
                coin = value;
                OnPropertyChanged();
                //LoadImage();
                //LoadPrice();
            }
        }
        public CoinInfoViewModel()
        {
            Coin = new Coin();
            Prices = new List<string>();
        }

        //public void DrawPlot()
        //{
        //    PlotModel = new PlotModel { Title = "Ціна" };

        //    // Create the LineSeries
        //    var lineSeries = new LineSeries();
        //    lineSeries.Title = "Ціна";

        //    lineSeries.Points.Add(new OxyPlot.DataPoint(1, 20708.2486150840481190));
        //    lineSeries.Points.Add(new OxyPlot.DataPoint(2, 20145.9601688888835710));
        //    lineSeries.Points.Add(new OxyPlot.DataPoint(3, 19349.0087527762090611));
        //    lineSeries.Points.Add(new OxyPlot.DataPoint(4, 19532.8435091213185286));

        //    //Add the LineSeries to the PlotModel
        //    PlotModel.Series.Add(lineSeries);
        //}


        public async void LoadPrice()
        {
            //if (Coin != null)
            //{
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($@"https://api.coincap.io/v2/assets/{Coin.id}/history?interval=d1");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        Root2 myDeserializedClass = JsonConvert.DeserializeObject<Root2>(responseBody);

                        string priceString = "";


                        for (int i = 0; i < myDeserializedClass.data.Count; i++)
                        {
                            if (myDeserializedClass.data[i].priceUsd.Length > 0)
                            {
                                myDeserializedClass.data[i].priceUsd = myDeserializedClass.data[i].priceUsd.Substring(0, myDeserializedClass.data[i].priceUsd.Length - 8);
                            }
                        }

                        foreach (var price in myDeserializedClass.data)
                        {
                            priceString += price.priceUsd + " ";
                        }

                        MessageBox.Show(priceString);


                        double price1 = double.Parse(myDeserializedClass.data[1].priceUsd);

                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
                //}
            }
        }


        //public async void LoadPrice()
        //{
        //    if (Coin != null)
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            try
        //            {
        //                HttpResponseMessage response = await client.GetAsync($@"https://api.coincap.io/v2/assets/{Coin.id}/history?interval=d1");
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    var responseBody = await response.Content.ReadAsStringAsync();

        //                    //Prices = JObject.Parse(responseBody)["data"].Select(d => (string)d["priceUsd"]).ToList();
        //                    JArray data = JArray.Parse(responseBody)["data"] as JArray;
        //                    Prices = data.Select(d => (string)d["priceUsd"]).ToList();


        //                    string PriceString = "";
        //                    foreach (var item in Prices)
        //                    {
        //                        PriceString += item + " ";
        //                    }
        //                    MessageBox.Show(PriceString);

        //                    //for (int i = 0;i< root.DataPoints.Count; i++)
        //                    //{
        //                    //    //lineSeries.Points.Add(new OxyPlot.DataPoint(i, Convert.ToDouble(string.Format("{0:0.00}", root.DataPoints[i].priceUsd))));

        //                    //}
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Error");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"An error occurred: {ex.Message}");
        //            }
        //        }
        //    }
        //}

        public async void LoadImage()
        {
            if (Coin != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    try
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
                            MessageBox.Show("Error");
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

}

