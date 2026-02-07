namespace Spectre.Tui;

public sealed class Border
{
    public required char TopLeft { get; init; }
    public required char TopRight { get; init; }
    public required char BottomLeft { get; init; }
    public required char BottomRight { get; init; }
    public required char VerticalLeft { get; init; }
    public required char VerticalRight { get; init; }
    public required char HorizontalTop { get; init; }
    public required char HorizontalBottom { get; init; }

    public static Border FromLine(Line line)
    {
        return new Border
        {
            TopLeft = line.TopLeft,
            TopRight = line.TopRight,
            BottomLeft = line.BottomLeft,
            BottomRight = line.BottomRight,
            VerticalLeft = line.Vertical,
            VerticalRight = line.Vertical,
            HorizontalTop = line.Horizontal,
            HorizontalBottom = line.Horizontal,
        };
    }

    public static Border Plain { get; } = FromLine(Line.Plain);
    public static Border Rounded { get; } = FromLine(Line.Rounded);
    public static Border Double { get; } = FromLine(Line.Double);
    public static Border Bold { get; } = FromLine(Line.Bold);
}
