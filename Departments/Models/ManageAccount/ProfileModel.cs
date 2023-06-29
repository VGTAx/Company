using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
    public class ProfileModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Phone]
        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }
    }
}
