using System.ComponentModel.DataAnnotations;

namespace Company.Models.ManageAccount
{
  public class DeletePersonalDataModel
  {
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string? Password { get; set; }

    public bool RequirePassword { get; set; }
  }
}
