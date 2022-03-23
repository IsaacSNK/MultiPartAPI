using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiPartAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormDataController : ControllerBase
    {
        [HttpGet]
        [Produces("application/xml")]
        public IActionResult Get()
        {
            var stringContent = new StringContent("Hola");
            return Ok(stringContent);
        }
    }
}
