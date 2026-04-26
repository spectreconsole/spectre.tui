namespace Sandbox;

public sealed class CityTableWidget : JustInTimeWidget
{
    private readonly TableWidget<City> _table;

    public int Position => _table.SelectedIndex ?? 0;
    public int Length => _table.Rows.Count;

    public CityTableWidget(IEnumerable<City> cities)
    {
        _table = new TableWidget<City>([.. cities])
            .AutoAddColumns()
            .HighlightStyle(new Style(decoration: Decoration.Invert))
            .HeaderStyle(new Style(Color.Green, decoration: Decoration.Bold))
            .WrapAround()
            .SelectedIndex(0);
    }

    public void MoveUp()
    {
        _table.MoveUp();
        MarkAsDirty();
    }

    public void MoveDown()
    {
        _table.MoveDown();
        MarkAsDirty();
    }

    protected override void RenderDirty(RenderContext context)
    {
        context.Render(_table);
    }
}