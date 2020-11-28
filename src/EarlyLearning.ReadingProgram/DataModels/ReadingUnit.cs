using System.Collections.Generic;
using EarlyLearning.Core.Program;

namespace EarlyLearning.ReadingProgram.DataModels
{
    public abstract class ReadingUnit
    {
        public string Title { get; }

        public ActivityStatus ActivityStatus { get; private set; }

        public IEnumerable<ActivitySession> CompletedSessions { get; private set; }
    }
}