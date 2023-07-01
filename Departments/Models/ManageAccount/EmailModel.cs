using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
    public class EmailModel
    {
        [EmailAddress]
        [Display(Name = "Эл.почта")]
        public string? Email { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Новая эл.почта")]
        public string? NewEmail { get; set; }

        public bool IsEmailConfirmed { get; set; }
    }
}
