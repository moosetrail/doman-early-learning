﻿using System;

namespace EarlyLearning.Core.Program
{
    public class ActivitySession
    {
        public string ActivityId { get; private set; }

        public string ChildId { get; private set; }

        public DateTime DateStamp { get; private set; }
    }
}