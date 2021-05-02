using EarlyLearning.API.Dataclasses.User;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EarlyLearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly ILogger Logger;
        private readonly CurrentUser _currentUser;

        protected ApiControllerBase(ILogger logger, CurrentUser currentUser = null)
        {
            Logger = logger.ForContext<ApiControllerBase>()
                .ForContext("User", User != null ? User.Identity?.Name : "Not Authenticated");
            _currentUser = currentUser;
        }

        protected string UserId => _currentUser.UserId;
    }
}
