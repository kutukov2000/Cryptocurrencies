using PropertyChanged;
using System.Collections.ObjectModel;

namespace Cryptocurrencies
{
    [AddINotifyPropertyChangedInterface]
    public class CoinsData
    {
        public ObservableCollection<Coin> Data { get; set; }
    }
}
