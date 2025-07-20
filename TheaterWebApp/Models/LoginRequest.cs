using System.ComponentModel.DataAnnotations;

namespace TheaterWebApp.Models;

public class LoginRequest
{
    [EmailAddress(ErrorMessage = "이메일 형식에 맞게 입력해주세요.")]
    [Required(ErrorMessage = "이메일은 필수입니다.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "비밀번호는 필수입니다.")]
    public string Password { get; set; }
}