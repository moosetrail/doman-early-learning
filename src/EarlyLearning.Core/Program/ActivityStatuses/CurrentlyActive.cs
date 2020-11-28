using System;

namespace EarlyLearning.Core.Program.ActivityStatuses
{
    public class CurrentlyActive : ActivityStatus
    {
        public DateTime StartDate { get; private set; }
    }
}