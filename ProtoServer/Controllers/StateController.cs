using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProtoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        // GET api/<State>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            // Return CSV value stored per state of input
            return "value";
        }
    }
}
