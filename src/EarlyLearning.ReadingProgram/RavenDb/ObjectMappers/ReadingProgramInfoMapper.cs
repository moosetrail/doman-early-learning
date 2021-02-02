using System.Linq;
using EarlyLearning.ReadingPrograms.DataModels;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;

namespace EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers
{
    public static class ReadingProgramInfoMapper
    {
        public static ReadingProgramInfo ToReadingProgramInfo(this ReadingProgramInfoDTO dto)
        {
            return new ReadingProgramInfo(dto.Id, dto.ChildrenIds.ToArray());
        }
    }
}