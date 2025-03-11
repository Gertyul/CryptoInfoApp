using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoInfoApp.Models
{
    // Model for cryptocurrency (used on the main page and in search)
    public class Currency
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketCap { get; set; }
        public decimal PriceChangePercentage24h { get; set; }
    }
}