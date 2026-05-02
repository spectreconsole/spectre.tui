namespace Spectre.Tui.App;

[PublicAPI]
public abstract record ApplicationMessage;

[PublicAPI]
public sealed record KeyMessage(ConsoleKeyInfo Info) : ApplicationMessage;

[PublicAPI]
public sealed record JobFailedMessage(IJobHandle Job, Exception Exception) : ApplicationMessage;