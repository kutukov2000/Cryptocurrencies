using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cryptocurrencies
{
    [AddINotifyPropertyChangedInterface]
    public class Coin
    {
        //public string id { get; set; }
        public string rank { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string priceusd { get; set; }
        //public string supply { get; set; }
        //public string maxsupply { get; set; }
        public string marketcapusd { get; set; }
        public string volumeusd24hr { get; set; }
        public string changepercent24hr { get; set; }
        public string vwap24hr { get; set; }
        //public string explorer { get; set; }
        public bool isPositive => double.Parse(changepercent24hr) > 0;
    }
    [AddINotifyPropertyChangedInterface]
    public class Root
    {
        public List<Coin> Data { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class ViewModel
    {
        private Root coins { get; set; }
        public ICollection<Coin> Coins => coins.Data;

        public void SetCoins(string jsonString)
        {
            coins = JsonConvert.DeserializeObject<Root>(jsonString);
        }
    }

    public partial class MainWindow : Window
    {
        ViewModel model = new ViewModel();
        public MainWindow()
        {
            InitializeComponent();

            DataContext = model;

            GetData();
        }

        public async void GetData()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.coincap.io/v2/assets?limit=10");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        model.SetCoins(responseBody);
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
