using Spectre.Tui.App;

namespace Spectre.Tui;

[PublicAPI]
public sealed class TextBoxKeyMap : IKeyMap
{
    private readonly TextBoxWidget _widget;

    public KeyBinding MoveUp { get; set; } = KeyBinding.For(Key.Up).WithHelp("Move up");
    public KeyBinding MoveDown { get; set; } = KeyBinding.For(Key.Down).WithHelp("Move down");
    public KeyBinding MoveBackward { get; set; } = KeyBinding.For(Key.Left).WithHelp("Move left");
    public KeyBinding MoveForward { get; set; } = KeyBinding.For(Key.Right).WithHelp("Move right");
    public KeyBinding MoveHome { get; set; } = KeyBinding.For(Key.Home).WithHelp("Start of line");
    public KeyBinding MoveEnd { get; set; } = KeyBinding.For(Key.End).WithHelp("End of line");
    public KeyBinding DeleteBackward { get; set; } = KeyBinding.For(Key.Backspace).WithHelp("Delete back");
    public KeyBinding DeleteForward { get; set; } = KeyBinding.For(Key.Delete).WithHelp("Delete forward");
    public KeyBinding InsertNewLine { get; set; } = KeyBinding.For(Key.Enter).WithHelp("New line");

    internal TextBoxKeyMap(TextBoxWidget widget)
    {
        _widget = widget ?? throw new ArgumentNullException(nameof(widget));
    }

    public void HandleKey(IKeyInfo message)
    {
        if (_widget.KeyMap.MoveBackward.Matches(message))
        {
            _widget.MoveLeft();
            return;
        }

        if (_widget.KeyMap.MoveForward.Matches(message))
        {
            _widget.MoveRight();
            return;
        }

        if (_widget.KeyMap.MoveUp.Matches(message))
        {
            _widget.MoveUp();
            return;
        }

        if (_widget.KeyMap.MoveDown.Matches(message))
        {
            _widget.MoveDown();
            return;
        }

        if (_widget.KeyMap.MoveHome.Matches(message))
        {
            _widget.MoveHome();
            return;
        }

        if (_widget.KeyMap.MoveEnd.Matches(message))
        {
            _widget.MoveEnd();
            return;
        }

        if (_widget.KeyMap.DeleteBackward.Matches(message))
        {
            _widget.DeleteBackward();
            return;
        }

        if (_widget.KeyMap.DeleteForward.Matches(message))
        {
            _widget.DeleteForward();
            return;
        }

        if (_widget.KeyMap.InsertNewLine.Matches(message))
        {
            _widget.InsertNewLine();
            return;
        }

        var character = message.Character;
        if (character == null || char.IsControl(character.Value))
        {
            return;
        }

        _widget.Insert(character.Value.ToString());
    }

    public IEnumerable<KeyBinding> Help()
    {
        yield return KeyBinding.Combine(MoveForward, MoveBackward, MoveUp, MoveDown).WithHelp("Move");
        yield return InsertNewLine;
    }
}