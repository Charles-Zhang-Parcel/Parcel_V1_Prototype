using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProtoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        // POST api/<API>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // Serialize input value (maybe JSON) into a graph, identified also with client ID
        }
    }
}
