﻿using Serilog;

namespace Moosetrail.EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public ILogger MockLogger()
        {
            return Log.Logger;
        }
    }
}