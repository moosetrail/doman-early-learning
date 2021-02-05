using System.Collections.Generic;
using System.Linq;
using EarlyLearning.API.Mappers.ReadingPrograms;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using Microsoft.AspNetCore.Mvc;

namespace EarlyLearning.API.Controllers.ReadingPrograms
{
    [Route("api/v1/reading-words-categories")]
    [ApiController]
    public class ReadingWordsCategoryController : ReadingCategoryController<ReadingCategory<ReadingWord>>
    {
        public ReadingWordsCategoryController(ReadingProgram<ReadingCategory<ReadingWord>> program) 
            : base(program)
        {
        }

        protected override object MapToVm(IEnumerable<ReadingCategory<ReadingWord>> elements)
        {
            var vms = elements.Select(x => x.ToReadingCategoryVM());
            return vms;
        }

        protected override ReadingCategory<ReadingWord> FromVmToUnitToAdd(ReadingCategoryToAddVM toAdd)
        {
            throw new System.NotImplementedException();
        }
    }
}
