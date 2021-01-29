using System.Collections.Generic;
using System.Linq;
using EarlyLearning.Core.People;

namespace EarlyLearning.ReadingPrograms.DTOForRavenDb
{
    public class ReadingProgramInfoDTO
    {
        public ReadingProgramInfoDTO(IEnumerable<Child> forChildren)
        {
            ChildrenIds = forChildren.Select(x => x.Id);
        }

        public string Id { get; private set; }

        public IEnumerable<string> ChildrenIds { get; private set; }
    }
}