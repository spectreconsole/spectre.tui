namespace Spectre.Tui;

public interface ITerminal : IDisposable
{
    void Clear();
    Size GetSize();
    void Write(IEnumerable<(int x, int y, Cell cell)> updates);
    void Flush();
}