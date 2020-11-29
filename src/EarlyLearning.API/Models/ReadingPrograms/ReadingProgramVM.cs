using System.Collections.Generic;

namespace EarlyLearning.API.Models.ReadingPrograms
{
    public class ReadingProgramVM
    {
        public string ProgramId { get; set; }

        public IEnumerable<string> ChildrenIds { get; set; }
    }
}