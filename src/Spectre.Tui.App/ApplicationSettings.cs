namespace Spectre.Tui.App;

public sealed class ApplicationSettings
{
    public int TargetFps { get; init; } = 60;
    public IInputReader? InputReader { get; init; }
    public ITerminal? Terminal { get; init; }
}