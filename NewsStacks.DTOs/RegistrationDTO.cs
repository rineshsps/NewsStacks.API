using System.ComponentModel.DataAnnotations;

namespace NewsStacks.DTOs
{
    public class RegistrationDTO
    {
        [Required]
        [MinLength(3), MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(3), MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MinLength(3), MaxLength(25)]
        public string Password { get; set; }
        public bool Dndactive { get; set; }
    }
}
