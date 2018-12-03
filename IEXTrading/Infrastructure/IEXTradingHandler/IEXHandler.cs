using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using IEXTrading.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace IEXTrading.Infrastructure.IEXTradingHandler
{
    public class IEXHandler
    {
        static string BASE_URL = "https://api.iextrading.com/1.0/"; //This is the base URL, method specific URL is appended to this.
        HttpClient httpClient;

        public IEXHandler()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /****
         * Calls the IEX reference API to get the list of symbols. 
        ****/
        public List<Company> GetSymbols()
        {
            string IEXTrading_API_PATH = BASE_URL + "ref-data/symbols";
            string companyList = "";

            List<Company> companies = null;

            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                companyList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!companyList.Equals(""))
            {
                companies = JsonConvert.DeserializeObject<List<Company>>(companyList);
                companies = companies.GetRange(0, 100);
            }
            return companies;
        }

        /****
         * Calls the IEX stock API to get 1 year's chart for the supplied symbol. 
        ****/
        public List<Equity> GetChart(string symbol)
        {
            //Using the format method.
            //string IEXTrading_API_PATH = BASE_URL + "stock/{0}/batch?types=chart&range=1y";
            //IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, symbol);

            string IEXTrading_API_PATH = BASE_URL + "stock/" + symbol + "/batch?types=chart&range=1y";

            string charts = "";
            List<Equity> Equities = new List<Equity>();
            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                charts = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            if (!charts.Equals(""))
            {
                ChartRoot root = JsonConvert.DeserializeObject<ChartRoot>(charts, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Equities = root.chart.ToList();
            }
            //make sure to add the symbol the chart
            foreach (Equity Equity in Equities)
            {
                Equity.symbol = symbol;
            }

            return Equities;
        }

        public StocksDetails GetResultsFromAPI(string symbol)
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/" + symbol + "/stats";
            string _stocklist = "";
            StocksDetails sd = new StocksDetails();

            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                _stocklist = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            var jsonObject = (JObject)JsonConvert.DeserializeObject(_stocklist);

            if ((jsonObject.Property("dividendRate") != null) && (!jsonObject.Property("dividendRate").Value.ToString().Equals("")))
            {
                //test = jsonObject.Property("dividendRate").Value.ToString();
                sd.dividendRate = double.Parse(jsonObject.Property("dividendRate").Value.ToString(), CultureInfo.InvariantCulture);
            }

            if ((jsonObject.Property("ytdChangePercent") != null) && (!jsonObject.Property("ytdChangePercent").Value.ToString().Equals("")))
            {
                //test = jsonObject.Property("ytdChangePercent").Value.ToString();
                sd.dividendRate = double.Parse(jsonObject.Property("ytdChangePercent").Value.ToString(), CultureInfo.InvariantCulture);
            }

            if ((jsonObject.Property("grossProfit") != null) && (!jsonObject.Property("grossProfit").Value.ToString().Equals("")))
            {
                //test = jsonObject.Property("grossProfit").Value.ToString();
                sd.dividendRate = double.Parse(jsonObject.Property("grossProfit").Value.ToString(), CultureInfo.InvariantCulture);
            }

            if (jsonObject.Property("companyName") != null)
            {
                //test = jsonObject.Property("companyName").Value.ToString();
                sd.companyName = jsonObject.Property("companyName").Value.ToString();
            }

            sd.symbol = symbol;

            return sd;

        }

        public List<StockStats> InFocus()
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/market/list/infocus";
            string _stocklist = "";
            List<StockStats> _list = null;
            StocksDetails sd = new StocksDetails();

            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                _stocklist = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!_stocklist.Equals(""))
            {
                _list = JsonConvert.DeserializeObject<List<StockStats>>(_stocklist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            return _list;
        }

        public List<StockStats> Gainers()
        {
            string IEXTrading_API_PATH = BASE_URL + "stock/market/list/gainers";
            string _stocklist = "";
            List<StockStats> _list = null;
            StocksDetails sd = new StocksDetails();

            httpClient.BaseAddress = new Uri(IEXTrading_API_PATH);
            HttpResponseMessage response = httpClient.GetAsync(IEXTrading_API_PATH).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                _stocklist = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }

            if (!_stocklist.Equals(""))
            {
                _list = JsonConvert.DeserializeObject<List<StockStats>>(_stocklist, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            return _list;
        }

    }
}
