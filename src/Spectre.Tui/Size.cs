namespace Spectre.Tui;

public readonly struct Size(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

    public int Area => Width * Height;
}