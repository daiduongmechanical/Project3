using System.ComponentModel.DataAnnotations;

namespace Project3.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
