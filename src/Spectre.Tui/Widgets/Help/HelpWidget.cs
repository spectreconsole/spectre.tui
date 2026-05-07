using Spectre.Tui.App;

namespace Spectre.Tui;

[PublicAPI]
public sealed class HelpWidget : IWidget
{
    private readonly IKeyMap[] _keyMaps;

    public Style? Style { get; set; } = new Style(Color.Grey);
    public Justify Alignment { get; set; } = Justify.Center;
    public string Separator { get; set; } = "  ";

    public HelpWidget(params IKeyMap[] keyMaps)
    {
        _keyMaps = keyMaps ?? throw new ArgumentNullException(nameof(keyMaps));
    }

    void IWidget.Render(RenderContext context)
    {
        var entries = new List<(int Order, string Segment)>();
        foreach (var keyMap in _keyMaps)
        {
            foreach (var binding in keyMap.Help())
            {
                var segment = FormatBinding(binding);
                if (!string.IsNullOrEmpty(segment))
                {
                    entries.Add((binding.Order, segment));
                }
            }
        }

        if (entries.Count == 0)
        {
            return;
        }

        var markup = string.Join(Separator, entries
            .OrderBy(e => e.Order)
            .Select(e => e.Segment));

        var paragraph = Paragraph.FromMarkup(markup, Style)
            .Alignment(Alignment)
            .Ellipsis();

        context.Render(paragraph, context.Viewport);
    }

    private static string FormatBinding(KeyBinding binding)
    {
        if (!binding.Enabled || binding.Keys.Count == 0)
        {
            return string.Empty;
        }

        var keys = new List<string>();
        foreach (var press in binding.Keys)
        {
            var name = FormatKey(press);
            if (!string.IsNullOrEmpty(name))
            {
                keys.Add(name);
            }
        }

        if (keys.Count == 0)
        {
            return string.Empty;
        }

        var keyMarkup = $"[bold][[{string.Join("/", keys)}]][/]";
        return string.IsNullOrEmpty(binding.Help)
            ? keyMarkup
            : $"{keyMarkup}:{binding.Help}";
    }

    private static string FormatKey(KeyPress press)
    {
        var name = FormatKeyName(press);
        if (string.IsNullOrEmpty(name))
        {
            return string.Empty;
        }

        if (press.Modifiers == KeyModifier.None)
        {
            return name;
        }

        var sb = new StringBuilder();
        if ((press.Modifiers & KeyModifier.Ctrl) != 0)
        {
            sb.Append("Ctrl+");
        }

        if ((press.Modifiers & KeyModifier.Alt) != 0)
        {
            sb.Append("Alt+");
        }

        if ((press.Modifiers & KeyModifier.Shift) != 0)
        {
            sb.Append("Shift+");
        }

        sb.Append(name);
        return sb.ToString();
    }

    private static string FormatKeyName(KeyPress press)
    {
        if (press.Key != Key.None)
        {
            return press.Key switch
            {
                Key.Up => "↑",
                Key.Down => "↓",
                Key.Left => "←",
                Key.Right => "→",
                Key.Enter => "Enter",
                Key.Escape => "Esc",
                Key.Tab => "Tab",
                Key.Space => "Space",
                Key.Backspace => "Backspace",
                Key.Delete => "Del",
                Key.Insert => "Ins",
                Key.Home => "Home",
                Key.End => "End",
                Key.PageUp => "PgUp",
                Key.PageDown => "PgDn",
                _ => press.Key.ToString(),
            };
        }

        if (press.Character.HasValue)
        {
            return char.ToUpperInvariant(press.Character.Value).ToString();
        }

        return string.Empty;
    }
}

[PublicAPI]
public static class HelpWidgetExtensions
{
    extension(HelpWidget widget)
    {
        public HelpWidget Style(Style? style)
        {
            widget.Style = style;
            return widget;
        }

        public HelpWidget Alignment(Justify alignment)
        {
            widget.Alignment = alignment;
            return widget;
        }

        public HelpWidget LeftAligned()
        {
            return widget.Alignment(Justify.Left);
        }

        public HelpWidget Centered()
        {
            return widget.Alignment(Justify.Center);
        }

        public HelpWidget RightAligned()
        {
            return widget.Alignment(Justify.Right);
        }

        public HelpWidget Separator(string separator)
        {
            widget.Separator = separator;
            return widget;
        }
    }
}
