using System.Collections.Generic;
using System.Linq;
using EarlyLearning.API.Dataclasses.User;
using EarlyLearning.API.Mappers.ReadingPrograms;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    [Route("api/v1/reading-words-categories")]
    [ApiController]
    public class ReadingWordsCategoryController : ReadingCategoryController<ReadingCategory<ReadingWord>>
    {
        public ReadingWordsCategoryController(ReadingProgramManager programManager, CurrentUser user, ILogger logger) 
            : base(programManager, logger.ForContext<ReadingWordsCategoryController>(), user)
        {
        }

        protected override object MapToVm(IEnumerable<ReadingCategory<ReadingWord>> elements)
        {
            var vms = elements.Select(MapToVm);
            return vms;
        }

        protected override ReadingCategory<ReadingWord> FromVmToUnitToAdd(ReadingCategoryToAddVM toAdd)
        {
            var category = new ReadingCategory<ReadingWord>(toAdd.Title,
                toAdd.OnTheCards.Select(x => new ReadingWord(x)), new Planned());
            return category;
        }

        protected override object MapToVm(ReadingCategory<ReadingWord> element)
        {
            return element.ToReadingCategoryVM();
        }
    }
}
