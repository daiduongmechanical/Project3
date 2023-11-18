using System.ComponentModel.DataAnnotations;

namespace Project3.Dtos
{
    public class RegisterDto

    {
        [Required(ErrorMessage = "Head doesn't empty")]
        public string Head {  get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]

        public string? Name { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).+$", ErrorMessage = "Mật khẩu phải có ít nhât 1 số, 1 kí tự hoa và 1 kí tự đặc biệt")]
        public string? Pass { get; set; }
        [Required(ErrorMessage = "Phone không được để trống")]
        [RegularExpression(@"^[+-]?\d{9,13}$", ErrorMessage = "Please enter a valid phone number with 9 to 11 digits.")]
        public string Phone { get; set; }
       
       
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).+$", ErrorMessage = "Mật khẩu phải có ít nhât 1 số, 1 kí tự hoa và 1 kí tự đặc biệt")]
        [Compare("Pass", ErrorMessage = "xác nhận mật khẩu không đúng")]
        [Required(ErrorMessage = "xác nhận mật khẩu không được để trống")]
        public string? ConfirmPassword { get; set; }
    }
}
