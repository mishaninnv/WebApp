using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="Поле обязательно для заполнения")]
        [MinLength(11, ErrorMessage = "Длина 11 символов")]
        [StringLength(11, ErrorMessage = "Длина 11 символов")]
        [RegularExpression(@"^7\d*", ErrorMessage = "Номер должен начинаться с 7")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [StringLength(20, ErrorMessage = "Максимальная длина 20 символов")]
        public string Password { get; set; }
    }
}
