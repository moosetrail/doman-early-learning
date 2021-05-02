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
    public class SingleWordReadingProgramOnRavenDb : ReadingProgram<ReadingCategory<ReadingWord>>
    {
        private readonly ILogger _logger;
        private readonly IAsyncDocumentSession _session;

        public SingleWordReadingProgramOnRavenDb(string programId, IAsyncDocumentSession session, ILogger logger)
        {
            _session = session;
            _logger = logger.ForContext<SingleWordReadingProgramOnRavenDb>().ForContext("ProgramId", ProgramId);
            ProgramId = programId;
        }

        public string ProgramId { get; private set; }

        public async Task<IEnumerable<ReadingCategory<ReadingWord>>> GetCurrent()
        {
            var found = await GetCurrentCategories();
            var currentCategories = found.Select(x => x.ToCategory());
            return currentCategories;
        }

        public async Task<IEnumerable<ReadingCategory<ReadingWord>>> GetPlanned(int limit, int offset)
        {
            var found = await GetPlannedCategoriesFromRaven(limit, offset);
            var plannedCategories = found.Select(x => x.ToCategory());
            return plannedCategories;
        }

        public async Task<IEnumerable<ReadingCategory<ReadingWord>>> GetRetired(int limit, int offset)
        {
            var found = await GetRetiredCategoriesFromRaven(limit, offset);
            var retiredCategories = found.Select(x => x.ToCategory());
            return retiredCategories;
        }

        public async Task Add(ReadingCategory<ReadingWord> toAdd)
        {
            var dto = toAdd.ToDTO(ProgramId);

            if (dto.ActivityStatus.Type == ActivityStatusType.Planned && dto.ActivityStatus.SortingIndex < 0)
                await SetSortIndexToLast(dto);

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

        public async Task MovePlanned(string unitId, int toSpot)
        {
            var dto = await GetReadingCategoryDTO(unitId);

            if (dto != null)
            {
                if (dto.ActivityStatus.Type == ActivityStatusType.Planned)
                    await ChangeSortIndex(toSpot, dto);
                else
                    throw new ApplicationException("Unit is not planned");
            }
        }

        private async Task<List<ReadingCategoryDTO<ReadingWordDTO>>> GetCurrentCategories()
        {
            try
            {
                var found = await CategoriesInProgram()
                    .Where(x => x.ActivityStatus.Type == ActivityStatusType.Active)
                    .ToListAsync();
                _logger.Debug("Got all active categories");
                return found;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        private IRavenQueryable<ReadingCategoryDTO<ReadingWordDTO>> CategoriesInProgram()
        {
            _logger.Debug("Get categories in program");
            return _session.Query<ReadingCategoryDTO<ReadingWordDTO>>()
                .Where(x => x.ProgramId == ProgramId);
        }

        private async Task<List<ReadingCategoryDTO<ReadingWordDTO>>> GetPlannedCategoriesFromRaven(int limit,
            int offset)
        {
            try
            {
                var found = await PlannedUnitsInProgramSorted()
                    .Skip(offset).Take(limit).ToListAsync();
                _logger.ForContext("Skip", offset).ForContext("Limit", limit).Debug("Got range of planned categories");
                return found;
            }
            catch (Exception e)
            {
                throw new RavenDbException(e);
            }
        }

        private IRavenQueryable<ReadingCategoryDTO<ReadingWordDTO>> PlannedUnitsInProgram()
        {
            return CategoriesInProgram()
                .Where(x => x.ActivityStatus.Type == ActivityStatusType.Planned);
        }


        private IRavenQueryable<ReadingCategoryDTO<ReadingWordDTO>> PlannedUnitsInProgramSorted()
        {
            return PlannedUnitsInProgram()
                .OrderBy(x => x.ActivityStatus.SortingIndex);
        }

        private async Task<List<ReadingCategoryDTO<ReadingWordDTO>>> GetRetiredCategoriesFromRaven(int limit,
            int offset)
        {
            try
            {
                var found = await CategoriesInProgram()
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

        private void UpdateActivityStatus(ActivityStatus newStatus, ReadingUnitDTO dto)
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

        private async Task ChangeSortIndex(int toSpot, ReadingCategoryDTO<ReadingWordDTO> dto)
        {
            if (toSpot < 0) await SetSortIndexToLast(dto);

            if (toSpot == 0) await SetSortIndexToFirst(dto);

            if (toSpot > 0) await UpdateSortIndex(toSpot, dto);

            await SaveChanges();
        }

        private async Task SetSortIndexToFirst(ReadingUnitDTO dto)
        {
            _logger.ForContext("UnitId", dto.Id).Debug("Move unit to first spot");
            var currentFrontTwo = await PlannedUnitsInProgramSorted().Take(2).ToListAsync();
            var currentFront = currentFrontTwo.First();
            currentFront.ActivityStatus.SortingIndex = CalculateNewSortIndex(currentFrontTwo);
            dto.ActivityStatus.SortingIndex = 0;
        }

        private async Task UpdateSortIndex(int toSpot, ReadingUnitDTO dto)
        {
            _logger.ForContext("UnitId", dto.Id).Debug("Move unit to spot {spot}", toSpot);

            var toSkip = toSpot;

            if (toSpot < dto.ActivityStatus.SortingIndex)
            {
                toSkip -= 1;
                _logger.Debug("Moving to before current position, skipping one less");
            }

            _logger.Debug("Skipping {toSkip} to find indexes", toSkip);
            var plannedToBeBetween = await CategoriesInProgram()
                .Where(x => x.ActivityStatus.Type == ActivityStatusType.Planned)
                .OrderBy(x => x.ActivityStatus.SortingIndex)
                .Skip(toSkip)
                .Take(2)
                .ToListAsync();

            dto.ActivityStatus.SortingIndex = CalculateNewSortIndex(plannedToBeBetween);
        }

        private async Task SetSortIndexToLast(ReadingUnitDTO dto)
        {
            _logger.ForContext("UnitId", dto.Id).Debug("Move unit to last spot");
            var currentTail =
                await PlannedUnitsInProgram()
                    .OrderByDescending(x => x.ActivityStatus.SortingIndex).FirstAsync();
            dto.ActivityStatus.SortingIndex = CalculateNewLastIndex(currentTail);
        }

        private decimal CalculateNewSortIndex(IEnumerable<ReadingCategoryDTO<ReadingWordDTO>> toBeBetween)
        {
            var front = toBeBetween.First();
            var behind = toBeBetween.Last();
            _logger.ForContext("ToBeBehind", front.Id).ForContext("ToBeInfrontOf", behind.Id).Debug("Calculate index to be between found");

            return CalculateNewSortIndex(
                front.ActivityStatus,
                behind.ActivityStatus);
        }

        private decimal CalculateNewSortIndex(ActivityStatusDTO statusBefore, ActivityStatusDTO statusAfter)
        {
            var diff = (statusAfter.SortingIndex - statusBefore.SortingIndex) / 2;
            var newSortIndex = statusBefore.SortingIndex + diff;

            _logger.Debug("Calculated new sort index to {newIndex}", newSortIndex);

            return newSortIndex;
        }

        private decimal CalculateNewLastIndex(ReadingUnitDTO currentTail)
        {
            var roundedTailIndex = Math.Round(currentTail.ActivityStatus.SortingIndex, 0);
            var newTailIndex = roundedTailIndex + 1;

            _logger.ForContext("OldTailIndex", currentTail.ActivityStatus.SortingIndex).Debug("Calculated new tail sort index to {newIndex}", newTailIndex);

            return newTailIndex;
        }
    }
}