using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Csv;
using Parcel.Shared.DataTypes;

namespace Parcel.Toolbox.DataSource
{
    public class YahooFinanceParameter
    {
        public string InputSymbol { get; set; }
        public DateTime InputStartDate { get; set; }
        public DateTime InputEndDate { get; set; }
        public string InputInterval { get; set; }
        public DataGrid OutputTable { get; set; }
    }
    
    public static class DataSourceHelper
    {
        public static void YahooFinance(YahooFinanceParameter parameter)
        {
            string ConvertTimeFormat(DateTime input)
            {
                // Tiemzone info: https://stackoverflow.com/questions/5996320/net-timezoneinfo-from-olson-time-zone
                // { "America/New_York", "Eastern Standard Time" },
                var americaNewYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                var newYorkTime = TimeZoneInfo.ConvertTimeFromUtc(input, americaNewYorkTimeZone);
                string timeStamp = ((DateTimeOffset) newYorkTime).ToUnixTimeSeconds().ToString();
                return timeStamp;   // TODO: NOT WORKING
            }
            
            Dictionary<string, string> validIntervals = new Dictionary<string, string>()
            {
                {"month", "1m"},
                {"day", "1d"},
                {"week", "1w"},
                {"year", "1y"},
            };
            if (parameter.InputStartDate > parameter.InputEndDate)
                throw new ArgumentException("Wrong date.");
            if (parameter.InputEndDate > DateTime.Now)
                throw new ArgumentException("Wrong date.");
            if (parameter.InputSymbol.Length > 5)
                throw new ArgumentException("Wrong symbol.");
            if (!validIntervals.Keys.Contains(parameter.InputInterval.ToLower()))
                throw new ArgumentException("Wrong interval.");

            string startTime = ConvertTimeFormat(parameter.InputStartDate);
            string endTime = ConvertTimeFormat(parameter.InputEndDate);
            string interval = validIntervals[parameter.InputInterval.ToLower()];
            string csvUrl =
                $"https://query1.finance.yahoo.com/v7/finance/download/{parameter.InputSymbol}?period1={startTime}&period2={endTime}&interval={interval}&events=history&includeAdjustedClose=true";
            string csvText = new WebClient().DownloadString(csvUrl);
            IEnumerable<ICsvLine> csv = Csv.CsvReader.ReadFromText(csvText, new CsvOptions()
            {
                HeaderMode = HeaderMode.HeaderPresent
            });
            parameter.OutputTable = new DataGrid(csv);
        }
    }
}