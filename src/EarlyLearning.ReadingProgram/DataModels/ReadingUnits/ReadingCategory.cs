using System.Collections.Generic;
using EarlyLearning.Core.Program;

namespace EarlyLearning.ReadingPrograms.DataModels.ReadingUnits
{
    public class ReadingCategory<T>: ReadingUnit where T: ReadingCard
    {
        private ReadingCategory()
        { }

        public ReadingCategory(string title, IEnumerable<T> cards, ActivityStatus status)
        :base(title, status)
        {
            ReadingCards = cards;
        }

        public IEnumerable<T> ReadingCards { get; private set; }
    }
}