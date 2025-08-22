using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nakisa.Contracts.Bot;
using Nakisa.Infrastructure.BotClient;
using WTelegram;

namespace Nakisa.API.Controllers;

[ApiController]
[Route("auth/telegram")]
public class TelegramAuthController : ControllerBase
{
    private static Client? _client;
    private static TaskCompletionSource<string>? _codeTcs;
    private static TaskCompletionSource<string>? _passwordTcs;
    private readonly TelegramClientOptions _options;
    private readonly TelegramConfigProvider _configProvider;

    public TelegramAuthController(IOptions<TelegramClientOptions> options, TelegramConfigProvider configProvider)
    {
        _configProvider = configProvider;
        _options = options.Value;
    }
    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        _configProvider.StartLogin();
        _client = new Client(_configProvider.Config);
        var user = await _client.LoginUserIfNeeded();
        return Ok(user);
    }

    [HttpPost("code")]
    public IActionResult SubmitCode([FromBody] string code)
    {
        _configProvider.ProvideCode(code);
        return Ok("Code submitted");
    }

    [HttpPost("password")]
    public IActionResult SubmitPassword([FromBody] string password)
    {
        _configProvider.ProvidePassword(password);
        return Ok("Password submitted");
    }
    
    
    
    [HttpPost("session")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Session([FromForm] SessionDto session)
    {
        var sessionFile = session.SessionFile;
        if (sessionFile == null || sessionFile.Length == 0)
            return BadRequest("No file uploaded");

        var destPath = "/tmp/WTelegram.session";

        if (System.IO.File.Exists(destPath))
            System.IO.File.Delete(destPath);

        await using var stream = new FileStream(destPath, FileMode.Create, FileAccess.Write);
        await sessionFile.CopyToAsync(stream);

        _options.SessionPath = destPath;

        return Ok("Session uploaded successfully");
    }
}

public class SessionDto
{
    public IFormFile SessionFile{get;set;}
}