using System;

namespace EarlyLearning.Core.Program.ActivityStatuses
{
    public class CurrentlyActive : ActivityStatus
    {
        public CurrentlyActive()
        {
            StartDate = DateTime.Now;
        }

        public CurrentlyActive(DateTime date)
        {
            StartDate = date;
        }

        public DateTime StartDate { get; private set; }
    }
}