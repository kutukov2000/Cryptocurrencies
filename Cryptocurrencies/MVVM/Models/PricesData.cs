using System;
using System.Collections.Generic;

namespace Cryptocurrencies.MVVM.ViewModel
{
    public class PricesData
    {
        public class PriceInfo
        {
            public string PriceUsd { get; set; }
            public DateTime Date { get; set; }
        }
        public List<PriceInfo> Data { get; set; }
    }

}

