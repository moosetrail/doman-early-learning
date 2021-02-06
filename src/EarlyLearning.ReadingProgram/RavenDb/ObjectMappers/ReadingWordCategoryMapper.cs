using System.Linq;
using EarlyLearning.Core.RavenDb.ObjectMappers;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;

namespace EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers
{
    public static class ReadingWordCategoryMapper
    {
        public static ReadingCategoryDTO<ReadingWordDTO> ToDTO(this ReadingCategory<ReadingWord> category, string programId)
        {
            var dto = new ReadingCategoryDTO<ReadingWordDTO>
            {
                ProgramId = programId,
                Title = category.Title,
                Cards = category.ReadingCards.Select(x => x.ToDTO()),
                ActivityStatus = category.ActivityStatus.ToDTO(),
                Sessions = category.CompletedSessions
            };

            return dto;
        }

        public static ReadingWordDTO ToDTO(this ReadingWord word)
        {
            var dto = new ReadingWordDTO
            {
                Text = word.TextOnTheCard
            };

            return dto;
        }

        public static ReadingCategory<ReadingWord> ToCategory(this ReadingCategoryDTO<ReadingWordDTO> dto)
        {
            var category = new ReadingCategory<ReadingWord>(dto.Id, dto.Title, 
                dto.Cards.Select(x => x.ToReadingWord()), 
                dto.ActivityStatus.ToStatus(),
                dto.Sessions);
            return category;
        }

        public static ReadingWord ToReadingWord(this ReadingWordDTO dto)
        {
            var word = new ReadingWord(dto.Text);
            return word;
        }
    }
}