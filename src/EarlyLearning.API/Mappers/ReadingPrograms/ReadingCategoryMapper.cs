using System.Linq;
using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;

namespace EarlyLearning.API.Mappers.ReadingPrograms
{
    public static class ReadingCategoryMapper
    {
        public static ReadingCategoryVM ToReadingCategoryVM(this ReadingCategory<ReadingWord> category)
        {
            var vm = new ReadingCategoryVM
            {
                Title = category.Title,
                Cards = category.ReadingCards.Select(x => x.TextOnTheCard)
            };

            return vm;
        }
    }
}