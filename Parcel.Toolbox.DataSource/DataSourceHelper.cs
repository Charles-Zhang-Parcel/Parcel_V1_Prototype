using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Csv;
using Parcel.Shared.DataTypes;

namespace Parcel.Toolbox.DataSource
{
    public class YahooFinanceParameter
    {
        // TODO: For all practical usage, we should all just use the default
        public string InputSymbol { get; set; }
        // public string[] InputFields { get; set; }
        // public string InputAPIKey { get; set; } // Yahoo REST API is useless compared to its website, with too many restrictions and too limited capabilities
        public DateTime InputStartTime { get; set; }
        public DateTime InputEndTime { get; set; }
        public string InputInterval { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    
    public static class DataSourceHelper
    {
        public static void YahooFinance(YahooFinanceParameter parameter)
        {
            string csvUrl =
                $"https://query1.finance.yahoo.com/v7/finance/download/{parameter.InputSymbol}?period1=1245196800&period2=1649376000&interval=1d&events=history&includeAdjustedClose=true";
            string csvText = new WebClient().DownloadString(csvUrl);
            IEnumerable<ICsvLine> csv = Csv.CsvReader.ReadFromText(csvText, new CsvOptions()
            {
                HeaderMode = HeaderMode.HeaderPresent
            });
            parameter.OutputTable = new DataGrid(csv);
        }

        public static void YahooFinanceOLDANDNOTWORKING(YahooFinanceParameter parameter)
        {
            var httpClient = new HttpClient();
                        httpClient.BaseAddress = new Uri("https://yfapi.net/");
                        httpClient.DefaultRequestHeaders.Add("X-API-KEY", 
                            /*parameter.InputAPIKey*/"Useless");
                        httpClient.DefaultRequestHeaders.Add("accept", 
                            "application/json");
            
                        // HttpResponseMessage response = /*await*/ httpClient.GetAsync(
                        //     $"v11/finance/quoteSummary/{parameter.InputSymbol}?lang=en&region=US&modules=defaultKeyStatistics%2CassetProfile").Result;
                        HttpResponseMessage response = /*await*/ httpClient.GetAsync(
                            $"v11/finance/quote/{parameter.InputSymbol}?lang=en&region=US&modules=defaultKeyStatistics%2CassetProfile").Result;
                        string responseBody = /*await*/ response.Content.ReadAsStringAsync().Result;
        }
    }
}