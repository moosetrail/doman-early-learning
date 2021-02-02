using System.Collections.Generic;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public ReadingCategory<ReadingWord> NewWordCategory()
        {
            var cards = GenerateRandomWordCards();

            return new ReadingCategory<ReadingWord>("Category " + random.Next(), cards);
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

        public IEnumerable<ReadingCategory<ReadingWord>> NewWordCategories(int nbr = 5)
        {
            var categories = new List<ReadingCategory<ReadingWord>>();

            for (var i = 0; i < nbr; i++)
            {
                categories.Add(NewWordCategory());
            }

            return categories;
        }
    }
}