using System;
using System.Collections.Generic;

namespace Cryptocurrencies.MVVM.ViewModel
{
    public class PricesData
    {
        public class PriceInfo
        {
            public string priceUsd { get; set; }
            public object time { get; set; }
            public DateTime date { get; set; }
        }
        public List<PriceInfo> data { get; set; }
        public long timestamp { get; set; }
    }

}

