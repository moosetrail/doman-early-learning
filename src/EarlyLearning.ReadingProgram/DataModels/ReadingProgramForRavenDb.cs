using System.Collections.Generic;
using EarlyLearning.Core.People;

namespace EarlyLearning.ReadingProgram.DataModels
{
    public class ReadingProgramForRavenDb : ReadingProgram
    {
        public string Id { get; private set; }

        public IEnumerable<Child> ChildrenOnProgram { get; private set; }
    }
}