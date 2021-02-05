using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.DataModels.ReadingUnits
{
    public class ReadingCategory<T>: ReadingUnit where T: ReadingCard
    {
        private ReadingCategory()
        { }

        public ReadingCategory(string title, IEnumerable<T> cards)
        :base(title)
        {
            ReadingCards = cards;
        }

        public IEnumerable<T> ReadingCards { get; private set; }
    }
}