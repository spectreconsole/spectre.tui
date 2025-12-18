using System.Runtime.CompilerServices;

namespace Spectre.Tui;

[PublicAPI]
[DebuggerDisplay("{DebuggerDisplay(),nq}")]
public readonly struct Rectangle(int x, int y, int width, int height)
    : IEquatable<Rectangle>
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int Top => Y;
    public int Bottom => Y + Height;
    public int Left => X;
    public int Right => X + Width;

    public bool IsEmpty => X == 0 || Y == 0;

    public bool Equals(Rectangle other)
    {
        return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rectangle other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Width, Height);
    }

    public int CalculateArea()
    {
        return Width * Height;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(Position position)
    {
        return position.X >= X && position.X <= X + Width &&
               position.Y >= Y && position.Y <= Y + Height;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(int x, int y)
    {
        return x >= X && x <= X + Width &&
               y >= Y && y <= Y + Height;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Rectangle value)
    {
        return value.Left < Right && Left < value.Right &&
               value.Top < Bottom && Top < value.Bottom;
    }

    public Rectangle Inflate(Size size)
    {
        return Inflate(size.Width, size.Height);
    }

    public Rectangle Inflate(int width, int height)
    {
        return new Rectangle(
            x: Math.Max(0, X - width),
            y: Math.Max(0, Y - height),
            width: Math.Max(0, Width + (2 * width)),
            height: Math.Max(0, Height + (2 * height))
        );
    }

    public Rectangle Offset(Position offset)
    {
        return Offset(offset.X, offset.Y);
    }

    public Rectangle Intersect(Rectangle other)
    {
        if (!Intersects(other))
        {
            throw new InvalidOperationException("The two rectangles do not intersect");
        }

        var right = Math.Min(X + Width, other.X + other.Width);
        var left = Math.Max(X, other.X);
        var top = Math.Max(Y, other.Y);
        var bottom = Math.Min(Y + Height, other.Y + other.Height);

        return new Rectangle(left, top, right - left, bottom - top);
    }

    public Rectangle Offset(int offsetX, int offsetY)
    {
        return new Rectangle(
            x: X + offsetX,
            y: Y + offsetY,
            Width, Height
        );
    }

    public Rectangle Union(Rectangle other)
    {
        var x = Math.Min(X, other.X);
        var y = Math.Min(Y, other.Y);
        return new Rectangle(x, y,
            Math.Max(Right, other.Right) - x,
            Math.Max(Bottom, other.Bottom) - y);
    }

    public void Deconstruct(out int x, out int y, out int width, out int height)
    {
        x = X;
        y = Y;
        width = Width;
        height = Height;
    }

    public static bool operator ==(Rectangle left, Rectangle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rectangle left, Rectangle right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return $"{X},{Y},{Width},{Height}";
    }

    private string DebuggerDisplay()
    {
        return $"{X},{Y},{Width},{Height}";
    }
}