namespace Sandbox;

public sealed class MyTabsWidget : JustInTimeWidget
{
    private readonly TabsWidget<MyTabsWidget.Item> _widget;

    public int SelectedIndex
    {
        get => _widget.SelectedIndex;
        set
        {
            _widget.SelectedIndex = value;
            MarkAsDirty();
        }
    }

    public MyTabsWidget(List<string> titles)
    {
        _widget = new TabsWidget<Item>()
            .WrapAround()
            .HighlightStyle(new Style(Color.Yellow, decoration: Decoration.Underline))
            .Items(titles.Select(title => new Item(title)))
            .SelectedIndex(0);
    }

    private class Item(string markup) : ITabWidgetItem
    {
        public TextLine CreateTextLine(bool isSelected)
        {
            return  TextLine.FromMarkup(markup);
        }
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