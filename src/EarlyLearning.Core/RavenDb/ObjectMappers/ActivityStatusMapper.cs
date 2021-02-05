using System;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.Program.ActivityStatuses;
using EarlyLearning.Core.RavenDb.DataTransferObjects;

namespace EarlyLearning.Core.RavenDb.ObjectMappers
{
    public static class ActivityStatusMapper
    {
        public static ActivityStatus ToStatus(this ActivityStatusDTO dto)
        {
            return dto.Type switch
            {
                ActivityStatusType.Planned => new Planned(dto.DateStamp, dto.SortingIndex),
                ActivityStatusType.Active => new CurrentlyActive(dto.DateStamp),
                ActivityStatusType.Retired => new Retired(dto.DateStamp),
                _ => throw new NotSupportedException("Unexpected type of ActivityStatus not yet supported")
            };
        }

        public static ActivityStatusDTO ToDTO(this ActivityStatus status)
        {
            return status switch
            {
                Planned planned => new ActivityStatusDTO
                {
                    DateStamp = planned.DateAddedToPlan,
                    SortingIndex = planned.SortingIndex,
                    Type = ActivityStatusType.Planned
                },
                CurrentlyActive active => new ActivityStatusDTO
                {
                    DateStamp = active.StartDate, SortingIndex = -1, Type = ActivityStatusType.Active
                },
                Retired retired => new ActivityStatusDTO
                {
                    DateStamp = retired.RetirementDate, SortingIndex = -1, Type = ActivityStatusType.Retired
                },
                _ => throw new NotSupportedException("Unexpected type of ActivityStatus not yet supported")
            };
        }
    }
}