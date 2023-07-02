using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models.Account
{
    [NotMapped]
    public class ForgorPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
