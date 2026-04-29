namespace Spectre.Tui.App;

[PublicAPI]
public abstract record ApplicationEvent;

[PublicAPI]
public sealed record KeyEvent(ConsoleKeyInfo Key) : ApplicationEvent;

[PublicAPI]
public sealed record JobFailed(IJobHandle Job, Exception Exception) : ApplicationEvent;