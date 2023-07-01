using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
    public class ProfileModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Эл.почта")]
        public string? Email { get; set; }
        
        
        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }

        public string? StatusMessage { get; set; }
    }
}
