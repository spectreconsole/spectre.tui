namespace Sandbox;

public sealed class TabsWidget : JustInTimeWidget
{
    private readonly TabsWidget<Item> _widget;

    public int SelectedIndex
    {
        get => _widget.SelectedIndex;
        set
        {
            _widget.SelectedIndex = value;
            MarkAsDirty();
        }
    }

    private class Item(string markup) : ITabWidgetItem
    {
        public TextLine CreateTextLine(bool isSelected)
        {
            return  TextLine.FromMarkup(markup);
        }
    }

    public TabsWidget(List<string> titles)
    {
        _widget = new TabsWidget<Item>()
            .WrapAround()
            .Items(titles.Select(title => new Item(title)))
            .SelectedIndex(0);
    }

    public void MoveNext()
    {
        _widget.MoveRight();
        MarkAsDirty();
    }

    protected override void RenderDirty(RenderContext context)
    {
        context.Render(_widget);
    }
}
