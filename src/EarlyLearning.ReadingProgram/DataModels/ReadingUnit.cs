using System.Collections.Generic;
using EarlyLearning.Core.Program;

namespace EarlyLearning.ReadingPrograms.DataModels
{
    public abstract class ReadingUnit
    {
        private ReadingUnit() {}

        protected ReadingUnit(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public ActivityStatus ActivityStatus { get; private set; }

        public IEnumerable<ActivitySession> CompletedSessions { get; private set; }
    }
}