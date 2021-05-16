using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EarlyLearning.Core.DTOForRavenDb;
using EarlyLearning.Core.DTOForRavenDb.Mappers;
using EarlyLearning.Core.People;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.Core.RavenDb
{
    public class ChildManagerOnRavenDb : ChildManager
    {
        private readonly IAsyncDocumentSession _session;
        private readonly ILogger _logger;

        public ChildManagerOnRavenDb(IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger.ForContext<ChildManagerOnRavenDb>();
        }

        public async Task<Child> AddChildForUser(string firstName, string lastname, string userId)
        {
            var dto = await CreateChild(firstName, lastname, userId);
            await AddChildToSession(dto);
            await SaveSession();

            return dto.ToChild();
        }

        private async Task<ChildDTO> CreateChild(string firstName, string lastname, string userId)
        {
            var dto = new ChildDTO
            {
                FirstName = firstName,
                LastName = lastname,
                Adults = new List<string> {userId}
            };

            await GenerateId(dto);
            return dto;
        }

        private async Task GenerateId(ChildDTO dto)
        {
            dto.Id = "Child/" + dto.LastName + ", " + dto.FirstName;

            var numberOfChildrenWithSameName = await _session.Query<ChildDTO>()
                .CountAsync(x => x.FirstName == dto.FirstName && x.LastName == dto.LastName);
            if (numberOfChildrenWithSameName > 0)
            {
                dto.Id += " - " + (numberOfChildrenWithSameName + 1);
                _logger.Debug("Found more children with same name, add number identifier {number} to id",
                    numberOfChildrenWithSameName + 1);
            }
        }

        private async Task AddChildToSession(ChildDTO dto)
        {
            await _session.StoreAsync(dto);
            _logger.ForContext("child", dto).Information("Added child to session");
        }

        private async Task SaveSession()
        {
            await _session.SaveChangesAsync();
            _logger.Information("Saved session");
        }

        public async Task<IEnumerable<Child>> GetChildrenForUser(string adultId)
        {
            var childrenDtos =
                await _session.Query<ChildDTO>().Where(x => x.Adults.Any(u => u == adultId)).ToListAsync();

            var foundChildren = childrenDtos.Select(x => x.ToChild());

            return foundChildren;
        }

        /// <remarks>Untested</remarks>
        public async Task<bool> ChildExists(string firstName, string lastName, string userId)
        {
            var childExists = await _session.Query<ChildDTO>().AnyAsync(SelectChild(firstName, lastName, userId));
            return childExists;
        }

        private static Expression<Func<ChildDTO, bool>> SelectChild(string firstName, string lastName, string userId)
        {
            return x =>
                x.FirstName == firstName && x.LastName == lastName && x.Adults.Any(u => u == userId);
        }

        /// <remarks>Untested</remarks>
        public async Task<Child> GetChild(string firstName, string lastName, string userId)
        {
            var dto = await _session.Query<ChildDTO>()
                .SingleOrDefaultAsync(SelectChild(firstName, lastName, userId));

            var child = dto?.ToChild();
            return child;
        }
    }
}