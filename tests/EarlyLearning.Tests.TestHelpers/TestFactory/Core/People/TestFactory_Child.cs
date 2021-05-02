using System.Collections.Generic;
using EarlyLearning.Core;
using EarlyLearning.Core.People;
using EarlyLearning.Core.RavenDb;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        private ChildManager ChildManager => new ChildManagerOnRavenDb(ActiveSession, TestLogger());

        public Child NewChild(string firstName = null, string lastName = null)
        {
            firstName ??= "Child " + random.Next(1, 100);
            lastName ??= "Lastname " + random.Next(1, 100);

            return new Child($"Child/{lastName}, {firstName}, ", firstName, lastName);
        }

        public IEnumerable<Child> NewChildList(int nbrOfChildren = 5)
        {
            var list = new List<Child>();
            
            for (var i = 0; i < nbrOfChildren; i++)
            {
                list.Add(NewChild("Child " + random.Next()));
            }

            return list;
        }

        public Child AddNewChild(string firstName = null, string lastName = null, params string[] adults)
        {
            var childToAdd = NewChild(firstName, lastName);

            var task = ChildManager.AddChildForUser(childToAdd.FirstName, childToAdd.LastName, adults[0]);
            var child = task.Result;

            return child;
        }
    }
}