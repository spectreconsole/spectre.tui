namespace Spectre.Tui;

[PublicAPI]
public interface IFocusable
{
    bool IsFocused { get; set; }
}