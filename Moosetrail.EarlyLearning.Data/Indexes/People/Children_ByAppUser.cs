using System.Linq;
using Moosetrail.EarlyLearning.Dataclasses.People;
using Raven.Client.Documents.Indexes;

namespace Moosetrail.EarlyLearning.Data.Indexes.People
{
    public class Children_ByAppUser : AbstractMultiMapIndexCreationTask
    {
        public Children_ByAppUser()
        {
            AddMap<AdultProgrammer>(adults => from a in adults select new { a.Email, a.ChildIds });
        }
    }
}