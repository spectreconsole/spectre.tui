namespace Spectre.Tui;

public interface IRenderBounds
{
    public Rectangle Screen { get; }
    public Rectangle Viewport { get; }
}