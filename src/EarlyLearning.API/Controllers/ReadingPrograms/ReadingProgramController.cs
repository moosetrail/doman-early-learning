using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    [Route("api/v1/reading-program")]
    [ApiController]
    public class ReadingProgramController : ApiControllerBase
    {
        public ReadingProgramController(ILogger logger) 
            : base(logger.ForContext<ReadingProgramController>()) 
        { }

        [HttpGet]
        [Route("all")]
        public Task<IActionResult> GetReadingProgramsForUser()
        {
            return null;
        }
    }
}
