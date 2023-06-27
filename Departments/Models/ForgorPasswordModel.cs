using System.ComponentModel.DataAnnotations;

namespace Company.Models
{
    public class ForgorPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
