using System.Collections.Generic;
using System.Linq;
using EarlyLearning.Core.DTOForRavenDb;
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
                list.Add(NewChild("Child " + random.Next()));
            }

            return list;
        }

        public Child AddNewChild(string firstName = null, string lastName = null, params string[] adults)
        {
            var child = new ChildDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Adults = adults.Where(x => !string.IsNullOrWhiteSpace(x))
            };

            using var session = DocumentStore.OpenSession();
            session.Store(child);
            session.SaveChanges();

            return MapToChild(child);
        }

        private static Child MapToChild(ChildDTO child)
        {
            return new Child(child.Id, child.FirstName, child.LastName);
        }
    }
}