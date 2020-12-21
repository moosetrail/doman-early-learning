using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits
{
    public class ReadingSentence : ReadingCard
    {
        public IEnumerable<DictionaryWord> DictionaryWords { get; private set; }
    }
}