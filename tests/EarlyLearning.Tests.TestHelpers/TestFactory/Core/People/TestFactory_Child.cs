using System.Collections.Generic;
using EarlyLearning.Core.People;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public Child NewChild(string firstName = null, string lastName = null)
        {
            firstName ??= "Child " + random.Next(1, 100);
            lastName ??= "Lastname " + random.Next(1, 100);

            return new Child(firstName, lastName);
        }

        public IEnumerable<Child> NewChildList(int nbrOfChildren = 5)
        {
            var list = new List<Child>();
            for (var i = 0; i < nbrOfChildren; i++)
            {
                list.Add(NewChild());
            }

            return list;
        }
    }
}