namespace Spectre.Tui;

[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public sealed class Cell : IEquatable<Cell>
{
    public Rune Rune { get; set; } = new(' ');
    public Decoration Decoration { get; set; } = Decoration.None;
    public Color Foreground { get; set; } = Color.Default;
    public Color Background { get; set; } = Color.Default;

    public Cell SetRune(Rune rune)
    {
        Rune = rune;
        return this;
    }

    public Cell SetDecoration(Decoration? decoration)
    {
        Decoration = decoration ?? Decoration.None;
        return this;
    }

    public Cell SetForeground(Color? color)
    {
        Foreground = color ?? Color.Default;
        return this;
    }

    public Cell SetBackground(Color? color)
    {
        Background = color ?? Color.Default;
        return this;
    }

    public Cell Clone()
    {
        return new Cell { Rune = Rune, Background = Background, Foreground = Foreground, Decoration = Decoration };
    }

    private string DebuggerDisplay()
    {
        return ((char)Rune.Value).ToString();
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

        return Rune.Equals(other.Rune) && Decoration == other.Decoration && Foreground.Equals(other.Foreground) && Background.Equals(other.Background);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Cell other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Rune, (int)Decoration, Foreground, Background);
    }

    public void Reset()
    {
        Rune = new Rune(' ');
        Foreground = Color.Default;
        Background = Color.Default;
        Decoration = Decoration.None;
    }
}