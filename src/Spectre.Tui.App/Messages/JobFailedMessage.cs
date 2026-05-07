namespace Spectre.Tui.App;

[PublicAPI]
public sealed record JobFailedMessage(IJobHandle Job, Exception Exception) : ApplicationMessage;