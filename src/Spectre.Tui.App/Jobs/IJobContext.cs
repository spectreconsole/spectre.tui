namespace Spectre.Tui.App;

[PublicAPI]
public interface IJobContext
{
    CancellationToken CancellationToken { get; }
    void Send<T>(T data) where T : ApplicationMessage;
    void Broadcast<T>(T data) where T : ApplicationMessage;
}
