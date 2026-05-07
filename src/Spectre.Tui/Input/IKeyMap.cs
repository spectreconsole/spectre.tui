namespace Spectre.Tui.App;

[PublicAPI]
public interface IKeyMap
{
    IEnumerable<KeyBinding> Help();
}