using Cryptocurrencies.MVVM.ViewModel;
using PropertyChanged;
using System.Collections.Generic;

namespace Cryptocurrencies
{
    [AddINotifyPropertyChangedInterface]
    public class Coin
    {
        public string Id { get; set; }
        public string Rank { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public double Priceusd { get; set; }
        public double Supply { get; set; }
        public double Marketcapusd { get; set; }
        public double MarketCapB => Marketcapusd / 1000000000;
        public double Volumeusd24hr { get; set; }
        public double Changepercent24hr { get; set; }
        public bool isPositive => Changepercent24hr > 0;
        public string Image { get; set; }
        public List<Price> Prices {get; set;}
    }
}
