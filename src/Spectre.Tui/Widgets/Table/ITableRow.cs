namespace Spectre.Tui;

[PublicAPI]
public interface ITableRow
{
    Text[] CreateCells(bool isSelected);
}

[PublicAPI]
public abstract class TableRow : ITableRow
{
    Text[] ITableRow.CreateCells(bool isSelected)
    {
        return CreateCells(isSelected);
    }

    protected abstract Text[] CreateCells(bool isSelected);
}
