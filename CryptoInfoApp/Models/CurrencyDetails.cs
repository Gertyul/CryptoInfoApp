using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoInfoApp.Models
{
    // Модель для детальной информации о криптовалюте
    public class CurrencyDetails
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal PriceChangePercentage24h { get; set; }
        public string Description { get; set; }
        public string Homepage { get; set; }
    }
}
