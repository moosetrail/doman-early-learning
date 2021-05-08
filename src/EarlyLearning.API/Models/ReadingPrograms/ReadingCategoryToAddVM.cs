using System.Collections.Generic;

namespace EarlyLearning.API.Models.ReadingPrograms
{
    public class ReadingCategoryToAddVM
    {
        public string Title { get; set; }

        public IEnumerable<string> OnTheCards { get; set; }
    }
}