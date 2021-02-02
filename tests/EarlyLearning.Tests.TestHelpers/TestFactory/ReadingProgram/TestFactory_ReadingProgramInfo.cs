using System.Linq;
using EarlyLearning.ReadingPrograms.DataModels;
using EarlyLearning.ReadingPrograms.RavenDb.DataTransferObjects;
using EarlyLearning.ReadingPrograms.RavenDb.ObjectMappers;

namespace EarlyLearning.Tests.TestHelpers.TestFactory
{
    public partial class TestFactory
    {
        public ReadingProgramInfo NewReadingProgramInfo()
        {
            return new ReadingProgramInfo(childIds: NewChildList(2).Select(x => x.Id).ToArray());
        }

        public ReadingProgramInfo AddNewReadingProgram(string userId = null)
        {
            userId ??= CurrentUser.UserId;

            var children = new[]
            {
                AddNewChild(adults: userId),
                AddNewChild(adults: userId)
            };

            var dto = new ReadingProgramInfoDTO
            {
                ChildrenIds = children.Select(x => x.Id)
            };

            using var session = DocumentStore.OpenSession();
            session.Store(dto);
            session.SaveChanges();

            return dto.ToReadingProgramInfo();
        }
    }
}