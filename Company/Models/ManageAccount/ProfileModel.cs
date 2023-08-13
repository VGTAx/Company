using System.ComponentModel.DataAnnotations;

namespace Company_.Models.ManageAccount
{
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
  }
}
