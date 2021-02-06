using System.Collections.Generic;
using EarlyLearning.Core.Program;

namespace EarlyLearning.ReadingPrograms.DataModels
{
    public abstract class ReadingUnit
    {
        protected ReadingUnit(string title, ActivityStatus status)
        {
            Title = title;
            ActivityStatus = status;
        }

        protected ReadingUnit(string id, string title, ActivityStatus status, IEnumerable<ActivitySession> sessions)
        {
            Id = id;
            Title = title;
            ActivityStatus = status;
            CompletedSessions = sessions;
        }

        public string Id { get; private set; }

        public string Title { get; private set; }

        public ActivityStatus ActivityStatus { get; private set; }

        public IEnumerable<ActivitySession> CompletedSessions { get; private set; }
    }
}