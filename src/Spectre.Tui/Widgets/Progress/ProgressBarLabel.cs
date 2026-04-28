namespace Spectre.Tui;

[PublicAPI]
public abstract class ProgressBarLabel
{
    public abstract string Format(double value, double max);

    public virtual int MeasureWidth(string formatted, double max)
    {
        return formatted.GetCellWidth();
    }

    public static ProgressBarLabel Percentage { get; } = new PercentageLabel();
    public static ProgressBarLabel Fraction { get; } = new FractionLabel();

    public static ProgressBarLabel Custom(Func<double, double, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);
        return new CustomLabel(formatter);
    }

    private sealed class PercentageLabel : ProgressBarLabel
    {
        public override string Format(double value, double max)
        {
            var ratio = max > 0 ? Math.Clamp(value / max, 0d, 1d) : 0d;
            var percent = (int)Math.Round(ratio * 100d, MidpointRounding.AwayFromZero);
            return percent.ToString(CultureInfo.InvariantCulture) + "%";
        }

        public override int MeasureWidth(string formatted, double max)
        {
            // There are 4 characters in '100%'
            return 4;
        }
    }

    private sealed class FractionLabel : ProgressBarLabel
    {
        public override string Format(double value, double max)
        {
            var v = ((long)Math.Round(value, MidpointRounding.AwayFromZero)).ToString(CultureInfo.InvariantCulture);
            var m = ((long)Math.Round(max, MidpointRounding.AwayFromZero)).ToString(CultureInfo.InvariantCulture);
            return v + "/" + m;
        }

        public override int MeasureWidth(string formatted, double max)
        {
            var digits = ((long)Math.Round(Math.Abs(max), MidpointRounding.AwayFromZero)).GetDigitCount();
            return (digits * 2) + 1;
        }
    }

    private sealed class CustomLabel(Func<double, double, string> formatter) : ProgressBarLabel
    {
        public override string Format(double value, double max)
        {
            return formatter(value, max) ?? string.Empty;
        }
    }
}
