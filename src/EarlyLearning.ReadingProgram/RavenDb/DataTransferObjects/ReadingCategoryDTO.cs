using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects
{
    public class ReadingCategoryDTO<T>: ReadingUnitDTO where T: ReadingCardDTO
    {
        public IEnumerable<T> Cards { get; set; }
    }
}