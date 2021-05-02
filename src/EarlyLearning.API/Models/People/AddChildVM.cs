using System.ComponentModel.DataAnnotations;

namespace EarlyLearning.API.Models.People
{
    public class AddChildVM
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
    }
}