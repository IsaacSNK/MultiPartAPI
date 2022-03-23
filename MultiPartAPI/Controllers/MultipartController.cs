using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MultiPartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultipartController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public MultipartController(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        [HttpGet]
        public MultipartResult Get()
        {
            return new MultipartResult()
            {
                new MultipartContent()
                {
                    ContentType = "text/plain",
                    FileName = "File.txt",
                    Stream = this.OpenFile("File.txt")
                },
                new MultipartContent()
                {
                    ContentType = "text/plain",
                    FileName = "File.json",
                    Stream = this.OpenFile("File.json")
                }
            };
        }

        private Stream OpenFile(string relativePath)
        {
            return System.IO.File.Open(
                Path.Combine(this._environment.ContentRootPath, relativePath),
                FileMode.Open,
                FileAccess.Read);
        }
    }
}
