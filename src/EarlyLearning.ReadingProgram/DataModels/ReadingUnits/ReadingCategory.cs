namespace EarlyLearning.ReadingProgram.DataModels.ReadingUnits
{
    public abstract class ReadingCategory<T>: ReadingUnit where T: ReadingSingleUnit
    {
        public string Title { get; }
    }
}