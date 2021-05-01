using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.API.Mappers.ReadingPrograms;
using EarlyLearning.People.DataModels;
using EarlyLearning.ReadingPrograms;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using CurrentUser = EarlyLearning.API.Dataclasses.User.CurrentUser;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    [Route("api/v1/reading-program")]
    [ApiController]
    public class ReadingProgramController : ApiControllerBase
    {
        private readonly ReadingProgramManager _programManager;
        private readonly CurrentUser _user;

        public ReadingProgramController(ILogger logger, ReadingProgramManager programManager, CurrentUser user) 
            : base(logger.ForContext<ReadingProgramController>())
        {
            _programManager = programManager;
            _user = user;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetReadingProgramsForUser()
        {
            var programs = await _programManager.GetAllProgramsForUser(_user.UserId);
            var vms = programs.Select(x => x.ToReadingProgramVM());
            return Ok(vms);
        }
    }
}
