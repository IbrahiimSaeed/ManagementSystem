using System.ComponentModel.DataAnnotations;

namespace Demo.Presentation.ViewModels.Identity
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Firstname can not be null")]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Lastname can not be null")]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username can not be null")]
        [MaxLength(50)]
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public bool IsAgreed { get; set; }
    }
}
