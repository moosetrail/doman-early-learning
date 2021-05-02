using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.API.Dataclasses.User;
using EarlyLearning.API.Mappers.ReadingPrograms;
using EarlyLearning.ReadingPrograms;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    [Route("api/v1/reading-program")]
    [ApiController]
    public class ReadingProgramController : ApiControllerBase
    {
        private readonly ReadingProgramManager _programManager;

        public ReadingProgramController(ILogger logger, ReadingProgramManager programManager, CurrentUser user) 
            : base(logger.ForContext<ReadingProgramController>(), user)
        {
            _programManager = programManager;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetReadingProgramsForUser()
        {
            var programs = await _programManager.GetAllProgramsForUser(UserId);
            var vms = programs.Select(x => x.ToReadingProgramVM());
            return Ok(vms);
        }
    }
}
