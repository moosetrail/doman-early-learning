namespace EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits
{
    public class ReadingWord : ReadingCard
    {
        private ReadingWord(){ }

        public ReadingWord(string word)
        :base(word)
        {

        }

        public DictionaryWord DictionaryWord { get; private set; }
    }
}