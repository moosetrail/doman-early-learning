using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.API.Dataclasses.User;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    public abstract class ReadingCategoryController<T> : ApiControllerBase where T : ReadingUnit
    {
        private readonly ReadingProgramManager _programManager;
        private readonly ILogger _logger;
        private readonly ReadingProgram<T> _program;

        protected ReadingCategoryController(ReadingProgramManager programManager, ILogger logger, CurrentUser user)
        :base(logger.ForContext<ReadingCategoryController<T>>(), user)
        {
            _programManager = programManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("current")]
        public Task<IActionResult> GetCurrent([FromQuery] string programId)
        {
            return GetFromProgram(programId, program => program.GetCurrent());
        }

        private async Task<IActionResult> WorkOnProgram(string programId,
            Func<ReadingProgram<T>, Task<IActionResult>> workOnProgram)
        {
            if (!await _programManager.UserCanAccessProgram(programId, UserId))
            {
                _logger.Warning("User tried to access program {programId} but doesn't have access to", programId);
                return Unauthorized();
            }

            var program = await _programManager.GetReadingProgram<T>(programId);

            if (program == null)
            {
                _logger.Information("Tried to access program {programId}, but not found", programId);
                return NotFound();
            }

            return await workOnProgram(program);
        }

        private Task<IActionResult> GetFromProgram(string programId, Func<ReadingProgram<T>, Task<IEnumerable<T>>> getFromProgram)
        {
            return WorkOnProgram(programId, async program =>
            {
                var current = await getFromProgram(program);

                var vmList = MapToVm(current);

                return Ok(vmList);
            });
        }

        protected abstract object MapToVm(IEnumerable<T> elements);

        [HttpGet]
        [Route("planned")]
        public Task<IActionResult> GetPlanned([FromQuery] string programId, [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            return GetFromProgram(programId, program => program.GetPlanned(limit, offset));
        }

        [HttpGet]
        [Route("retired")]
        public Task<IActionResult> GetRetired([FromQuery] string programId, [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            return GetFromProgram(programId, program => program.GetRetired(limit, offset));
        }

        [HttpPost]
        [Route("")]
        public Task<IActionResult> Add([FromBody] ReadingCategoryToAddVM unitToAdd, [FromQuery] string programId)
        {
            var toAdd = FromVmToUnitToAdd(unitToAdd);

            return WorkOnProgram(programId, async (program) =>
            {
                var added = await program.Add(toAdd);
                var resultVm = MapToVm(added);
                return Ok(resultVm);
            });
        }

        protected abstract T FromVmToUnitToAdd(ReadingCategoryToAddVM toAdd);

        protected abstract object MapToVm(T element);

        [HttpPatch]
        [Route("status")]
        public Task<IActionResult> ChangeStatus([FromQuery] string programId, [FromQuery] string unitId, [FromQuery] ReadingUnitStatusVM newStatus)
        {
            return WorkOnProgram(programId, async program =>
            {
                var statusToChangeTo = ToStatus(newStatus);
                await program.ChangeStatus(unitId, statusToChangeTo);
                return Ok();
            });
        }

        private static ActivityStatus ToStatus(ReadingUnitStatusVM vm)
        {
            return vm switch
            {
                ReadingUnitStatusVM.Active => new CurrentlyActive(),
                ReadingUnitStatusVM.Planned => new Planned(),
                ReadingUnitStatusVM.Retired => new Retired(),
                _ => throw new NotSupportedException("The supplied status wasn't recognized")
            };
        }

        [HttpPatch]
        [Route("move")]
        public  Task<IActionResult> MovePlanned([FromQuery] string unitId, [FromQuery] string programId,
            [FromQuery] int toSpot)
        {
            return WorkOnProgram(programId, async program =>
            {
                await program.MovePlanned(unitId, toSpot);
                return Ok();
            });
        }
    }
}
