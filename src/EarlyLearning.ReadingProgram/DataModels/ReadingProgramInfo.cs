using System.Collections.Generic;

namespace EarlyLearning.ReadingPrograms.DataModels
{
    public class ReadingProgramInfo
    {
        private ReadingProgramInfo(){}

        public ReadingProgramInfo(string id = null, params string[] childIds)
        {
            Id = id;
            Children = childIds;
        }

        public string Id { get; private set; }

        public IEnumerable<string> Children { get; private set; }
    }
}