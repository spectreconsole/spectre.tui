namespace Spectre.Tui;

[PublicAPI]
public sealed class ListWidget<TItem> : IWidget
    where TItem : IListWidgetItem
{
    private int _offset;
    private int? _selectedIndex;

    public List<TItem> Items { get; }
    public TextLine? HighlightSymbol { get; set; }
    public Style? HighlightStyle { get; set; }
    public bool WrapAround { get; set; }

    public TItem? SelectedItem => GetSelectedItem();
    public int? SelectedIndex
    {
        get => _selectedIndex;
        set => SetSelectedIndex(value);
    }

    public void MoveUp()
    {
        SetSelectedIndex(--_selectedIndex);
    }

    public void MoveDown()
    {
        SetSelectedIndex(++_selectedIndex);
    }

    public void MoveToStart()
    {
        SetSelectedIndex(0);
    }

    public void MoveToEnd()
    {
        SetSelectedIndex(Items.Count - 1);
    }

    public ListWidget(params List<TItem> items)
    {
        Items = items ?? throw new ArgumentNullException(nameof(items));
    }

    public void Render(RenderContext context)
    {
        var viewport = context.Viewport;

        // Ensure the selected index is within range
        SetSelectedIndex(_selectedIndex);

        // Create an array with the text representation of all items
        var items = new Text[Items.Count];
        for (var index = 0; index < Items.Count; index++)
        {
            items[index] = Items[index].CreateText(index == _selectedIndex);
        }

        // Get the visible bounds and update the offset
        var (firstVisibleIndex, lastVisibleIndex) = GetVisibleBounds(context, items);
        _offset = firstVisibleIndex;

        // Symbols
        var highlightSymbol = HighlightSymbol ?? null;
        var highlightSymbolWidth = highlightSymbol?.GetWidth() ?? 0;

        // Iterate the items (skip / take)
        var currentHeight = 0;
        for (var index = _offset; index < _offset + lastVisibleIndex - firstVisibleIndex; index++)
        {
            var item = items[index];

            var (x, y) = (viewport.Left, viewport.Top + currentHeight);
            var itemHeight = item.GetHeight();

            currentHeight += itemHeight;

            // Calculate the row area
            var rowArea = new Rectangle(x, y, viewport.Width, item.GetHeight());

            // Calculate the item area
            var itemArea = highlightSymbol != null
                ? new Rectangle(
                    x: rowArea.X + highlightSymbolWidth,
                    y: rowArea.Y,
                    width: (rowArea.Width - highlightSymbolWidth).EnsurePositive(),
                    height: rowArea.Height)
                : rowArea;

            // Render the row
            context.Render(item, itemArea);

            // Current index selected?
            var isSelected = _selectedIndex == index;
            if (isSelected && HighlightStyle != null)
            {
                // Update the row style
                context.SetStyle(itemArea, HighlightStyle);
            }

            // Render chevron?
            if (highlightSymbol != null && isSelected)
            {
                context.SetLine(x, y, highlightSymbol, highlightSymbolWidth);
            }
        }
    }

    private TItem? GetSelectedItem()
    {
        if (_selectedIndex == null || Items.Count == 0)
        {
            return default;
        }

        if (_selectedIndex < 0 || _selectedIndex > Items.Count - 1)
        {
            return default;
        }

        return Items[_selectedIndex.Value];
    }

    private void SetSelectedIndex(int? index)
    {
        if (index == null || Items.Count == 0)
        {
            _selectedIndex = null;
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

        _selectedIndex = Math.Clamp(index.Value, 0, Math.Max(0, Items.Count - 1));
    }

    // Algorithm ported from Ratatui.
    private (int FirstVisible, int LastVisible) GetVisibleBounds(
        RenderContext context,
        ReadOnlySpan<Text> items)
    {
        var selectedIndex = _selectedIndex;
        var offset = Math.Min(_offset, (items.Length - 1).EnsurePositive());
        var maxHeight = context.Viewport.Height;

        var firstVisibleIndex = offset;
        var lastVisibleIndex = offset;

        var heightFromOffset = 0;

        for (var i = offset; i < items.Length; i++)
        {
            var itemHeight = items[i].GetHeight();
            if (heightFromOffset + itemHeight > maxHeight)
            {
                break;
            }

            heightFromOffset += itemHeight;
            lastVisibleIndex++;
        }

        var indexToDisplay = Math.Min(selectedIndex ?? 0, Math.Max(0, items.Length - 1));
        while (indexToDisplay >= lastVisibleIndex)
        {
            heightFromOffset += items[lastVisibleIndex].GetHeight();
            lastVisibleIndex++;

            while (heightFromOffset > maxHeight)
            {
                heightFromOffset = (heightFromOffset - items[firstVisibleIndex].GetHeight()).EnsurePositive();
                firstVisibleIndex++;
            }
        }

        while (indexToDisplay < firstVisibleIndex)
        {
            firstVisibleIndex--;
            heightFromOffset += items[firstVisibleIndex].GetHeight();

            while (heightFromOffset > maxHeight)
            {
                lastVisibleIndex--;
                heightFromOffset = (heightFromOffset - items[lastVisibleIndex].GetHeight()).EnsurePositive();
            }
        }

        return (firstVisibleIndex, lastVisibleIndex);
    }
}

public interface IListWidgetItem
{
    Text CreateText(bool isSelected);
}

public abstract class ListWidgetItem : IListWidgetItem
{
    Text IListWidgetItem.CreateText(bool isSelected)
    {
        return CreateText(isSelected);
    }

    protected abstract Text CreateText(bool isSelected);
}

public static class ListWidgetExtensions
{
    extension<TItem>(ListWidget<TItem> widget)
        where TItem : IListWidgetItem
    {
        public ListWidget<TItem> WithItems(params IEnumerable<TItem> items)
        {
            widget.Items.Clear();
            widget.Items.AddRange(items);
            return widget;
        }

        public ListWidget<TItem> HighlightStyle(Style? style)
        {
            widget.HighlightStyle = style;
            return widget;
        }

        public ListWidget<TItem> HighlightSymbol(TextLine? symbol)
        {
            widget.HighlightSymbol = symbol;
            return widget;
        }

        public ListWidget<TItem> WrapAround(bool enable = true)
        {
            widget.WrapAround = enable;
            return widget;
        }

        public ListWidget<TItem> SelectedIndex(int? index)
        {
            widget.SelectedIndex = index;
            return widget;
        }
    }
}