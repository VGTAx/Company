using System.ComponentModel.DataAnnotations;

namespace Company_.Models.Account
{
  public class ForgorPasswordModel
  {
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
  }
}
