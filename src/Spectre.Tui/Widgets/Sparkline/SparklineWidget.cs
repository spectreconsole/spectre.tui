namespace Spectre.Tui;

[PublicAPI]
public sealed class SparklineWidget : IWidget
{
    private static readonly char[] _glyphs =
    [
        Bar.OneEight,
        Bar.OneQuarter,
        Bar.ThreeEights,
        Bar.Half,
        Bar.FiveEights,
        Bar.ThreeQuarters,
        Bar.SevenEights,
    ];

    public List<SparklineSegment> Data { get; } = [];
    public ulong? Max { get; set; }
    public SparklineDirection Direction { get; set; } = SparklineDirection.LeftToRight;
    public Style? Style { get; set; }
    public Style? AbsentValueStyle { get; set; }
    public char AbsentValueSymbol { get; set; } = ' ';

    void IWidget.Render(RenderContext context)
    {
        var area = context.Viewport;
        if (area.Width <= 0 || area.Height <= 0 || Data.Count == 0)
        {
            return;
        }

        var max = Max ?? ResolveAutoMax(Data);
        var maxEighths = area.Height * 8L;

        for (var x = 0; x < area.Width; x++)
        {
            // Right-to-left pins the newest sample (last in the list) to the right edge.
            var index = Direction == SparklineDirection.LeftToRight
                ? x
                : Data.Count - area.Width + x;

            if (index < 0 || index >= Data.Count)
            {
                continue;
            }

            var segment = Data[index];
            if (segment.Value is null)
            {
                for (var y = 0; y < area.Height; y++)
                {
                    context.SetSymbol(x, y, AbsentValueSymbol);
                    context.SetStyle(x, y, AbsentValueStyle);
                }

                continue;
            }

            if (max == 0)
            {
                continue;
            }

            var totalEighths = Math.Clamp(
                (long)Math.Round(
                    (double)segment.Value.Value / max * maxEighths,
                    MidpointRounding.AwayFromZero),
                0L, maxEighths);

            var fullCells = (int)(totalEighths / 8);
            var remainder = (int)(totalEighths % 8);
            var combined = Style?.Combine(segment.Style) ?? segment.Style ?? Style;

            // Full cells
            for (var amplitude = 0; amplitude < fullCells; amplitude++)
            {
                var y = area.Height - 1 - amplitude;
                context.SetSymbol(x, y, Bar.Full);
                context.SetStyle(x, y, combined);
            }

            // Partial cell
            if (remainder > 0 && fullCells < area.Height)
            {
                var y = area.Height - 1 - fullCells;
                context.SetSymbol(x, y, _glyphs[remainder - 1]);
                context.SetStyle(x, y, combined);
            }
        }
    }

    private static ulong ResolveAutoMax(List<SparklineSegment> data)
    {
        ulong max = 0;
        foreach (var segment in data)
        {
            if (segment.Value is { } value && value > max)
            {
                max = value;
            }
        }

        return max;
    }
}

[PublicAPI]
public static class SparklineWidgetExtensions
{
    extension(SparklineWidget widget)
    {
        public SparklineWidget Data(params IEnumerable<SparklineSegment> data)
        {
            widget.Data.Clear();
            widget.Data.AddRange(data);
            return widget;
        }

        public SparklineWidget Data(params IEnumerable<ulong> data)
        {
            widget.Data.Clear();
            foreach (var value in data)
            {
                widget.Data.Add(new SparklineSegment(value));
            }

            return widget;
        }

        public SparklineWidget Max(ulong? max)
        {
            widget.Max = max;
            return widget;
        }

        public SparklineWidget AutoMax()
        {
            widget.Max = null;
            return widget;
        }

        public SparklineWidget Direction(SparklineDirection direction)
        {
            widget.Direction = direction;
            return widget;
        }

        public SparklineWidget Style(Style? style)
        {
            widget.Style = style;
            return widget;
        }

        public SparklineWidget AbsentValueSymbol(char symbol)
        {
            widget.AbsentValueSymbol = symbol;
            return widget;
        }

        public SparklineWidget AbsentValueStyle(Style? style)
        {
            widget.AbsentValueStyle = style;
            return widget;
        }
    }
}