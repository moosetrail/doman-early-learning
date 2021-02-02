using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects
{
    public class ReadingProgramInfoDTO
    {
        public string Id { get; set; }

        public IEnumerable<string> ChildrenIds { get; set; }
    }
}