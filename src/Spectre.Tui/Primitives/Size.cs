namespace Spectre.Tui;

[PublicAPI]
[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public readonly struct Size(int width, int height) : IEquatable<Size>
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int Area => Width * Height;

    public Rectangle ToRectangle()
    {
        return new Rectangle(0, 0, Width, Height);
    }

    public bool Equals(Size other)
    {
        return Width == other.Width && Height == other.Height;
    }

    public override bool Equals(object? obj)
    {
        return obj is Size other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height);
    }

    public static bool operator ==(Size left, Size right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Size left, Size right)
    {
        return !(left == right);
    }

    private string DebuggerDisplay()
    {
        return $"{Width},{Height}";
    }
}