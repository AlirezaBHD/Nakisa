using Nakisa.Domain.Enums;

namespace Nakisa.Application.DTOs;

public class RegisterDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public RegisterStep Step { get; set; }
}