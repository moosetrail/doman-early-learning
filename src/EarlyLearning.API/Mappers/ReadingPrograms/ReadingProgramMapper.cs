using EarlyLearning.API.Models.ReadingPrograms;
using EarlyLearning.ReadingPrograms.DataModels;

namespace EarlyLearning.API.Mappers.ReadingPrograms
{
    public static class ReadingProgramMapper
    {
        public static ReadingProgramVM ToReadingProgramVM(this ReadingProgramInfo program)
        {
            return new ReadingProgramVM
            {
                ProgramId = program.Id,
                ChildrenIds = program.Children
            };
        }
    }
}