using System.ComponentModel.DataAnnotations;

namespace Project3.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).+$", ErrorMessage = "Mật khẩu phải có ít nhât 1 số, 1 kí tự hoa và 1 kí tự đặc biệt")]
        public string? Password { get; set; }
        public string? Device_id { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string? Head {  get; set; }
    }
}
