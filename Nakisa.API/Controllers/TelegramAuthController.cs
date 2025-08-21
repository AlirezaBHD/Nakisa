using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nakisa.Contracts.Bot;
using WTelegram;

namespace Nakisa.API.Controllers;

[ApiController]
[Route("auth/telegram")]
public class TelegramAuthController : ControllerBase
{
    private static Client? _client;
    private static string? _pendingCode;
    private static string? _pendingPassword;
    private readonly TelegramClientOptions _options;

    public TelegramAuthController(IOptions<TelegramClientOptions> options)
    {
        _options = options.Value;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        _client = new Client(Config);
        var user = await _client.LoginUserIfNeeded();
        return Ok(user);
    }

    [HttpPost("code")]
    public IActionResult SubmitCode([FromBody] string code)
    {
        _pendingCode = code;
        return Ok("Code submitted, continue login...");
    }

    [HttpPost("password")]
    public IActionResult SubmitPassword([FromBody] string password)
    {
        _pendingPassword = password;
        return Ok("Password submitted, continue login...");
    }

    private string? Config(string what)
    {
        return what switch
        {
            "api_id"             => _options.ApiId.ToString(),
            "api_hash"           => _options.ApiHash,
            "phone_number"       => _options.PhoneNumber,
            "verification_code"  => WaitFor(ref _pendingCode),
            "password"           => WaitFor(ref _pendingPassword),
            _ => null
        };
    }

    private static string WaitFor(ref string? field)
    {
        while (field == null)
            Thread.Sleep(500);
        var value = field;
        field = null;
        return value!;
    }
}