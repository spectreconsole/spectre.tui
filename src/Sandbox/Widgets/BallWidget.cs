namespace Sandbox;

public sealed class BallWidget : IStatefulWidget<BallState>
{
    public void Render(RenderContext context, BallState state)
    {
        var x = (int)state.Position.X;
        var y = (int)state.Position.Y;

        context.SetSymbol(x, y, '⬤');
        context.SetForeground(x, y, Color.Red);
    }
}

public sealed class BallState
{
    private const double Speed = 60;

    public Vector2 Position { get; private set; } = new(0, 0);
    public Vector2 Direction { get; private set; } = new(1, 1);

    public void Update(TimeSpan elapsed, Rectangle viewport)
    {
        Position += (Direction * Speed) * elapsed.TotalSeconds;

        if (Position.X >= viewport.Width)
        {
            Position = new Vector2(Position.X - 1, Position.Y);
            Direction = new Vector2(-Direction.X, Direction.Y);
        }

        if (Position.X <= viewport.Left)
        {
            Position = new Vector2(viewport.Left, Position.Y);
            Direction = new Vector2(-Direction.X, Direction.Y);
        }

        if (Position.Y > viewport.Bottom)
        {
            Position = new Vector2(Position.X, viewport.Bottom - 1);
            Direction = new Vector2(Direction.X, -Direction.Y);
        }

        if (Position.Y < viewport.Top)
        {
            Position = new Vector2(Position.X, viewport.Top + 1);
            Direction = new Vector2(Direction.X, -Direction.Y);
        }
    }
}

public readonly record struct Vector2(double X, double Y)
{
    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
        return new Vector2(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2 operator *(Vector2 left, double right)
    {
        return new Vector2(left.X * right, left.Y * right);
    }

    public override string ToString()
    {
        return $"{X},{Y}";
    }
}