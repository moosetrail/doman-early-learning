using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.API.Dataclasses.User;
using EarlyLearning.API.Models.People;
using EarlyLearning.Core;
using EarlyLearning.Core.People;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EarlyLearning.API.Controllers.People
{
    [Route("api/v1/children")]
    [ApiController]
    public class ChildController : ApiControllerBase
    {
        private readonly ChildManager _manager;

        public ChildController(CurrentUser currentUser, ChildManager manager, ILogger logger)
        : base(logger.ForContext<ChildController>(), currentUser: currentUser)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddChild(AddChildVM child)
        {
            if (!ModelState.IsValid)
                return InvalidChildToAdd(child);

            var addedChild = await _manager.AddChildForUser(child.FirstName, child.LastName, UserId);
            var childVM = ChildToVM(addedChild);
            return Ok(childVM);
        }

        private IActionResult InvalidChildToAdd(AddChildVM child)
        {
            Logger.ForContext("Child", child).Information("Tried to add invalid child");
            return BadRequest("Invalid child");
        }

        private static ChildVM ChildToVM(Child child)
        {
            return new ChildVM
            {
                Id = child.Id,
                FirstName = child.FirstName,
                LastName = child.LastName
            };
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetChildren()
        {
            var foundChildren = await _manager.GetChildrenForUser(UserId);
            var childrenVMs = foundChildren.Select(ChildToVM);
            return Ok(childrenVMs);
        }
    }
}
