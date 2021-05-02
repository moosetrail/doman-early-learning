using EarlyLearning.Core.People;

namespace EarlyLearning.Core.DTOForRavenDb.Mappers
{
    internal static class ChildMapper
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

        public static Child ToChild(this ChildDTO dto)
        {
            return new Child(dto.Id, dto.FirstName, dto.LastName);
        }
    }
}