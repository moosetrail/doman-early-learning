using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.API.Dataclasses.User;
using EarlyLearning.API.Models.People;
using EarlyLearning.Core.DTOForRavenDb;
using EarlyLearning.Core.People;
using EarlyLearning.People.DataModels;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.API.Controllers.People
{
    [Route("api/v1/children")]
    [ApiController]
    public class ChildController : ApiControllerBase
    {
        public ChildController(CurrentUser currentUser, IAsyncDocumentSession session, ILogger logger)
        : base(logger.ForContext<ChildController>(), session, currentUser)
        {
        }

        [HttpPost]
        [Route("")]
        public async Task<ChildVM> AddChild(ChildVM child)
        {
            var nbrChildrenWithSameName = await Session.Query<Child>()
                .CountAsync(x => x.FirstName == child.FirstName && x.LastName == child.LastName);

            var childToAdd = new Child(child.FirstName, child.LastName);
            childToAdd.SetIdWithNumber(nbrChildrenWithSameName + 1);

            await Session.StoreAsync(childToAdd);
            Logger.ForContext("Child", child).Information("Stored to session");

            if (CurrentUser is AdultProgrammer adult)
            {
                adult.AddChild(childToAdd.Id);
                Logger.ForContext("ChildId", child.Id).ForContext("AdultProgrammer", CurrentUser.Email).Information("Added child to adult");
            }

            await Session.SaveChangesAsync();
            Logger.Information("Session saved");

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

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<ChildVM>> GetChildren()
        {
            var children = await Session.Query<ChildDTO>().Where(x => x.Adults.Any(a => a == AppUserEmail))
                .ToListAsync();
            return null;
        }
    }
}
