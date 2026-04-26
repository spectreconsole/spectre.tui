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