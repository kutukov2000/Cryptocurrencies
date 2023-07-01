using System;
using System.Collections.Generic;

namespace Cryptocurrencies.MVVM.ViewModel
{
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

}

