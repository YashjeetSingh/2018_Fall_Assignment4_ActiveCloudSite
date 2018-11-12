using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IEXTrading.Models
{
    public class StocksDetails
    {
        public decimal dividendRate { get; set; }
        public decimal ytdChangePercent { get; set; }
        public decimal grossProfit { get; set; }
        public string symbol { get; set; }
        public string companyName { get; set; }
    }
}
