namespace Spectre.Tui;

[PublicAPI]
public interface ITableColumnDefinition
{
    static abstract IEnumerable<TableColumn> GetColumns();
}