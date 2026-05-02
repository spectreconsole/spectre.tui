namespace Spectre.Tui;

public interface IListWidgetItem
{
    Text CreateText(bool isSelected);
}