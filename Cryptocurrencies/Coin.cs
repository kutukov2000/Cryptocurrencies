using PropertyChanged;

namespace Cryptocurrencies
{
    [AddINotifyPropertyChangedInterface]
    public class Coin
    {
        public string id { get; set; }
        public string rank { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public double priceusd { get; set; }
        public string supply { get; set; }
        public string maxsupply { get; set; }
        public double marketcapusd { get; set; }

        public double MarketCapB => marketcapusd / 1000000000;

        public string volumeusd24hr { get; set; }
        public string changepercent24hr { get; set; }
        public string vwap24hr { get; set; }
        public string explorer { get; set; }
        //public bool isPositive => double.Parse(changepercent24hr) > 0;
    }
}
