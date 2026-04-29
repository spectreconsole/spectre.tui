namespace Spectre.Tui.App;

[PublicAPI]
public interface IJobContext
{
    CancellationToken CancellationToken { get; }
    void Send<T>(T data) where T : ApplicationEvent;
    void Broadcast<T>(T data) where T : ApplicationEvent;
}
