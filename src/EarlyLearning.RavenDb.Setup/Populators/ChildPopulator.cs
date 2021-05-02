using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core;
using EarlyLearning.Core.People;

namespace EarlyLearning.RavenDb.Setup.Populators
{
    internal class ChildPopulator
    {
        private readonly ChildManager _manager;

        private string[] children = {"Zacharias", "Jacqueline", "Dominiqué", "Phoenix"};
        private string[] otherChildren = {"Alan", "Nick", "Amanda"};

        public ChildPopulator(ChildManager manager)
        {
            _manager = manager;
        }

        public async Task<IEnumerable<Child>> Run()
        {
            var childList = new List<Child>();

            await AddChildrenInList(childList, children, "Hultén", "abcd-abcd-abcd-abcd");
            await AddChildrenInList(childList, otherChildren, "Andersson", "xyz-xyz-xyz");

            return null;
        }

        private async Task AddChildrenInList(ICollection<Child> childList, IEnumerable<string> names, string lastName, string adultId)
        {
            foreach (var name in names)
            {
                await _manager.AddChildForUser(name, lastName, adultId);
            }
        }
    }
}