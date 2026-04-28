namespace Spectre.Tui;

[PublicAPI]
public abstract class ProgressBarFill
{
    public abstract void Render(
        RenderContext context,
        int width,
        double ratio,
        ProgressBarBrush? foreground,
        ProgressBarBrush? background,
        TimeSpan elapsed);

    public static ProgressBarFill Default { get; } = Custom(Block.Full, Block.Shade.Medium);
    public static ProgressBarFill Smooth { get; } = new SmoothFill();
    public static ProgressBarFill Custom(char foreground, char background) => new CustomFill(foreground, background);

    private sealed class CustomFill(char filled, char unfilled) : ProgressBarFill
    {
        public override void Render(
            RenderContext context,
            int width,
            double ratio,
            ProgressBarBrush? foreground,
            ProgressBarBrush? background,
            TimeSpan elapsed)
        {
            var filledCells = (int)Math.Round(ratio * width, MidpointRounding.AwayFromZero);
            if (filledCells < 0)
            {
                filledCells = 0;
            }
            else if (filledCells > width)
            {
                filledCells = width;
            }

            var fgStyle = foreground?.IsSpatiallyUniform ?? false
                ? foreground.GetStyle(0, width, elapsed)
                : (Style?)null;
            var bgStyle = background?.IsSpatiallyUniform ?? false
                ? background.GetStyle(0, width, elapsed)
                : (Style?)null;

            for (var index = 0; index < width; index++)
            {
                if (index < filledCells)
                {
                    var style = fgStyle ?? foreground?.GetStyle(index, width, elapsed);
                    SetCell(context, index, filled, style);
                }
                else
                {
                    var style = bgStyle ?? background?.GetStyle(index, width, elapsed);
                    SetCell(context, index, unfilled, style);
                }
            }
        }
    }

    private sealed class SmoothFill : ProgressBarFill
    {
        private static readonly char[] _partialGlyphs =
        [
            Block.OneEight,
            Block.OneQuarter,
            Block.ThreeEights,
            Block.Half,
            Block.FiveEights,
            Block.ThreeQuarters,
            Block.SevenEights,
        ];

        public override void Render(
            RenderContext context,
            int width,
            double ratio,
            ProgressBarBrush? foreground,
            ProgressBarBrush? background,
            TimeSpan elapsed)
        {
            var maxEighths = width * 8L;
            var totalEighths = (long)Math.Round(ratio * maxEighths, MidpointRounding.AwayFromZero);
            if (totalEighths < 0)
            {
                totalEighths = 0;
            }
            else if (totalEighths > maxEighths)
            {
                totalEighths = maxEighths;
            }

            var fullCells = (int)(totalEighths / 8);
            var partial = (int)(totalEighths % 8);

            var fgStyle = foreground?.IsSpatiallyUniform ?? false
                ? foreground.GetStyle(0, width, elapsed) : (Style?)null;
            var bgStyle = background?.IsSpatiallyUniform ?? false
                ? background.GetStyle(0, width, elapsed) : (Style?)null;

            for (var index = 0; index < fullCells; index++)
            {
                var style = fgStyle ?? foreground?.GetStyle(index, width, elapsed);
                SetCell(context, index, Block.Full, style);
            }

            if (partial > 0 && fullCells < width)
            {
                var glyph = _partialGlyphs[partial - 1];
                var fgColor = (fgStyle ?? foreground?.GetStyle(fullCells, width, elapsed))?.Foreground;
                var bgColor = (bgStyle ?? background?.GetStyle(fullCells, width, elapsed))?.Foreground;
                var partialStyle = new Style(foreground: fgColor, background: bgColor);
                SetCell(context, fullCells, glyph, partialStyle);
            }

            var unfillCanonical = bgStyle is { } u ? MakeSolid(u) : (Style?)null;
            var unfilledStart = fullCells + (partial > 0 ? 1 : 0);
            for (var index = unfilledStart; index < width; index++)
            {
                Style? solid;
                if (bgStyle != null)
                {
                    solid = unfillCanonical;
                }
                else
                {
                    var raw = background?.GetStyle(index, width, elapsed);
                    solid = raw is { } r ? MakeSolid(r) : (Style?)null;
                }

                SetCell(context, index, ' ', solid);
            }
        }

        private static Style MakeSolid(Style style)
        {
            if (style.Foreground == Color.Default)
            {
                return style;
            }

            return new Style(
                foreground: style.Foreground,
                background: style.Foreground,
                decoration: style.Decoration);
        }
    }

    private static void SetCell(RenderContext context, int x, char glyph, Style? style)
    {
        var cell = context.GetCell(x, 0);
        if (cell is null)
        {
            return;
        }

        cell.SetSymbol(glyph);
        if (style is not null)
        {
            cell.SetStyle(style);
        }
    }
}