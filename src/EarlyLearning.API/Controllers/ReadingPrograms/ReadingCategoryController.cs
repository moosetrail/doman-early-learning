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
    }
}
