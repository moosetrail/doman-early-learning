using System;

namespace EarlyLearning.Core.Program.ActivityStatuses
{
    public class Planned : ActivityStatus
    {
        public Planned()
        {
            DateAddedToPlan = DateTime.Now;
            SortingIndex = -1;
        }

        public Planned(DateTime date, decimal sorting)
        {
            DateAddedToPlan = date;
            SortingIndex = sorting;
        }

        public DateTime DateAddedToPlan { get; private set; }

        public decimal SortingIndex { get; private set; }
    }
}