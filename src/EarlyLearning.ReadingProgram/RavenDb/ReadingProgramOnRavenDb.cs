using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.RavenDb;
using EarlyLearning.Core.RavenDb.DataTransferObjects;
using EarlyLearning.Core.RavenDb.ObjectMappers;
using EarlyLearning.ReadingPrograms.DataModels.ReadingSingleUnits;
using EarlyLearning.ReadingPrograms.DataModels.ReadingUnits;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Serilog;

namespace EarlyLearning.ReadingPrograms.RavenDb
{
    public class ReadingProgramOnRavenDb : ReadingProgram<ReadingCategory<ReadingWord>>
    {
        private readonly IAsyncDocumentSession _session;
        private readonly ILogger _logger;

        public ReadingProgramOnRavenDb(IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger;
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
                var found = await PlannedUnitsInProgramSorted(programId)
                    .Skip(offset).Take(limit).ToListAsync();
                return found;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        private IRavenQueryable<ReadingCategoryDTO<ReadingWordDTO>> PlannedUnitsInProgram(string programId)
        {
            return CategoriesInProgram(programId)
                .Where(x => x.ActivityStatus.Type == ActivityStatusType.Planned);
        }


        private IRavenQueryable<ReadingCategoryDTO<ReadingWordDTO>> PlannedUnitsInProgramSorted(string programId)
        {
            return PlannedUnitsInProgram(programId)
                .OrderBy(x => x.ActivityStatus.SortingIndex);
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

            if (dto.ActivityStatus.Type == ActivityStatusType.Planned && dto.ActivityStatus.SortingIndex < 0)
                await SetSortIndexToLast(programId, dto);
            
            await _session.StoreAsync(dto);
            await _session.SaveChangesAsync();
        }

        public async Task ChangeStatus(string unitId, ActivityStatus newStatus)
        {
            var dto = await GetReadingCategoryDTO(unitId);

            if (dto != null)
            {
                UpdateActivityStatus(newStatus, dto);
                await SaveChanges();
            }
        }

        private async Task<ReadingCategoryDTO<ReadingWordDTO>> GetReadingCategoryDTO(string unitId)
        {
            try
            {
                var dto = await _session.Query<ReadingCategoryDTO<ReadingWordDTO>>()
                    .SingleOrDefaultAsync(x => x.Id == unitId);
                return dto;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        private void UpdateActivityStatus(ActivityStatus newStatus, ReadingCategoryDTO<ReadingWordDTO> dto)
        {
            var oldStatus = dto.ActivityStatus;
            dto.ActivityStatus = newStatus.ToDTO();
            _logger
                .ForContext("ReadingUnit", dto.Id)
                .ForContext("FromStatus", oldStatus)
                .ForContext("ToStatus", dto.ActivityStatus)
                .Information("Update activity status");
        }

        private async Task SaveChanges()
        {
            await _session.SaveChangesAsync();
            _logger.Information("Session saved");
        }

        public async Task MovePlanned(string unitId, string programId, int toSpot)
        {
            var dto = await GetReadingCategoryDTO(unitId);

            if (dto != null)
            {
                if (dto.ActivityStatus.Type == ActivityStatusType.Planned)
                {
                    await ChangeSortIndex(programId, toSpot, dto);
                }
                else
                {
                    throw new ApplicationException("Unit is not planned");
                }
            } 
        }

        private async Task ChangeSortIndex(string programId, int toSpot, ReadingCategoryDTO<ReadingWordDTO> dto)
        {
            if (toSpot < 0)
            {
                await SetSortIndexToLast(programId, dto);
            }

            if (toSpot == 0)
            {
                await SetSortIndexToFirst(programId, dto);
            }

            if (toSpot > 0)
            {
                await UpdateSortIndex(programId, toSpot, dto);
            }

            await SaveChanges();
        }

        private async Task SetSortIndexToFirst(string programId, ReadingCategoryDTO<ReadingWordDTO> dto)
        {
            var currentFrontTwo = await PlannedUnitsInProgramSorted(programId).Take(2).ToListAsync();
            var currentFront = currentFrontTwo.First();
            currentFront.ActivityStatus.SortingIndex = CalculateNewSortIndex(currentFrontTwo);
            dto.ActivityStatus.SortingIndex = 0;
        }

        private async Task UpdateSortIndex(string programId, int toSpot, ReadingCategoryDTO<ReadingWordDTO> dto)
        {
            var toSkip = toSpot;

            if (toSpot < dto.ActivityStatus.SortingIndex)
            {
                toSkip -= 1;
            }

            var plannedToBeBetween = await CategoriesInProgram(programId)
                .Where(x => x.ActivityStatus.Type == ActivityStatusType.Planned)
                .OrderBy(x => x.ActivityStatus.SortingIndex)
                .Skip(toSkip)
                .Take(2)
                .ToListAsync();

            dto.ActivityStatus.SortingIndex = CalculateNewSortIndex(plannedToBeBetween);
        }

        private async Task SetSortIndexToLast(string programId, ReadingUnitDTO dto)
        {
            var currentTail =
                await PlannedUnitsInProgram(programId)
                    .OrderByDescending(x => x.ActivityStatus.SortingIndex).FirstAsync();
            dto.ActivityStatus.SortingIndex = CalculateNewLastIndex(currentTail);
        }

        private decimal CalculateNewSortIndex(IEnumerable<ReadingCategoryDTO<ReadingWordDTO>> toBeBetween)
        {
            return CalculateNewSortIndex(
                toBeBetween.ElementAt(0).ActivityStatus, 
                toBeBetween.ElementAt(1).ActivityStatus);
        }

        private decimal CalculateNewSortIndex(ActivityStatusDTO statusBefore, ActivityStatusDTO statusAfter)
        {
            var diff = (statusAfter.SortingIndex - statusBefore.SortingIndex)/2;
            var newSortIndex = statusBefore.SortingIndex + diff;

            _logger.Debug("Calculated new sort index to " + newSortIndex);

            return newSortIndex;
        }

        private decimal CalculateNewLastIndex(ReadingUnitDTO currentTail)
        {
            var roundedTailIndex = Math.Round(currentTail.ActivityStatus.SortingIndex, 0);
            var newTailIndex = roundedTailIndex + 1;

            _logger.Debug("Calculated new tail sort index to " + newTailIndex);

            return newTailIndex;
        }
    }
}