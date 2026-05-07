namespace Sandbox;

public sealed class MainScreenKeyMap : IKeyMap
{
    public KeyBinding Quit { get; set; } = KeyBinding.For('q').WithHelp("Quit");
    public KeyBinding Popup { get; set; } = KeyBinding.For('b').WithHelp("Popup");

    public IEnumerable<KeyBinding> Help()
    {
        yield return Quit;
        yield return Popup;
    }
}
