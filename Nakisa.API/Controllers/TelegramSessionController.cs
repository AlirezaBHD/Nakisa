using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nakisa.Contracts.Bot;

namespace Nakisa.API.Controllers;


// TEMPORARY!
// Temporarily bypassing the free deployment server's limitations.

[ApiController]
[Route("api/[controller]")]

public class TelegramSessionController : ControllerBase
{
    private readonly TelegramClientOptions _options;

    public TelegramSessionController(IOptions<TelegramClientOptions> options)
    {
        _options = options.Value;
    }
    
    [HttpPost("session")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Session([FromForm] SessionDto session)
    {
        var sessionFile = session.SessionFile;
        if (sessionFile == null || sessionFile.Length == 0)
            return BadRequest("No file uploaded");

        var destPath = _options.SessionPath;

        if (System.IO.File.Exists(destPath))
            return BadRequest("File already exists");

        await using var stream = new FileStream(destPath!, FileMode.Create, FileAccess.Write);
        await sessionFile.CopyToAsync(stream);
        
        return Ok("Session uploaded successfully");
    }
}

public class SessionDto
{
    public IFormFile? SessionFile { get; set; }
}