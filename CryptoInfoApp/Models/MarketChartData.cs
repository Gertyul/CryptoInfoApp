using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoInfoApp.Models
{
    public class MarketChartData
    {
        // Масив масивів: [timestamp, price]
        public List<List<decimal>> Prices { get; set; }
        
    }
}

