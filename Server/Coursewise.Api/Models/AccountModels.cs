using System.ComponentModel.DataAnnotations;

namespace Coursewise.Api.Models
{
    public class LoginModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
   

    public class ResetPasswordModel
    {

        public string UserId { get; set; }
        public string Token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
    public class ChangePasswordModel
        {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }   
        }
    public class ConfirmEmailModel
    {

        public string UserId { get; set; }
        public string Token { get; set; }
        
    }
    public class RegisterModel
    {


        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string reCaptcha { get; set; }
    }
    public class ResetModel
    {
        [RegularExpression("^([A-zÀ-úA-zÀ-ÿ0-9]+)([\\-+.\\'][A-zÀ-úA-zÀ-ÿ0-9]+)*@(([a-zA-Z0-9]+)([\\-][a-zA-Z0-9]+)*\\.){1,5}([A-Za-z]){2,6}$", ErrorMessage = "Please enter a valid email address")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
