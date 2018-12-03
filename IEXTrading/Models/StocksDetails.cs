using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IEXTrading.Models
{
    public class StocksDetails
    {
        public double dividendRate { get; set; }
        public double ytdChangePercent { get; set; }
        public double grossProfit { get; set; }
        public string symbol { get; set; }
        public string companyName { get; set; }
    }
}
