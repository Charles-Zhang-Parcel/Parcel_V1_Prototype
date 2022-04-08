using System;
using Parcel.Toolbox.DataSource;

namespace Parcel.SetupTest.DataSourceTest001
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSourceHelper.YahooFinance(new YahooFinanceParameter()
            {
                InputSymbol = "AAPL",
            });
        }
    }
}