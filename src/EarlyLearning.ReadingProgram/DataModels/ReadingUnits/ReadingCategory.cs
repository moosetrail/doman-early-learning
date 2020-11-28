using System.Collections.Generic;

namespace EarlyLearning.ReadingProgram.DataModels.ReadingUnits
{
    public abstract class ReadingCategory<T>: ReadingUnit where T: ReadingSingleUnit
    {
        public IEnumerable<T> ReadingUnits { get; private set; }
    }
}