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
    private static TaskCompletionSource<string>? _codeTcs;
    private static TaskCompletionSource<string>? _passwordTcs;
    private readonly TelegramClientOptions _options;

    public TelegramAuthController(IOptions<TelegramClientOptions> options)
    {
        _options = options.Value;
    }
    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        _codeTcs = new TaskCompletionSource<string>();
        _passwordTcs = new TaskCompletionSource<string>();

        _client = new Client(Config);
        var user = await _client.LoginUserIfNeeded();
        return Ok(user);
    }

    [HttpPost("code")]
    public IActionResult SubmitCode([FromBody] string code)
    {
        _codeTcs?.TrySetResult(code);
        return Ok("Code submitted");
    }

    [HttpPost("password")]
    public IActionResult SubmitPassword([FromBody] string password)
    {
        _passwordTcs?.TrySetResult(password);
        return Ok("Password submitted");
    }

    private string? Config(string what)
    {
        return what switch
        {
            "api_id" => _options.ApiId.ToString(),
            "api_hash" => _options.ApiHash,
            "phone_number" => _options.PhoneNumber,
            // "session_pathname" => _options.SessionPath,
            "verification_code" => _codeTcs!.Task.Result,
            "password" => _passwordTcs!.Task.Result,

            _ => null
        };
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