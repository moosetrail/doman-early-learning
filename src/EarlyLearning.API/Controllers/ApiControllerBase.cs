using System.Threading.Tasks;
using EarlyLearning.Core.People;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly IAsyncDocumentSession Session;
        private readonly AppUser _currentUser;

        protected ApiControllerBase(ILogger logger, IAsyncDocumentSession session = null, AppUser currentUser = null)
        {
            Logger = logger.ForContext<ApiControllerBase>()
                .ForContext("User", User != null ? User.Identity?.Name : "Not Authenticated");
            Session = session;
            _currentUser = currentUser;
        }

        protected AppUser CurrentUser
        {
            get
            {
                var searchForUser = Session.Query<AppUser>().SingleOrDefaultAsync(u => u.Email == User.Identity.Name);
                Task.WaitAll(searchForUser);

                return searchForUser.Result;
            }
        }

        protected string AppUserEmail => _currentUser.Email;
    }
}
