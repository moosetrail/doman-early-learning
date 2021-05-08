using System.Collections.Generic;
using System.Linq;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public ReadingCategory<ReadingWord> NewWordCategory(ActivityStatus status = null, string categoryName = null, string categoryId = null, params string[]onTheCards)
        {
            var cards = onTheCards == null ? GenerateRandomWordCards() : ReadingCards(onTheCards);

            status ??= new CurrentlyActive();

            categoryName ??= "Category " + random.Next();

            return new ReadingCategory<ReadingWord>(categoryName, cards, status);
        }

        private IEnumerable<ReadingWord> GenerateRandomWordCards()
        {
            var cards = new List<ReadingWord>();
            for (var i = 0; i < 5; i++)
            {
                var card = new ReadingWord("Word " + random.Next());
                cards.Add(card);
            }

            return cards;
        }

        private IEnumerable<ReadingWord> ReadingCards(string[] textOnCards)
        {
            return textOnCards.Select(word => new ReadingWord(word)).ToList();
        }

        public IEnumerable<ReadingCategory<ReadingWord>> NewWordCategories(int nbr = 5)
        {
            var categories = new List<ReadingCategory<ReadingWord>>();

            for (var i = 0; i < nbr; i++)
            {
                categories.Add(NewWordCategory());
            }

            return categories;
        }

        public IEnumerable<ReadingCategory<ReadingWord>> AddNewWordCategories(int nbr = 5, string programId = "Program Id",
            ActivityStatus status = null)
        {
            using var session = DocumentStore.OpenSession();
            status ??= new CurrentlyActive();
            var categories = new List<ReadingCategoryDTO<ReadingWordDTO>>();

            for (int i = 0; i < nbr; i++)
            {
                var category = NewWordCategory(status);
                var dto = category.ToDTO(programId);

                if (status is Planned)
                {
                    dto.ActivityStatus.SortingIndex = i;
                }

                session.Store(dto);
                categories.Add(dto);
            }

            session.SaveChanges();

            var toReturn = categories.Select(x => x.ToCategory());
            return toReturn;
        }
    }
}