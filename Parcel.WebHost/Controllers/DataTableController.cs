using Microsoft.AspNetCore.Mvc;
using Parcel.Shared;
using Parcel.Shared.DataTypes;

namespace Parcel.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataTableController : ControllerBase
    {
        [HttpGet]
        public string GetTable(string tableName)
        {
            if (WebHostRuntime.Singleton.DataTableEndPoints.ContainsKey(tableName))
            {
                DataGrid dataGrid = WebHostRuntime.Singleton.DataTableEndPoints[tableName];
                return dataGrid.ToCSV();
            }

            return "Message\nNotFound";
        }
    }
}