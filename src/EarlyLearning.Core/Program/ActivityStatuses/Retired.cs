using System;

namespace EarlyLearning.Core.Program.ActivityStatuses
{
    public class Retired : ActivityStatus
    {
        public Retired()
        {
            RetirementDate = DateTime.Now;
        }

        public Retired(DateTime date)
        {
            RetirementDate = date;
        }

        public DateTime RetirementDate { get; private set; }
    }
}