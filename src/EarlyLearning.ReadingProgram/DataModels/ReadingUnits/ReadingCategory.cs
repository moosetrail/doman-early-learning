using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.DataModels.ReadingUnits
{
    public abstract class ReadingCategory<T>: ReadingUnit where T: ReadingCard
    {
        public IEnumerable<T> ReadingCards { get; private set; }
    }
}