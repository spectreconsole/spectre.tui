namespace Spectre.Tui;

[PublicAPI]
public interface ITerminal : IDisposable
{
    void Clear();
    Size GetSize();
    void MoveTo(int x, int y);
    void Write(Cell cell);
    void Flush();
}