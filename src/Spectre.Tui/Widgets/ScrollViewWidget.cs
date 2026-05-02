namespace Spectre.Tui;

[PublicAPI]
public sealed class ScrollViewWidget : IWidget
{
    private readonly RenderSurface _surface = new();
    private readonly ScrollbarWidget _verticalBar = new();
    private readonly ScrollbarWidget _horizontalBar = new ScrollbarWidget().HorizontalBottom();
    private int _lastPageHeight;

    public IWidget? Inner { get; set; }

    public int VerticalOffset { get; set; }
    public int HorizontalOffset { get; set; }

    public ScrollMode VerticalScroll { get; set; } = ScrollMode.Auto;
    public ScrollMode HorizontalScroll { get; set; } = ScrollMode.Auto;

    public Size? ContentSize { get; set; }

    public Style? ScrollbarStyle { get; set; }
    public Style? ScrollbarThumbStyle { get; set; }

    public void ScrollUp(int by = 1)
    {
        VerticalOffset = Math.Max(0, VerticalOffset - Math.Max(0, by));
    }

    public void ScrollDown(int by = 1)
    {
        VerticalOffset += Math.Max(0, by);
    }

    public void ScrollLeft(int by = 1)
    {
        HorizontalOffset = Math.Max(0, HorizontalOffset - Math.Max(0, by));
    }

    public void ScrollRight(int by = 1)
    {
        HorizontalOffset += Math.Max(0, by);
    }

    public void PageUp()
    {
        ScrollUp(Math.Max(1, _lastPageHeight));
    }

    public void PageDown()
    {
        ScrollDown(Math.Max(1, _lastPageHeight));
    }

    public void ScrollToTop()
    {
        VerticalOffset = 0;
    }

    public void ScrollToBottom()
    {
        VerticalOffset = int.MaxValue;
    }

    public void ScrollToStart()
    {
        HorizontalOffset = 0;
    }

    public void ScrollToEnd()
    {
        HorizontalOffset = int.MaxValue;
    }

    void IWidget.Render(RenderContext context)
    {
        var viewport = context.Viewport;
        var inner = Inner;

        if (viewport.IsEmpty || inner is null)
        {
            return;
        }

        var reservedVertical = false;
        var reservedHorizontal = false;
        var contentWidth = viewport.Width;
        var contentHeight = viewport.Height;
        var surfaceWidth = 0;
        var surfaceHeight = 0;

        // A scrollbar appearing on one axis can flip the other axis from "fits" to
        // "overflows". Iterate until reservations stabilize (max 3 passes)
        for (var pass = 0; pass < 3; pass++)
        {
            contentWidth = viewport.Width - (reservedVertical ? 1 : 0);
            contentHeight = viewport.Height - (reservedHorizontal ? 1 : 0);

            if (contentWidth <= 0 || contentHeight <= 0)
            {
                // Viewport collapsed under reservations.
                // Drop the horizontal bar first (vertical wins by default)
                if (reservedHorizontal)
                {
                    reservedHorizontal = false;
                    continue;
                }

                if (reservedVertical)
                {
                    reservedVertical = false;
                    continue;
                }

                return;
            }

            var renderWidth = ContentSize?.Width ??
                              (HorizontalScroll == ScrollMode.Disabled ? contentWidth : short.MaxValue);
            var renderHeight = ContentSize?.Height ??
                               (VerticalScroll == ScrollMode.Disabled ? contentHeight : short.MaxValue);

            _surface.Render(inner.Render, new Size(renderWidth, renderHeight));

            surfaceWidth = ContentSize?.Width ?? _surface.Width;
            surfaceHeight = ContentSize?.Height ?? _surface.Height;

            var nextReservedVertical = ResolveShow(VerticalScroll, surfaceHeight > contentHeight);
            var nextReservedHorizontal = ResolveShow(HorizontalScroll, surfaceWidth > contentWidth);

            if (nextReservedVertical == reservedVertical && nextReservedHorizontal == reservedHorizontal)
            {
                break;
            }

            reservedVertical = nextReservedVertical;
            reservedHorizontal = nextReservedHorizontal;
        }

        VerticalOffset = VerticalScroll == ScrollMode.Disabled
            ? 0
            : Math.Clamp(VerticalOffset, 0, Math.Max(0, surfaceHeight - contentHeight));
        HorizontalOffset = HorizontalScroll == ScrollMode.Disabled
            ? 0
            : Math.Clamp(HorizontalOffset, 0, Math.Max(0, surfaceWidth - contentWidth));

        _lastPageHeight = contentHeight;

        if (contentWidth > 0 && contentHeight > 0)
        {
            context.Blit(
                0, 0,
                _surface,
                new Rectangle(HorizontalOffset, VerticalOffset, contentWidth, contentHeight));
        }

        if (reservedVertical)
        {
            _verticalBar.Length = surfaceHeight;
            _verticalBar.ViewportLength = contentHeight;
            _verticalBar.Position = VerticalOffset;
            _verticalBar.Style = ScrollbarStyle;
            _verticalBar.ThumbStyle = ScrollbarThumbStyle;

            // Truncate by 1 when both bars share an edge so the corner cell stays empty
            context.Render(_verticalBar, new Rectangle(viewport.Width - 1, 0, 1, contentHeight));
        }

        if (reservedHorizontal)
        {
            _horizontalBar.Length = surfaceWidth;
            _horizontalBar.ViewportLength = contentWidth;
            _horizontalBar.Position = HorizontalOffset;
            _horizontalBar.Style = ScrollbarStyle;
            _horizontalBar.ThumbStyle = ScrollbarThumbStyle;

            context.Render(_horizontalBar, new Rectangle(0, viewport.Height - 1, contentWidth, 1));
        }
    }

    private static bool ResolveShow(ScrollMode mode, bool overflowing)
    {
        return mode switch
        {
            ScrollMode.Auto => overflowing,
            ScrollMode.Always => true,
            ScrollMode.Hidden => false,
            ScrollMode.Disabled => false,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}

[PublicAPI]
public enum ScrollMode
{
    Auto,
    Always,
    Hidden,
    Disabled,
}

[PublicAPI]
public static class ScrollViewWidgetExtensions
{
    extension(ScrollViewWidget widget)
    {
        public ScrollViewWidget Inner(IWidget inner)
        {
            widget.Inner = inner;
            return widget;
        }

        public ScrollViewWidget VerticalOffset(int offset)
        {
            widget.VerticalOffset = offset;
            return widget;
        }

        public ScrollViewWidget HorizontalOffset(int offset)
        {
            widget.HorizontalOffset = offset;
            return widget;
        }

        public ScrollViewWidget VerticalScroll(ScrollMode mode)
        {
            widget.VerticalScroll = mode;
            return widget;
        }

        public ScrollViewWidget HorizontalScroll(ScrollMode mode)
        {
            widget.HorizontalScroll = mode;
            return widget;
        }

        public ScrollViewWidget ContentSize(Size size)
        {
            widget.ContentSize = size;
            return widget;
        }

        public ScrollViewWidget ContentSize(int width, int height)
        {
            widget.ContentSize = new Size(width, height);
            return widget;
        }

        public ScrollViewWidget ScrollbarStyle(Style? style)
        {
            widget.ScrollbarStyle = style;
            return widget;
        }

        public ScrollViewWidget ScrollbarThumbStyle(Style? style)
        {
            widget.ScrollbarThumbStyle = style;
            return widget;
        }
    }
}