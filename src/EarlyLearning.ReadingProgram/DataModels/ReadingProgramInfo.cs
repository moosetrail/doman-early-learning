using System.Collections.Generic;
using EarlyLearning.Core.People;

namespace EarlyLearning.ReadingPrograms.DataModels
{
    public class ReadingProgramInfo
    {
        private ReadingProgramInfo(){}

        public ReadingProgramInfo(string id = null, params Child[] forChildren)
        {
            Children = forChildren;
            Id = id;
        }

        public string Id { get; private set; }

        public IEnumerable<Child> Children { get; private set; }
    }
}