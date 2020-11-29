using System.Collections.Generic;
using EarlyLearning.Core.Program;

namespace EarlyLearning.ReadingPrograms.DataModels
{
    public abstract class ReadingUnit
    {
        public string Title { get; private set; }

        public ActivityStatus ActivityStatus { get; private set; }

        public IEnumerable<ActivitySession> CompletedSessions { get; private set; }
    }
}