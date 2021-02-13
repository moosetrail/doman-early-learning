using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    public abstract class ReadingCategoryController<T> : ControllerBase where T : ReadingUnit
    {
        private readonly ReadingProgram<T> _program;
        protected ReadingCategoryController(ReadingProgram<T> program)
        {
            _program = program;
        }

        [HttpGet]
        [Route("current")]
        public async Task<IActionResult> GetCurrent([FromQuery] string programId)
        {
            var current = await _program.GetCurrent(programId);

            var vmList = MapToVm(current);

            return Ok(vmList);
        }

        protected abstract object MapToVm(IEnumerable<T> elements);

        [HttpGet]
        [Route("planned")]
        public async Task<IActionResult> GetPlanned([FromQuery] string programId, [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var current = await _program.GetPlanned(programId, limit, offset);

            var vmList = MapToVm(current);

            return Ok(vmList);
        }

        [HttpGet]
        [Route("retired")]
        public async Task<IActionResult> GetRetired([FromQuery] string programId, [FromQuery] int limit = 10,
            [FromQuery] int offset = 0)
        {
            var current = await _program.GetRetired(programId, limit, offset);

            var vmList = MapToVm(current);

            return Ok(vmList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] ReadingCategoryToAddVM unitToAdd, [FromQuery] string programId)
        {
            var toAdd = FromVmToUnitToAdd(unitToAdd);
            await _program.Add(toAdd, programId);
            return Ok();
        }

        protected abstract T FromVmToUnitToAdd(ReadingCategoryToAddVM toAdd);

        [HttpPatch]
        [Route("status")]
        public async Task<IActionResult> ChangeStatus([FromQuery] string unitId, [FromQuery] ReadingUnitStatusVM newStatus)
        {
            var statusToChangeTo = ToStatus(newStatus);
            await _program.ChangeStatus(unitId, statusToChangeTo);
            return Ok(); 
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
        public async Task<IActionResult> MovePlanned([FromQuery] string unitId, [FromQuery] string programId,
            [FromQuery] int toSpot)
        {
            await _program.MovePlanned(unitId, programId, toSpot);
            return Ok();
        }
    }
}
