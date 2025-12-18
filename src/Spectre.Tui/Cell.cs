namespace Spectre.Tui;

[PublicAPI]
[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public sealed class Cell : IEquatable<Cell>
{
    public string Symbol { get; private set; } = " ";
    public Style Style { get; set; } = Style.Plain;

    public Decoration Decoration => Style.Decoration;
    public Color Foreground => Style.Foreground;
    public Color Background => Style.Background;

    internal Cell SetSymbol(string? text)
    {
        Symbol = text ?? " ";
        return this;
    }

    public Cell SetSymbol(Rune rune)
    {
        Symbol = rune.ToString();
        return this;
    }

    public Cell SetStyle(Style? style)
    {
        Style = style ?? Style.Plain;
        return this;
    }

    public Cell SetDecoration(Decoration? decoration)
    {
        Style = Style with { Decoration = decoration ?? Decoration.None };
        return this;
    }

    public Cell SetForeground(Color? color)
    {
        Style = Style with { Foreground = color ?? Color.Default };
        return this;
    }

    public Cell SetBackground(Color? color)
    {
        Style = Style with { Background = color ?? Color.Default };
        return this;
    }

    public Cell Clone()
    {
        return new Cell { Symbol = Symbol, Style = Style };
    }

    private string DebuggerDisplay()
    {
        return Symbol;
    }

    public bool Equals(Cell? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Symbol.Equals(other.Symbol) && Decoration == other.Decoration && Foreground.Equals(other.Foreground) &&
               Background.Equals(other.Background);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Cell other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Symbol, (int)Decoration, Foreground, Background);
    }

    public void Reset()
    {
        Symbol = " ";
        Style = Style.Plain;
    }
}

public static class CellExtensions
{
    extension(Cell cell)
    {
        public Cell SetSymbol(char symbol)
        {
            return cell.SetSymbol(new Rune(symbol));
        }
    }
}