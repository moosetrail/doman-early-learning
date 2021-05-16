using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core;
using EarlyLearning.Core.People;

namespace EarlyLearning.RavenDb.Setup.Populators
{
    internal class ChildPopulator
    {
        private readonly ChildManager _manager;
        private readonly string _fakeUserId;

        private string[] children = {"Zacharias", "Jacqueline", "Dominiqué", "Phoenix"};
        private string[] otherChildren = {"Alan", "Nick", "Amanda"};
        private List<Child> _children;

        public ChildPopulator(ChildManager manager, string fakeUserId)
        {
            _manager = manager;
            _fakeUserId = fakeUserId;
            _children = new List<Child>();
        }

        public async Task Run()
        {
            await AddChildrenInList(children, "Hultén", _fakeUserId);
            await AddChildrenInList(otherChildren, "Andersson", "xyz-xyz-xyz");
        }

        private async Task AddChildrenInList(IEnumerable<string> names, string lastName, string adultId)
        {
            foreach (var name in names)
            {
                if(! await _manager.ChildExists(name, lastName, adultId))
                    _children.Add(await _manager.AddChildForUser(name, lastName, adultId));
                else 
                    _children.Add(await _manager.GetChild(name, lastName, adultId));
            }
        }

        internal Child Zacharias => _children.Find(x => x.FirstName == "Zacharias");

        internal Child Jacqueline => _children.Find(x => x.FirstName == "Jacqueline");

        internal Child Dominique => _children.Find(x => x.FirstName == "Dominiqué");

        internal Child Phoenix => _children.Find(x => x.FirstName == "Phoenix");
    }
}