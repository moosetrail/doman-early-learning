using System.Collections.Generic;
using System.Linq;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using Raven.Client.Documents.Indexes;

namespace EarlyLearning.ReadingPrograms.RavenDb.Indexes
{
    public class ReadingProgram_ByUser : AbstractIndexCreationTask<ReadingProgramInfoDTO>
    {
        public class Result
        {
            public IEnumerable<string> UserIds { get; set; }
        }

        public ReadingProgram_ByUser()
        {
            Map = programs => from program in programs
                select new Result
                {
                    UserIds = new List<string>()
                };
        }
    }
}