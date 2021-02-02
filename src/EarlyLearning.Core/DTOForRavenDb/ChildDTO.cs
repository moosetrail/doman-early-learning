using System.Collections.Generic;

namespace EarlyLearning.Core.DTOForRavenDb
{
    public class ChildDTO
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> Adults { get; set; }
    }
}