namespace Spectre.Tui;

[PublicAPI]
public readonly record struct Style()
{
    public Color Foreground { get; init; } = Color.Default;
    public Color Background { get; init; } = Color.Default;
    public Decoration Decoration { get; init; } = Decoration.None;

    public static Style Plain { get; } = new();

    public static implicit operator Style(Color color)
    {
        return new Style
        {
            Foreground = color,
        };
    }

    public Style Combine(Style? other)
    {
        if (other is null)
        {
            return this;
        }

        var foreground = Foreground;
        if (!other.Value.Foreground.IsDefault)
        {
            foreground = other.Value.Foreground;
        }

        var background = Background;
        if (!other.Value.Background.IsDefault)
        {
            background = other.Value.Background;
        }

        return new Style
        {
            Foreground = foreground,
            Background = background,
            Decoration = Decoration | other.Value.Decoration,
        };
    }
}