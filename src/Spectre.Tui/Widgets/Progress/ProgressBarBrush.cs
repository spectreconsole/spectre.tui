namespace Spectre.Tui;

[PublicAPI]
public abstract class ProgressBarBrush
{
    public abstract Style GetStyle(int cellIndex, int totalCells, TimeSpan elapsed);

    /// <summary>
    /// Gets a value indicating whether or not <see cref="GetStyle"/> returns the same
    /// value for every cell at a given time.
    /// </summary>
    public virtual bool IsSpatiallyUniform => false;

    public static ProgressBarBrush Solid(Style style)
    {
        return new SolidBrush(style);
    }

    public static ProgressBarBrush Gradient(Color from, Color to)
    {
        return new GradientBrush(from, to);
    }

    public static ProgressBarBrush Pulsate(Color from, Color to, TimeSpan? period = null)
    {
        return new PulsateBrush(from, to, period ?? TimeSpan.FromSeconds(1.5));
    }

    public static ProgressBarBrush Wave(Color from, Color to, TimeSpan? period = null)
    {
        return new WaveBrush(from, to, period ?? TimeSpan.FromSeconds(2));
    }

    private sealed class SolidBrush(Style style) : ProgressBarBrush
    {
        public override bool IsSpatiallyUniform => true;

        public override Style GetStyle(int cellIndex, int totalCells, TimeSpan elapsed)
        {
            return style;
        }
    }

    private sealed class GradientBrush(Color from, Color to) : ProgressBarBrush
    {
        public override Style GetStyle(int cellIndex, int totalCells, TimeSpan elapsed)
        {
            var t = totalCells <= 1 ? 0f : Math.Clamp((float)cellIndex / (totalCells - 1), 0f, 1f);
            return new Style(foreground: from.Blend(to, t));
        }
    }

    private sealed class PulsateBrush(Color from, Color to, TimeSpan period) : ProgressBarBrush
    {
        public override bool IsSpatiallyUniform => true;

        public override Style GetStyle(int cellIndex, int totalCells, TimeSpan elapsed)
        {
            var p = period.TotalSeconds;
            var t = p <= 0 ? 0d : elapsed.TotalSeconds / p;
            var f = (float)((1d - Math.Cos(2d * Math.PI * t)) / 2d);
            return new Style(foreground: from.Blend(to, f));
        }
    }

    private sealed class WaveBrush(Color from, Color to, TimeSpan period) : ProgressBarBrush
    {
        public override Style GetStyle(int cellIndex, int totalCells, TimeSpan elapsed)
        {
            if (totalCells <= 0)
            {
                return new Style(foreground: from);
            }

            var s = (double)cellIndex / totalCells;
            var p = period.TotalSeconds;
            var t = p <= 0 ? 0d : elapsed.TotalSeconds / p;

            var phase = (s - t) % 1d;
            if (phase < 0d)
            {
                phase += 1d;
            }

            var factor = (float)((1d - Math.Cos(2d * Math.PI * phase)) / 2d);
            return new Style(foreground: from.Blend(to, factor));
        }
    }
}