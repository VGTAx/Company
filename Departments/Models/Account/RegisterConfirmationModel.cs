using System.ComponentModel.DataAnnotations.Schema;

namespace Company.Models.Account
{
    [NotMapped]
    public class RegisterConfirmationModel
    {
        public string StatusMessage { get; set; }
    }
}
