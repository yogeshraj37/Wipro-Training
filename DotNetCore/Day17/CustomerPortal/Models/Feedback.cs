using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models
{
    public class Feedback
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comments { get; set; }
    }
}