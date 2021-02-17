using EarlyLearning.Core.People;

namespace EarlyLearning.Core.DTOForRavenDb.Mappers
{
    public static class ChildMapper
    {
        public static ChildDTO ToDTO(this Child child)
        {
            var dto = new ChildDTO
            {
                Id = child.Id,
                FirstName = child.FirstName,
                LastName = child.LastName,
            };

            return dto;
        }
    }
}