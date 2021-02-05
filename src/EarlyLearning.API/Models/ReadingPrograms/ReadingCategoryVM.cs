using System.Collections.Generic;

namespace EarlyLearning.API.Models.ReadingPrograms
{
    public class ReadingCategoryVM
    {
        public string Title { get; set; }

        public IEnumerable<string> Cards { get; set; }
    }
}