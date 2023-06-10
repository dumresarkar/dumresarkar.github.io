using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiFileUpload.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        public IActionResult Get() => Ok("File Upload API running .....");


        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            return Ok();
        }
    }


    
}
