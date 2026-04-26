namespace Spectre.Tui;

public readonly struct Padding : IEquatable<Padding>
{
    public int Left { get; }
    public int Top { get; }
    public int Right { get; }
    public int Bottom { get; }

    public Padding(int size)
        : this(size, size, size, size)
    {
    }

    public Padding(int horizontal, int vertical)
        : this(horizontal, vertical, horizontal, vertical)
    {
    }

    public Padding(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public override bool Equals(object? obj)
    {
        return obj is Padding padding && Equals(padding);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = (int)2166136261;
            hash = (hash * 16777619) ^ Left.GetHashCode();
            hash = (hash * 16777619) ^ Top.GetHashCode();
            hash = (hash * 16777619) ^ Right.GetHashCode();
            hash = (hash * 16777619) ^ Bottom.GetHashCode();
            return hash;
        }
    }

    public bool Equals(Padding other)
    {
        return Left == other.Left
               && Top == other.Top
               && Right == other.Right
               && Bottom == other.Bottom;
    }

    public static bool operator ==(Padding left, Padding right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Padding left, Padding right)
    {
        return !(left == right);
    }

    public int GetWidth()
    {
        return Left + Right;
    }

    public int GetHeight()
    {
        return Top + Bottom;
    }
}

/// <summary>
/// Contains extension methods for <see cref="Padding"/>.
/// </summary>
public static class PaddingExtensions
{
    public static int GetLeftSafe(this Padding? padding)
    {
        return padding?.Left ?? 0;
    }

    public static int GetRightSafe(this Padding? padding)
    {
        return padding?.Right ?? 0;
    }

    public static int GetTopSafe(this Padding? padding)
    {
        return padding?.Top ?? 0;
    }

    public static int GetBottomSafe(this Padding? padding)
    {
        return padding?.Bottom ?? 0;
    }
}