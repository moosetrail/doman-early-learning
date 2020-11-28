using System;

namespace EarlyLearning.Core.Program.ActivityStatuses
{
    public class Retired : ActivityStatus
    {
        public DateTime RetirementDate { get; private set; }
    }
}