using System.Collections.Generic;

namespace EarlyLearning.ReadingProgram.DataModels.ReadingSingleUnits
{
    public class ReadingSentence : ReadingSingleUnit
    {
        public IEnumerable<DictionaryWord> DictionaryWords { get; private set; }
    }
}