using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moosetrail.EarlyLearning.Dataclasses.People;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;

namespace Moosetrail.EarlyLearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IAsyncDocumentSession _session;

        protected ApiControllerBase(ILogger logger, IAsyncDocumentSession session = null)
        {
            _logger = logger.ForContext<ApiControllerBase>().ForContext("User", User != null ? User.Identity.Name : "Not Authenticated");
            _session = session;
        }

        protected AppUser AppUser
        {
            get
            {
                var searchForUser = _session.Query<AppUser>().SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                Task.WaitAll(searchForUser);

                return searchForUser.Result;
            }
        }

        protected string AppUserEmail => User?.Identity.Name;
    }
}
