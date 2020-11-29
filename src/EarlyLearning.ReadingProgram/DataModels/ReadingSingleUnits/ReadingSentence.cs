using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits
{
    public class ReadingSentence : ReadingSingleUnit
    {
        public IEnumerable<DictionaryWord> DictionaryWords { get; private set; }
    }
}