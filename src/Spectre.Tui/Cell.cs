using System.Text;

namespace Spectre.Tui;

public struct Cell
{
    public Rune Rune { get; init; } = new Rune(' ');
    public Decoration Decoration { get; init; } = Decoration.None;

    public Cell()
    {
    }
}