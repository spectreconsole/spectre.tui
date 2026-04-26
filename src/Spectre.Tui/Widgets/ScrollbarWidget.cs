namespace Spectre.Tui;

[PublicAPI]
public sealed class ScrollbarWidget : IWidget
{
    public int Length { get; set; }
    public int ViewportLength { get; set; }
    public int Position { get; set; }
    public Style? Style { get; set; }
    public Style? ThumbStyle { get; set; }

    public ScrollOrientation Orientation { get; set; } = ScrollOrientation.VerticalRight;
    public char? BeginSymbol { get; set; } = Scrollbar.DoubleVertical.Begin;
    public char? EndSymbol { get; set; } = Scrollbar.DoubleVertical.End;
    public char TrackSymbol { get; set; } = Scrollbar.DoubleVertical.Track;
    public char ThumbSymbol { get; set; } = Scrollbar.DoubleVertical.Thumb;

    public void Render(RenderContext context)
    {
        var area = context.Viewport;

        var isVertical = Orientation is ScrollOrientation.VerticalLeft or ScrollOrientation.VerticalRight;
        var size = isVertical ? area.Height : area.Width;
        if (size <= 0)
        {
            return;
        }

        var crossAxis = Orientation switch
        {
            ScrollOrientation.VerticalLeft => 0,
            ScrollOrientation.VerticalRight => area.Width - 1,
            ScrollOrientation.HorizontalTop => 0,
            ScrollOrientation.HorizontalBottom => area.Height - 1,
            _ => 0,
        };

        if (size == 1)
        {
            Paint(0, ThumbSymbol, isThumb: true);
            return;
        }

        if (BeginSymbol is { } beginning)
        {
            Paint(0, beginning, isThumb: false);
        }

        if (EndSymbol is { } end)
        {
            Paint(size - 1, end, isThumb: false);
        }

        var trackStart = BeginSymbol is null ? 0 : 1;
        var trackEnd = EndSymbol is null ? size : size - 1;
        var track = trackEnd - trackStart;
        if (track <= 0)
        {
            return;
        }

        for (var i = trackStart; i < trackEnd; i++)
        {
            Paint(i, TrackSymbol, isThumb: false);
        }

        var total = Math.Max(Length, 1);
        var viewport = Math.Clamp(ViewportLength <= 0 ? 1 : ViewportLength, 1, total);
        var thumbSize = (int)Math.Max(1L, ((long)viewport * track + total - 1) / total);

        thumbSize = Math.Min(thumbSize, track);

        var maxScroll = Math.Max(total - viewport, 0);
        var pos = Math.Clamp(Position, 0, maxScroll);
        var thumbStart = maxScroll == 0 ? 0 : (int)((long)pos * (track - thumbSize) / maxScroll);

        for (var i = 0; i < thumbSize; i++)
        {
            Paint(trackStart + thumbStart + i, ThumbSymbol, isThumb: true);
        }

        return;

        void Paint(int index, char glyph, bool isThumb)
        {
            var (x, y) = isVertical ? (crossAxis, index) : (index, crossAxis);
            var cell = context.GetCell(x, y);
            if (cell is null)
            {
                return;
            }

            cell.SetSymbol(glyph);
            var style = (isThumb ? ThumbStyle : null) ?? Style;
            if (style is not null)
            {
                cell.SetStyle(style);
            }
        }
    }
}

[PublicAPI]
public enum ScrollOrientation
{
    VerticalLeft,
    VerticalRight,
    HorizontalTop,
    HorizontalBottom,
}

[PublicAPI]
public static class ScrollbarWidgetExtensions
{
    extension(ScrollbarWidget widget)
    {
        public ScrollbarWidget Length(int length)
        {
            widget.Length = length;
            return widget;
        }

        public ScrollbarWidget ViewportLength(int viewportLength)
        {
            widget.ViewportLength = viewportLength;
            return widget;
        }

        public ScrollbarWidget Position(int position)
        {
            widget.Position = position;
            return widget;
        }

        public ScrollbarWidget Style(Style? style)
        {
            widget.Style = style;
            return widget;
        }

        public ScrollbarWidget ThumbStyle(Style? style)
        {
            widget.ThumbStyle = style;
            return widget;
        }

        public ScrollbarWidget BeginSymbol(char? symbol)
        {
            widget.BeginSymbol = symbol;
            return widget;
        }

        public ScrollbarWidget EndSymbol(char? symbol)
        {
            widget.EndSymbol = symbol;
            return widget;
        }

        public ScrollbarWidget TrackSymbol(char symbol)
        {
            widget.TrackSymbol = symbol;
            return widget;
        }

        public ScrollbarWidget ThumbSymbol(char symbol)
        {
            widget.ThumbSymbol = symbol;
            return widget;
        }

        public ScrollbarWidget Orientation(ScrollOrientation orientation)
        {
            widget.Orientation = orientation;
            return widget.FromSymbols(orientation switch
            {
                ScrollOrientation.VerticalLeft or ScrollOrientation.VerticalRight => Scrollbar.DoubleVertical,
                ScrollOrientation.HorizontalTop or ScrollOrientation.HorizontalBottom => Scrollbar.DoubleHorizontal,
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            });
        }

        public ScrollbarWidget VerticalLeft()
        {
            return widget.Orientation(ScrollOrientation.VerticalLeft);
        }

        public ScrollbarWidget VerticalRight()
        {
            return widget.Orientation(ScrollOrientation.VerticalRight);
        }

        public ScrollbarWidget HorizontalTop()
        {
            return widget.Orientation(ScrollOrientation.HorizontalTop);
        }

        public ScrollbarWidget HorizontalBottom()
        {
            return widget.Orientation(ScrollOrientation.HorizontalBottom);
        }

        public ScrollbarWidget FromSymbols(Scrollbar scrollbar)
        {
            return widget
                .BeginSymbol(scrollbar.Begin)
                .EndSymbol(scrollbar.End)
                .TrackSymbol(scrollbar.Track)
                .ThumbSymbol(scrollbar.Thumb);
        }
    }
}