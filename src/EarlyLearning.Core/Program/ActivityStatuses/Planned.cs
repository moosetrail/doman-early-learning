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

        public Planned(DateTime date, int sorting)
        {
            DateAddedToPlan = date;
            SortingIndex = sorting;
        }

        public DateTime DateAddedToPlan { get; private set; }

        public int SortingIndex { get; private set; }
    }
}