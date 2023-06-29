using System.ComponentModel.DataAnnotations;

namespace Company.Models.Account
{
    public class ForgorPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
