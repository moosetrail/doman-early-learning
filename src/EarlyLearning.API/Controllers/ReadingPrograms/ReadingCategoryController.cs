using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    public abstract class ReadingCategoryController : ControllerBase
    {
        protected ReadingCategoryController() {}

        [HttpGet]
        [Route("{programId}/current")]
        public async Task<IActionResult> GetCurrentUnitsForProgram()
        {
            return null;
        }
    }
}
