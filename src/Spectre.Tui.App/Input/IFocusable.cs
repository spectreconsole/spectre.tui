namespace Spectre.Tui.App;

[PublicAPI]
public interface IFocusable
{
    bool IsFocused { get; set; }
}
