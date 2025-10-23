using Microsoft.AspNetCore.Mvc;
using Nakisa.Application.DTOs.TelegramSession;
using Nakisa.Application.Interfaces;

namespace Nakisa.API.Controllers;

[ApiController]
[Route("api/telegram")]
public class TelegramController : ControllerBase
{
    private readonly ITelegramClientService _telegramClientService;
    public TelegramController(ITelegramClientService telegramClientService) => _telegramClientService = telegramClientService;

    [HttpPost("session/start")]
    public IActionResult Start()
    {
        _telegramClientService.StartLoginAsync();
        return Accepted(new { message = "started, waiting for verification code..." });
    }

    [HttpPost("session/code")]
    public IActionResult Code([FromBody] CodeDto dto)
    {
        _telegramClientService.ProvideCode(dto.Code);
        return Ok(new { message = "code provided" });
    }

    [HttpPost("session/password")]
    public IActionResult Password([FromBody] PasswordDto dto)
    {
        _telegramClientService.ProvidePassword(dto.Password);
        return Ok(new { message = "password provided" });
    }

    [HttpGet("session/status")]
    public IActionResult GetSessionStatus()
    {
        bool isAuthenticated = _telegramClientService.IsAuthenticated();
    
        if (isAuthenticated)
        {
            return Ok(new { 
                status = "authenticated",
                message = "User is logged in",
            });
        }
    
        return Unauthorized(new { 
            status = "unauthenticated", 
            message = "Please login first" 
        });
    }
}