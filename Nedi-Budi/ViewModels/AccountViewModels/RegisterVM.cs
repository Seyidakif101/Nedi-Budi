using System.ComponentModel.DataAnnotations;

namespace Nedi_Budi.ViewModels.AccountViewModels
{
    public class RegisterVM
    {
        [Required, MaxLength(256), MinLength(3)]
        public string UserName { get; set; } = string.Empty;
       
        [Required, MaxLength(256), MinLength(3), EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3), DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3), DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
