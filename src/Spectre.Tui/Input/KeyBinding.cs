namespace Spectre.Tui.App;

[PublicAPI]
public record KeyBinding
{
    public required IReadOnlyList<KeyPress> Keys { get; init; }
    public bool Enabled { get; init; } = true;
    public string Help { get; init; } = string.Empty;
    public int Order { get; init; }

    public bool Matches(IKeyInfo key)
    {
        if (!Enabled)
        {
            return false;
        }

        foreach (var binding in Keys)
        {
            if (binding.Modifiers != key.Modifiers)
            {
                continue;
            }

            var hasKey = binding.Key != Key.None;
            var hasCharacter = binding.Character.HasValue;

            if (!hasKey && !hasCharacter)
            {
                continue;
            }

            if (hasKey && binding.Key != key.Key)
            {
                continue;
            }

            if (hasCharacter && binding.Character != key.Character)
            {
                continue;
            }

            return true;
        }

        return false;
    }

    public static KeyBinding For(params List<KeyPress> keys)
    {
        return new KeyBinding
        {
            Keys = keys,
        };
    }

    public static KeyBinding For(params List<Key> keys)
    {
        return new KeyBinding
        {
            Keys = [.. keys.Select(KeyPress.For)],
        };
    }

    public static KeyBinding For(params List<char> keys)
    {
        return new KeyBinding
        {
            Keys = [.. keys.Select(KeyPress.For)],
        };
    }

    public static KeyBinding Combine(params List<KeyBinding> bindings)
    {
        return new KeyBinding
        {
            Keys = [.. bindings.SelectMany(b => b.Keys)],
            Enabled = bindings.All(b => b.Enabled),
        };
    }
}

[PublicAPI]
public static class KeyBindingExtensions
{
    extension(KeyBinding binding)
    {
        public KeyBinding WithHelp(string help)
        {
            return binding with
            {
                Help = help,
            };
        }

        public KeyBinding Enabled(bool enabled = true)
        {
            return binding with
            {
                Enabled = enabled,
            };
        }

        public KeyBinding Disabled()
        {
            return binding.Enabled(false);
        }

        public KeyBinding Order(int order)
        {
            return binding with
            {
                Order = order,
            };
        }
    }
}