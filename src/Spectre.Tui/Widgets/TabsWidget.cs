namespace Spectre.Tui;

[PublicAPI]
public class TabsWidget<T> : IWidget
    where T : ITabWidgetItem
{
    private int _selectedIndex;

    public List<T> Items { get; }
    public Style? HighlightStyle { get; set; }
    public bool WrapAround { get; set; }
    public TextSpan Separator { get; set; } = "|";
    public TextLine LeftPadding { get; set; } = " ";
    public TextLine RightPadding { get; set; } = " ";

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetSelectedIndex(value);
    }

    public T SelectedItem
    {
        get
        {
            if (Items.Count == 0)
            {
                throw new InvalidOperationException("Tabs has no items");
            }

            return Items[_selectedIndex];
        }
    }

    public TabsWidget()
        : this([])
    {
    }

    public TabsWidget(params List<T> items)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }

    public void MoveLeft()
    {
        SetSelectedIndex(_selectedIndex - 1);
    }

    public void MoveRight()
    {
        SetSelectedIndex(_selectedIndex + 1);
    }

    public void MoveToStart()
    {
        SetSelectedIndex(0);
    }

    public void MoveToEnd()
    {
        SetSelectedIndex(Items.Count - 1);
    }

    public void Render(RenderContext context)
    {
        if (Items.Count == 0)
        {
            return;
        }

        // Ensure the selected index is within range
        SetSelectedIndex(_selectedIndex);

        var area = context.Viewport;
        var x = area.Left;

        foreach (var (index, first, last, item) in Items.Enumerate())
        {
            // Left padding (skipped for the first item)
            if (!first && !TryWrite(LeftPadding))
            {
                Truncate();
                return;
            }

            // Title
            var titleStart = x;
            var isSelected = index == _selectedIndex;
            var titleFits = TryWrite(item.CreateTextLine(isSelected));

            // Apply the highlight to whatever was rendered so that a
            // truncated title (and any ellipsis overlaid on its last
            // cell) keeps the selection style.
            if (isSelected)
            {
                context.SetStyle(
                    new Rectangle(titleStart, area.Top, x - titleStart, 1),
                    HighlightStyle);
            }

            if (!titleFits)
            {
                Truncate();
                return;
            }

            // Right padding
            if (!TryWrite(RightPadding))
            {
                if (!last)
                {
                    Truncate();
                }

                return;
            }

            // Separator (skipped for the last item)
            if (last)
            {
                return;
            }

            if (!TrySpan(Separator))
            {
                Truncate();
                return;
            }
        }

        return;

        bool TryWrite(TextLine line)
        {
            var remaining = area.Right - x;
            if (remaining <= 0)
            {
                return false;
            }

            x = context.SetLine(x, area.Top, line, remaining).X;
            return area.Right - x > 0;
        }

        bool TrySpan(TextSpan span)
        {
            var remaining = area.Right - x;
            if (remaining <= 0)
            {
                return false;
            }

            x = context.SetSpan(x, area.Top, span, remaining).X;
            return area.Right - x > 0;
        }

        void Truncate()
        {
            if (area.Right - 1 >= area.Left)
            {
                context.SetSymbol(area.Right - 1, area.Top, '…');
            }
        }
    }

    private void SetSelectedIndex(int index)
    {
        if (Items.Count == 0)
        {
            _selectedIndex = 0;
            return;
        }

        if (WrapAround && Items.Count > 1)
        {
            if (index >= Items.Count)
            {
                _selectedIndex = 0;
                return;
            }

            if (index < 0)
            {
                _selectedIndex = Items.Count - 1;
                return;
            }
        }

        _selectedIndex = Math.Clamp(index, 0, Items.Count - 1);
    }
}

[PublicAPI]
public interface ITabWidgetItem
{
    TextLine CreateTextLine(bool isSelected);
}

[PublicAPI]
public static class TabsExtensions
{
    extension<T>(TabsWidget<T> widget)
        where T : ITabWidgetItem
    {
        public TabsWidget<T> Items(params IEnumerable<T> items)
        {
            widget.Items.Clear();
            widget.Items.AddRange(items);
            return widget;
        }

        public TabsWidget<T> HighlightStyle(Style? style)
        {
            widget.HighlightStyle = style;
            return widget;
        }

        public TabsWidget<T> WrapAround(bool enable = true)
        {
            widget.WrapAround = enable;
            return widget;
        }

        public TabsWidget<T> Separator(TextSpan separator)
        {
            widget.Separator = separator;
            return widget;
        }

        public TabsWidget<T> LeftPadding(TextLine padding)
        {
            widget.LeftPadding = padding;
            return widget;
        }

        public TabsWidget<T> RightPadding(TextLine padding)
        {
            widget.RightPadding = padding;
            return widget;
        }

        public TabsWidget<T> SelectedIndex(int index)
        {
            widget.SelectedIndex = index;
            return widget;
        }
    }
}