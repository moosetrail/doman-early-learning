using System;

namespace EarlyLearning.Core.Program.ActivityStatuses
{
    public class Planned : ActivityStatus
    {
        public DateTime DateAddedToPlan { get; private set; }
    }
}