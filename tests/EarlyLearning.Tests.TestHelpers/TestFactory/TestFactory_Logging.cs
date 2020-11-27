using Serilog;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public ILogger MockLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.NUnitOutput()
                .CreateLogger();
        }
    }
}