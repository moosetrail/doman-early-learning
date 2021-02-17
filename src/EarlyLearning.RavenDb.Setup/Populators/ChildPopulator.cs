using System.Collections.Generic;
using System.Threading.Tasks;
using EarlyLearning.Core.DTOForRavenDb.Mappers;
using EarlyLearning.Core.People;
using Raven.Client.Documents.Session;

namespace EarlyLearning.RavenDb.Setup.Populators
{
    internal class ChildPopulator
    {
        private readonly IAsyncDocumentSession _session;

        private string[] children = {"Zacharias", "Jacqueline", "Dominiqué", "Phoenix"};
        private string[] otherChildren = {"Alan", "Nick", "Amanda"};

        public ChildPopulator(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<Child>> Run()
        {
            var childList = new List<Child>();

            await AddChildrenInList(childList, children, "Hultén", "abcd-abcd-abcd-abcd");
            await AddChildrenInList(childList, otherChildren, "Andersson", "xyz-xyz-xyz");

            await _session.SaveChangesAsync();
            return null;
        }

        private async Task AddChildrenInList(ICollection<Child> childList, IEnumerable<string> names, string lastName, string adultId)
        {
            foreach (var name in names)
            {
                var child = new Child(name, lastName);
                var dto = child.ToDTO();
                dto.Adults = new[] {adultId};
                await _session.StoreAsync(dto);
                childList.Add(child);
            }
        }
    }
}