using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(250, ErrorMessage = "Максимальная длина 250 символов")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Phone]
        [MinLength(11, ErrorMessage = "Длина 11 символов")]
        [StringLength(11, ErrorMessage = "Длина 11 символов")]
        [RegularExpression(@"^7\d*", ErrorMessage = "Номер должен начинаться с 7")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(150, ErrorMessage = "Максимальная длина 150 символов")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Compare("PasswordConfirm", ErrorMessage = "Введенные пароли должны совпадать")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        [Compare("Password", ErrorMessage = "Введенные пароли должны совпадать")]
        public string PasswordConfirm { get; set; }
    }
}
