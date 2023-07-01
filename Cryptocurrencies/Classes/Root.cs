using PropertyChanged;
using System.Collections.ObjectModel;

namespace Cryptocurrencies
{
    [AddINotifyPropertyChangedInterface]
    public class Root
    {
        public ObservableCollection<Coin> Data { get; set; }
    }
}
