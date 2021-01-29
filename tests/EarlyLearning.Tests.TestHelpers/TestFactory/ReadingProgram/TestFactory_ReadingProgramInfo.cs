using System.Linq;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public ReadingProgramInfo NewReadingProgramInfo()
        {
            return new ReadingProgramInfo(forChildren: NewChildList(2).ToArray());
        }

        public ReadingProgramInfo AddNewReadingProgram(string userId = null)
        {


            return null;
        }
    }
}