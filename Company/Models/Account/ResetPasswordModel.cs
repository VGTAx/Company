﻿using System.ComponentModel.DataAnnotations;

namespace Company_.Models.Account
{
  public class ResetPasswordModel
  {
    [Required]
    public string? Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [Display(Name = "Новый пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }


    [DataType(DataType.Password)]
    [Display(Name = "Подтверждение пароля")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [Required]
    public string? Code { get; set; }
  }
}
