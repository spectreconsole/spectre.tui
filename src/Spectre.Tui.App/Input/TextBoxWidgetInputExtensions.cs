namespace Spectre.Tui.App;

[PublicAPI]
public static class TextBoxWidgetInputExtensions
{
    extension(TextBoxWidget widget)
    {
        public bool HandleKey(KeyMessage message)
        {
            var info = message.Info;
            var ctrl = (info.Modifiers & ConsoleModifiers.Control) != 0;

            switch (info.Key)
            {
                case ConsoleKey.LeftArrow:
                    widget.MoveLeft();
                    return true;
                case ConsoleKey.RightArrow:
                    widget.MoveRight();
                    return true;
                case ConsoleKey.UpArrow:
                    if (widget.Mode != TextBoxMode.MultiLine)
                    {
                        return false;
                    }
                    widget.MoveUp();
                    return true;
                case ConsoleKey.DownArrow:
                    if (widget.Mode != TextBoxMode.MultiLine)
                    {
                        return false;
                    }
                    widget.MoveDown();
                    return true;
                case ConsoleKey.Home:
                    if (ctrl)
                    {
                        widget.MoveToStart();
                    }
                    else
                    {
                        widget.MoveHome();
                    }
                    return true;
                case ConsoleKey.End:
                    if (ctrl)
                    {
                        widget.MoveToEnd();
                    }
                    else
                    {
                        widget.MoveEnd();
                    }
                    return true;
                case ConsoleKey.Backspace:
                    widget.DeleteBackward();
                    return true;
                case ConsoleKey.Delete:
                    widget.DeleteForward();
                    return true;
                case ConsoleKey.Enter:
                    if (widget.Mode != TextBoxMode.MultiLine)
                    {
                        return false;
                    }
                    widget.InsertNewLine();
                    return true;
                case ConsoleKey.Tab:
                case ConsoleKey.Escape:
                    return false;
            }

            var ch = info.KeyChar;
            if (ch == '\0' || char.IsControl(ch))
            {
                return false;
            }

            widget.Insert(ch.ToString());
            return true;
        }
    }
}
