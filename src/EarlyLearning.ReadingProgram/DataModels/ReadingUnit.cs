using System.Collections.Generic;
using EarlyLearning.Core.Program;

namespace EarlyLearning.ReadingPrograms.DataModels
{
    public abstract class ReadingUnit
    {
        protected ReadingUnit() {}

        protected ReadingUnit(string title, ActivityStatus status)
        {
            Title = title;
            ActivityStatus = status;
        }

        public string Id { get; private set; }

        public string Title { get; private set; }

        public ActivityStatus ActivityStatus { get; private set; }

        public IEnumerable<ActivitySession> CompletedSessions { get; private set; }
    }
}