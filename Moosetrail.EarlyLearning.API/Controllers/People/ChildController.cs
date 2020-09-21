using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moosetrail.EarlyLearning.API.Models.People;
using Moosetrail.EarlyLearning.Dataclasses.People;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;

namespace Moosetrail.EarlyLearning.API.Controllers.People
{
    [Route("api/children")]
    [ApiController]
    public class ChildController : ApiControllerBase
    {
        public ChildController(IAsyncDocumentSession session, ILogger logger)
        : base(logger.ForContext<ChildController>(), session)
        {
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public async Task<ChildVM> AddChild(ChildVM child)
        {
            var nbrChildrenWithSameName = await _session.Query<Child>()
                .CountAsync(x => x.FirstName == child.FirstName && x.LastName == child.LastName);

            var childToAdd = new Child(child.FirstName, child.LastName);
            childToAdd.SetIdWithNumber(nbrChildrenWithSameName + 1);

            await _session.StoreAsync(childToAdd);
            _logger.ForContext("Child", child).Information("Stored to session");

            if (AppUser is AdultProgrammer adult)
            {
                adult.AddChild(childToAdd.Id);
                _logger.ForContext("ChildId", child.Id).ForContext("AdultProgrammer", AppUser.Email).Information("Added child to adult");
            }

            await _session.SaveChangesAsync();
            _logger.Information("Session saved");

            return ChildToVM(childToAdd);
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

        public async Task<IEnumerable<ChildVM>> GetChildren()
        {
            var user = await _session.Query<AppUser>().SingleOrDefaultAsync(x => x.Email == AppUserEmail);

            return null;
        }
    }
}
