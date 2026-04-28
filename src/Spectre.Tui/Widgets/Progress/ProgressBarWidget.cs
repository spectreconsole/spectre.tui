namespace Spectre.Tui;

[PublicAPI]
public sealed class ProgressBarWidget : IWidget
{
    private TimeSpan _elapsed = TimeSpan.Zero;

    public double Value { get; set; }
    public double Max { get; set; } = 100d;

    public ProgressBarFill Fill { get; set; } = ProgressBarFill.Default;

    public ProgressBarBrush? Foreground { get; set; }
    public ProgressBarBrush? Background { get; set; }

    public bool ShowLabel { get; set; } = true;
    public ProgressBarLabel Label { get; set; } = ProgressBarLabel.Percentage;
    public Style? LabelStyle { get; set; }

    public void Update(FrameInfo frame)
    {
        _elapsed += frame.FrameTime;
    }

    public void Render(RenderContext context)
    {
        var area = context.Viewport;
        if (area.Width <= 0 || area.Height <= 0)
        {
            return;
        }

        string? label = null;
        var labelWidth = 0;
        var formatWidth = 0;
        var barWidth = area.Width;

        if (ShowLabel)
        {
            label = Label.Format(Value, Max);
            formatWidth = label.GetCellWidth();
            labelWidth = Math.Max(Label.MeasureWidth(label, Max), formatWidth);

            // Reserve label width plus a single-cell gap. Drop the label
            // entirely when it would leave fewer than 3 cells for the bar
            var reserved = labelWidth + 1;
            if (area.Width - reserved >= 3)
            {
                barWidth = area.Width - reserved;
            }
            else
            {
                label = null;
                labelWidth = 0;
            }
        }

        if (barWidth > 0)
        {
            var ratio = Max > 0 ? Math.Clamp(Value / Max, 0d, 1d) : 0d;
            Fill.Render(context, barWidth, ratio, Foreground, Background, _elapsed);
        }

        if (label is not null)
        {
            // Right-align inside the reserved label field
            var padding = labelWidth - formatWidth;
            var x = barWidth + 1;
            for (var i = 0; i < padding; i++)
            {
                context.GetCell(x + i, 0)?.SetSymbol(' ').SetStyle(LabelStyle);
            }

            x += padding;
            context.SetString(x, 0, label, LabelStyle);
        }
    }
}

[PublicAPI]
public static class ProgressBarWidgetExtensions
{
    extension(ProgressBarWidget widget)
    {
        public ProgressBarWidget Value(double value)
        {
            widget.Value = value;
            return widget;
        }

        public ProgressBarWidget Max(double max)
        {
            widget.Max = max;
            return widget;
        }

        public ProgressBarWidget Fill(ProgressBarFill fill)
        {
            widget.Fill = fill ?? throw new ArgumentNullException(nameof(fill));
            return widget;
        }

        public ProgressBarWidget Smooth()
        {
            return widget.Fill(ProgressBarFill.Smooth);
        }

        public ProgressBarWidget Symbols(char foreground, char background)
        {
            return widget.Fill(ProgressBarFill.Custom(foreground, background));
        }

        public ProgressBarWidget Foreground(ProgressBarBrush? brush)
        {
            widget.Foreground = brush;
            return widget;
        }

        public ProgressBarWidget Background(ProgressBarBrush? brush)
        {
            widget.Background = brush;
            return widget;
        }

        public ProgressBarWidget ShowLabel(bool show = true)
        {
            widget.ShowLabel = show;
            return widget;
        }

        public ProgressBarWidget HideLabel()
        {
            widget.ShowLabel = false;
            return widget;
        }

        public ProgressBarWidget Label(ProgressBarLabel label)
        {
            widget.Label = label ?? throw new ArgumentNullException(nameof(label));
            widget.ShowLabel = true;
            return widget;
        }

        public ProgressBarWidget LabelStyle(Style? style)
        {
            widget.LabelStyle = style;
            return widget;
        }

        public ProgressBarWidget Percentage()
        {
            return widget.Label(ProgressBarLabel.Percentage);
        }

        public ProgressBarWidget Fraction()
        {
            return widget.Label(ProgressBarLabel.Fraction);
        }
    }
}
