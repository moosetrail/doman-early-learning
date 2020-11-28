using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EarlyLearning.API.Controllers.ReadingProgram
{
    [Route("api/v1/reading-program")]
    [ApiController]
    public class ReadingProgramController : ControllerBase
    {
        public ReadingProgramController() {}

        [HttpGet]
        [Route("all")]
        public Task<IActionResult> GetReadingProgramsForUser()
        {
            return null;
        }
    }
}
