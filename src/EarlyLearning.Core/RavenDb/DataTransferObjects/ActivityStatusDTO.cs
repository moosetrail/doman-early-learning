﻿using System;

namespace EarlyLearning.Core.RavenDb.DataTransferObjects
{
    public class ActivityStatusDTO
    {
        public DateTime DateStamp { get; set; }

        public ActivityStatusType Type { get; set; }

        public decimal SortingIndex { get; set; }
    }
}