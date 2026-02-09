namespace Spectre.Tui;

[PublicAPI]
public readonly record struct Appearance()
{
    public Color Foreground { get; init; } = Color.Default;
    public Color Background { get; init; } = Color.Default;
    public Decoration Decoration { get; init; } = Decoration.None;

    public static Appearance Plain { get; } = new();

    public static implicit operator Appearance(Color color)
    {
        return new Appearance
        {
            Foreground = color,
        };
    }

    public static implicit operator Appearance(Style style)
    {
        return new Appearance
        {
            Foreground = style.Foreground,
            Background = style.Background,
            Decoration = style.Decoration,
        };
    }

    public static implicit operator Style(Appearance appearance)
    {
        return appearance.ToStyle();
    }

    public Style ToStyle()
    {
        return new Style(Foreground, Background, Decoration);
    }

    public Appearance Combine(Appearance? other)
    {
        if (other is null)
        {
            return this;
        }

        var foreground = Foreground;
        if (other.Value.Foreground != Color.Default)
        {
            foreground = other.Value.Foreground;
        }

        var background = Background;
        if (other.Value.Background != Color.Default)
        {
            background = other.Value.Background;
        }

        return new Appearance
        {
            Foreground = foreground,
            Background = background,
            Decoration = Decoration | other.Value.Decoration,
        };
    }

    internal Appearance Combine(IEnumerable<Appearance> source)
    {
        return source.Aggregate(this, (current, next) => current.Combine(next));
    }
}