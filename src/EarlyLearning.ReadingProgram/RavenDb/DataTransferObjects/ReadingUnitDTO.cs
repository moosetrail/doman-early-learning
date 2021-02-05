using System.Collections.Generic;
using EarlyLearning.Core.Program;
using EarlyLearning.Core.RavenDb.DataTransferObjects;

namespace EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects
{
    public class ReadingUnitDTO
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ProgramId { get; set; }

        public ActivityStatusDTO ActivityStatus { get; set; }

        public IEnumerable<ActivitySession> Sessions { get; set; }
    }
}