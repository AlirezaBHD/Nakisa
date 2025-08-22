using Microsoft.Extensions.Options;
using Nakisa.Contracts.Bot;

namespace Nakisa.Infrastructure.BotClient;

public class TelegramConfigProvider
{
    private readonly TelegramClientOptions _options;

    private TaskCompletionSource<string>? _codeTcs;
    private TaskCompletionSource<string>? _passwordTcs;

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
        "verification_code" => _codeTcs?.Task.Result,
        "password"          => _passwordTcs?.Task.Result,
        _ => null
    };

    public void ProvideCode(string code)
    {
        _codeTcs?.TrySetResult(code);
    }

    public void ProvidePassword(string password)
    {
        _passwordTcs?.TrySetResult(password);
    }

    public void StartLogin()
    {
        _codeTcs = new TaskCompletionSource<string>();
        _passwordTcs = new TaskCompletionSource<string>();
    }
}
