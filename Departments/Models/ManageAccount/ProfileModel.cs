using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models.ManageAccount
{
    [NotMapped]
    public class ProfileModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Эл.почта")]
        public string? Email { get; set; }
        
        
        [Display(Name = "Номер телефона")]
        public string? Phone { get; set; }

        [Display(Name = "Имя")]
        public string? Name { get; set; }

        public string? StatusMessage { get; set; }
    }
}
