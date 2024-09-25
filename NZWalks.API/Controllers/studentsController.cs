using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentsController : ControllerBase
    {
        [HttpGet]
       public IActionResult GetAllStudnet()
        {
            string[] studentsName = new string[] { "aaa", "bbb", "ccc" };

            return Ok(studentsName);

        }
    }
}
