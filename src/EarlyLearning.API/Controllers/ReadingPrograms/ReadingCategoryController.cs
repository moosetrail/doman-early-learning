using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    public abstract class ReadingCategoryController : ControllerBase
    {
        protected ReadingCategoryController() {}

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrent([FromQuery] string programId)
        {
            return null;
        }

        [HttpGet]
        [Route("planned")]
        public Task<IActionResult> GetPlanned([FromQuery] string programId, [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            return null;
        }

        [HttpGet]
        [Route("retired")]
        public Task<IActionResult> GetRetired([FromQuery] string programId, [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            return null;
        }

        [HttpPost]
        [Route("")]
        private Task<IActionResult> Add([FromBody] object unitToAdd, [FromQuery] string programId)
        {
            return null;
        }

        [HttpPatch]
        [Route("status")]
        public Task<IActionResult> ChangeStatus([FromQuery] string unitId, [FromQuery] int newStatus)
        {
            return null; 
        }

        [HttpPatch]
        [Route("move")]
        public Task<IActionResult> MovePlanned([FromQuery] string unitId, [FromQuery] string programId,
            [FromQuery] int toSpot)
        {
            return null;
        }
    }
}
