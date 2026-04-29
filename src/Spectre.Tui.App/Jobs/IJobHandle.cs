namespace Spectre.Tui.App;

[PublicAPI]
public interface IJobHandle
{
    Task Completion { get; }
    void Cancel();
}
