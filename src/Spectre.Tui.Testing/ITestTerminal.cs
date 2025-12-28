namespace Spectre.Tui.Testing;

public interface ITestTerminal : ITerminal
{
    public string Output
    {
        get;
    }
}
