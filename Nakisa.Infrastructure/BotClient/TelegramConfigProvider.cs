using Microsoft.Extensions.Options;
using Nakisa.Contracts.Bot;

namespace Nakisa.Infrastructure.BotClient;

public class TelegramConfigProvider
{
    private readonly TelegramClientOptions _options;

    public TelegramConfigProvider(IOptions<TelegramClientOptions> options)
    {
        _options = options.Value;
    }

    public string? Config(string what) => what switch
    {
        "api_id"            => _options.ApiId.ToString(),
        "api_hash"          => _options.ApiHash,
        "phone_number"      => _options.PhoneNumber,
        "session_pathname"  => _options.SessionPath,
        "verification_code" => _options.VerificationCode,
        "password"          => "123",
        _ => null
    };
}
