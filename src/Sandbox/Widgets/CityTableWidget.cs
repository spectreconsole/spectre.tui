using System.Globalization;

namespace Sandbox;

public sealed class City(int rank, string name, string country, long population)
    : TableRow, ITableColumnDefinition
{
    public int Rank { get; } = rank;
    public string Name { get; } = name;
    public string Country { get; } = country;
    public long Population { get; } = population;

    public static IEnumerable<TableColumn> GetColumns()
    {
        return
        [
            new TableColumn("Rank"),
            new TableColumn("City").StarWidth(2),
            new TableColumn("Country").StarWidth(1),
            new TableColumn("Population").RightAligned(),
        ];
    }

    protected override Text[] CreateCells(bool isSelected)
    {
        return
        [
            Text.FromMarkup("[italic]" + Rank.ToString(CultureInfo.InvariantCulture) + "[/]"),
            Text.FromString(Name),
            Text.FromMarkup(Country),
            Text.FromString(Population.ToString("N0", CultureInfo.InvariantCulture)),
        ];
    }
}

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