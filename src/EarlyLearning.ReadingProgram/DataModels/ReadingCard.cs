namespace EarlyLearning.ReadingPrograms.DataModels
{
    public abstract class ReadingCard
    {
        protected ReadingCard() {}

        protected ReadingCard(string textOnCard)
        {
            TextOnTheCard = textOnCard;
        }

        public string TextOnTheCard { get; private set; }
    }
}