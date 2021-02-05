using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.Core.RavenDb;
using EarlyLearning.Core.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace EarlyLearning.ReadingPrograms.RavenDb
{
    public class ReadingProgramOnRavenDb : ReadingProgram<ReadingCategory<ReadingWord>>
    {
        private readonly IAsyncDocumentSession _session;

        public ReadingProgramOnRavenDb(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<IEnumerable<ReadingCategory<ReadingWord>>> GetCurrent(string programId)
        {
            var found = await GetCurrentCategories(programId);
            var currentCategories = found.Select(x => x.ToCategory());
            return currentCategories;
        }

        private async Task<List<ReadingCategoryDTO<ReadingWordDTO>>> GetCurrentCategories(string programId)
        {
            try
            {
                var found = await CategoriesInProgram(programId)
                    .Where(x => x.ActivityStatus.Type == ActivityStatusType.Active)
                    .ToListAsync();
                return found;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        private IRavenQueryable<ReadingCategoryDTO<ReadingWordDTO>> CategoriesInProgram(string programId)
        {
            return _session.Query<ReadingCategoryDTO<ReadingWordDTO>>()
                .Where(x => x.ProgramId == programId);
        }

        public async Task<IEnumerable<ReadingCategory<ReadingWord>>> GetPlanned(string programId, int limit, int offset)
        {
            var found = await GetPlannedCategoriesFromRaven(programId, limit, offset);
            var plannedCategories = found.Select(x => x.ToCategory());
            return plannedCategories;
        }

        private async Task<List<ReadingCategoryDTO<ReadingWordDTO>>> GetPlannedCategoriesFromRaven(string programId, int limit, int offset)
        {
            try
            {
                var found = await CategoriesInProgram(programId)
                    .Where(x => x.ActivityStatus.Type == ActivityStatusType.Planned)
                    .OrderBy(x => x.ActivityStatus.SortingIndex)
                    .Skip(offset).Take(limit).ToListAsync();
                return found;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        public async Task<IEnumerable<ReadingCategory<ReadingWord>>> GetRetired(string programId, int limit, int offset)
        {
            var found = await GetRetiredCategoriesFromRaven(programId, limit, offset);
            var retiredCategories = found.Select(x => x.ToCategory());
            return retiredCategories;
        }

        private async Task<List<ReadingCategoryDTO<ReadingWordDTO>>> GetRetiredCategoriesFromRaven(string programId, int limit, int offset)
        {
            try
            {
                var found = await CategoriesInProgram(programId)
                    .Where(x => x.ActivityStatus.Type == ActivityStatusType.Retired)
                    .OrderByDescending(x => x.ActivityStatus.DateStamp)
                    .Skip(offset).Take(limit).ToListAsync();
                return found;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        public async Task Add(ReadingCategory<ReadingWord> toAdd, string programId)
        {
            var dto = toAdd.ToDTO(programId);
            await _session.StoreAsync(dto);
            await _session.SaveChangesAsync();
        }

        public async Task ChangeStatus(string unitId, int newStatus)
        {
            var dto = await _session.Query<ReadingCategoryDTO<ReadingWordDTO>>()
                .SingleOrDefaultAsync(x => x.Id == unitId);

            if (dto != null)
            {
                
            }

            throw new NotImplementedException();
        }

        public Task MovePlanned(string unitId, string programId, int toSpot)
        {
            throw new System.NotImplementedException();
        }
    }
}