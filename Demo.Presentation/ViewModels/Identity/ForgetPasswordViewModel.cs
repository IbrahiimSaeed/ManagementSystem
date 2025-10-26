using System.ComponentModel.DataAnnotations;

namespace Demo.Presentation.ViewModels.Identity
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email can not be empty!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
