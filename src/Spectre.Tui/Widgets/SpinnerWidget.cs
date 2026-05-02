namespace Spectre.Tui;

public sealed class SpinnerWidget : IWidget
{
    private TimeSpan _accumulated = TimeSpan.Zero;
    private int _frame;

    public SpinnerKind Kind { get; set; } = SpinnerKind.Dots;

    public void Update(FrameInfo frame)
    {
        _accumulated += frame.FrameTime;

        if (_accumulated > Kind.Interval)
        {
            _frame = (_frame + 1) % Kind.Frames.Count;
            _accumulated = TimeSpan.Zero;
        }
    }

    void IWidget.Render(RenderContext context)
    {
        context.SetString(0, 0, Kind.Frames[_frame]);
    }
}

[PublicAPI]
public static class SpinnerWidgetExtensions
{
    extension(SpinnerWidget widget)
    {
        public SpinnerWidget Kind(SpinnerKind kind)
        {
            widget.Kind = kind;
            return widget;
        }
    }
}

public abstract class SpinnerKind
{
    public abstract TimeSpan Interval { get; }
    public abstract IReadOnlyList<string> Frames { get; }

    private sealed class DefaultSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⣷",
            "⣯",
            "⣟",
            "⡿",
            "⢿",
            "⣻",
            "⣽",
            "⣾",
        };
    }

    private sealed class AsciiSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "-",
            "\\",
            "|",
            "/",
            "-",
            "\\",
            "|",
            "/",
        };
    }

    private sealed class DotsSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠋",
            "⠙",
            "⠹",
            "⠸",
            "⠼",
            "⠴",
            "⠦",
            "⠧",
            "⠇",
            "⠏",
        };
    }

    private sealed class Dots2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⣾",
            "⣽",
            "⣻",
            "⢿",
            "⡿",
            "⣟",
            "⣯",
            "⣷",
        };
    }

    private sealed class Dots3Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠋",
            "⠙",
            "⠚",
            "⠞",
            "⠖",
            "⠦",
            "⠴",
            "⠲",
            "⠳",
            "⠓",
        };
    }

    private sealed class Dots4Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠄",
            "⠆",
            "⠇",
            "⠋",
            "⠙",
            "⠸",
            "⠰",
            "⠠",
            "⠰",
            "⠸",
            "⠙",
            "⠋",
            "⠇",
            "⠆",
        };
    }

    private sealed class Dots5Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠋",
            "⠙",
            "⠚",
            "⠒",
            "⠂",
            "⠂",
            "⠒",
            "⠲",
            "⠴",
            "⠦",
            "⠖",
            "⠒",
            "⠐",
            "⠐",
            "⠒",
            "⠓",
            "⠋",
        };
    }

    private sealed class Dots6Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠁",
            "⠉",
            "⠙",
            "⠚",
            "⠒",
            "⠂",
            "⠂",
            "⠒",
            "⠲",
            "⠴",
            "⠤",
            "⠄",
            "⠄",
            "⠤",
            "⠴",
            "⠲",
            "⠒",
            "⠂",
            "⠂",
            "⠒",
            "⠚",
            "⠙",
            "⠉",
            "⠁",
        };
    }

    private sealed class Dots7Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠈",
            "⠉",
            "⠋",
            "⠓",
            "⠒",
            "⠐",
            "⠐",
            "⠒",
            "⠖",
            "⠦",
            "⠤",
            "⠠",
            "⠠",
            "⠤",
            "⠦",
            "⠖",
            "⠒",
            "⠐",
            "⠐",
            "⠒",
            "⠓",
            "⠋",
            "⠉",
            "⠈",
        };
    }

    private sealed class Dots8Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠁",
            "⠁",
            "⠉",
            "⠙",
            "⠚",
            "⠒",
            "⠂",
            "⠂",
            "⠒",
            "⠲",
            "⠴",
            "⠤",
            "⠄",
            "⠄",
            "⠤",
            "⠠",
            "⠠",
            "⠤",
            "⠦",
            "⠖",
            "⠒",
            "⠐",
            "⠐",
            "⠒",
            "⠓",
            "⠋",
            "⠉",
            "⠈",
            "⠈",
        };
    }

    private sealed class Dots9Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⢹",
            "⢺",
            "⢼",
            "⣸",
            "⣇",
            "⡧",
            "⡗",
            "⡏",
        };
    }

    private sealed class Dots10Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⢄",
            "⢂",
            "⢁",
            "⡁",
            "⡈",
            "⡐",
            "⡠",
        };
    }

    private sealed class Dots11Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠁",
            "⠂",
            "⠄",
            "⡀",
            "⢀",
            "⠠",
            "⠐",
            "⠈",
        };
    }

    private sealed class Dots12Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⢀⠀",
            "⡀⠀",
            "⠄⠀",
            "⢂⠀",
            "⡂⠀",
            "⠅⠀",
            "⢃⠀",
            "⡃⠀",
            "⠍⠀",
            "⢋⠀",
            "⡋⠀",
            "⠍⠁",
            "⢋⠁",
            "⡋⠁",
            "⠍⠉",
            "⠋⠉",
            "⠋⠉",
            "⠉⠙",
            "⠉⠙",
            "⠉⠩",
            "⠈⢙",
            "⠈⡙",
            "⢈⠩",
            "⡀⢙",
            "⠄⡙",
            "⢂⠩",
            "⡂⢘",
            "⠅⡘",
            "⢃⠨",
            "⡃⢐",
            "⠍⡐",
            "⢋⠠",
            "⡋⢀",
            "⠍⡁",
            "⢋⠁",
            "⡋⠁",
            "⠍⠉",
            "⠋⠉",
            "⠋⠉",
            "⠉⠙",
            "⠉⠙",
            "⠉⠩",
            "⠈⢙",
            "⠈⡙",
            "⠈⠩",
            "⠀⢙",
            "⠀⡙",
            "⠀⠩",
            "⠀⢘",
            "⠀⡘",
            "⠀⠨",
            "⠀⢐",
            "⠀⡐",
            "⠀⠠",
            "⠀⢀",
            "⠀⡀",
        };
    }

    private sealed class Dots13Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⣼",
            "⣹",
            "⢻",
            "⠿",
            "⡟",
            "⣏",
            "⣧",
            "⣶",
        };
    }

    private sealed class Dots14Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠉⠉",
            "⠈⠙",
            "⠀⠹",
            "⠀⢸",
            "⠀⣰",
            "⢀⣠",
            "⣀⣀",
            "⣄⡀",
            "⣆⠀",
            "⡇⠀",
            "⠏⠀",
            "⠋⠁",
        };
    }

    private sealed class Dots8BitSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠀",
            "⠁",
            "⠂",
            "⠃",
            "⠄",
            "⠅",
            "⠆",
            "⠇",
            "⡀",
            "⡁",
            "⡂",
            "⡃",
            "⡄",
            "⡅",
            "⡆",
            "⡇",
            "⠈",
            "⠉",
            "⠊",
            "⠋",
            "⠌",
            "⠍",
            "⠎",
            "⠏",
            "⡈",
            "⡉",
            "⡊",
            "⡋",
            "⡌",
            "⡍",
            "⡎",
            "⡏",
            "⠐",
            "⠑",
            "⠒",
            "⠓",
            "⠔",
            "⠕",
            "⠖",
            "⠗",
            "⡐",
            "⡑",
            "⡒",
            "⡓",
            "⡔",
            "⡕",
            "⡖",
            "⡗",
            "⠘",
            "⠙",
            "⠚",
            "⠛",
            "⠜",
            "⠝",
            "⠞",
            "⠟",
            "⡘",
            "⡙",
            "⡚",
            "⡛",
            "⡜",
            "⡝",
            "⡞",
            "⡟",
            "⠠",
            "⠡",
            "⠢",
            "⠣",
            "⠤",
            "⠥",
            "⠦",
            "⠧",
            "⡠",
            "⡡",
            "⡢",
            "⡣",
            "⡤",
            "⡥",
            "⡦",
            "⡧",
            "⠨",
            "⠩",
            "⠪",
            "⠫",
            "⠬",
            "⠭",
            "⠮",
            "⠯",
            "⡨",
            "⡩",
            "⡪",
            "⡫",
            "⡬",
            "⡭",
            "⡮",
            "⡯",
            "⠰",
            "⠱",
            "⠲",
            "⠳",
            "⠴",
            "⠵",
            "⠶",
            "⠷",
            "⡰",
            "⡱",
            "⡲",
            "⡳",
            "⡴",
            "⡵",
            "⡶",
            "⡷",
            "⠸",
            "⠹",
            "⠺",
            "⠻",
            "⠼",
            "⠽",
            "⠾",
            "⠿",
            "⡸",
            "⡹",
            "⡺",
            "⡻",
            "⡼",
            "⡽",
            "⡾",
            "⡿",
            "⢀",
            "⢁",
            "⢂",
            "⢃",
            "⢄",
            "⢅",
            "⢆",
            "⢇",
            "⣀",
            "⣁",
            "⣂",
            "⣃",
            "⣄",
            "⣅",
            "⣆",
            "⣇",
            "⢈",
            "⢉",
            "⢊",
            "⢋",
            "⢌",
            "⢍",
            "⢎",
            "⢏",
            "⣈",
            "⣉",
            "⣊",
            "⣋",
            "⣌",
            "⣍",
            "⣎",
            "⣏",
            "⢐",
            "⢑",
            "⢒",
            "⢓",
            "⢔",
            "⢕",
            "⢖",
            "⢗",
            "⣐",
            "⣑",
            "⣒",
            "⣓",
            "⣔",
            "⣕",
            "⣖",
            "⣗",
            "⢘",
            "⢙",
            "⢚",
            "⢛",
            "⢜",
            "⢝",
            "⢞",
            "⢟",
            "⣘",
            "⣙",
            "⣚",
            "⣛",
            "⣜",
            "⣝",
            "⣞",
            "⣟",
            "⢠",
            "⢡",
            "⢢",
            "⢣",
            "⢤",
            "⢥",
            "⢦",
            "⢧",
            "⣠",
            "⣡",
            "⣢",
            "⣣",
            "⣤",
            "⣥",
            "⣦",
            "⣧",
            "⢨",
            "⢩",
            "⢪",
            "⢫",
            "⢬",
            "⢭",
            "⢮",
            "⢯",
            "⣨",
            "⣩",
            "⣪",
            "⣫",
            "⣬",
            "⣭",
            "⣮",
            "⣯",
            "⢰",
            "⢱",
            "⢲",
            "⢳",
            "⢴",
            "⢵",
            "⢶",
            "⢷",
            "⣰",
            "⣱",
            "⣲",
            "⣳",
            "⣴",
            "⣵",
            "⣶",
            "⣷",
            "⢸",
            "⢹",
            "⢺",
            "⢻",
            "⢼",
            "⢽",
            "⢾",
            "⢿",
            "⣸",
            "⣹",
            "⣺",
            "⣻",
            "⣼",
            "⣽",
            "⣾",
            "⣿",
        };
    }

    private sealed class DotsCircleSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⢎ ",
            "⠎⠁",
            "⠊⠑",
            "⠈⠱",
            " ⡱",
            "⢀⡰",
            "⢄⡠",
            "⢆⡀",
        };
    }

    private sealed class SandSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠁",
            "⠂",
            "⠄",
            "⡀",
            "⡈",
            "⡐",
            "⡠",
            "⣀",
            "⣁",
            "⣂",
            "⣄",
            "⣌",
            "⣔",
            "⣤",
            "⣥",
            "⣦",
            "⣮",
            "⣶",
            "⣷",
            "⣿",
            "⡿",
            "⠿",
            "⢟",
            "⠟",
            "⡛",
            "⠛",
            "⠫",
            "⢋",
            "⠋",
            "⠍",
            "⡉",
            "⠉",
            "⠑",
            "⠡",
            "⢁",
        };
    }

    private sealed class LineSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(130);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "-",
            "\\",
            "|",
            "/",
        };
    }

    private sealed class Line2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠂",
            "-",
            "–",
            "—",
            "–",
            "-",
        };
    }

    private sealed class PipeSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "┤",
            "┘",
            "┴",
            "└",
            "├",
            "┌",
            "┬",
            "┐",
        };
    }

    private sealed class SimpleDotsSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(400);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            ".  ",
            ".. ",
            "...",
            "   ",
        };
    }

    private sealed class SimpleDotsScrollingSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(200);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            ".  ",
            ".. ",
            "...",
            " ..",
            "  .",
            "   ",
        };
    }

    private sealed class StarSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(70);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "✶",
            "✸",
            "✹",
            "✺",
            "✹",
            "✷",
        };
    }

    private sealed class Star2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "+",
            "x",
            "*",
        };
    }

    private sealed class FlipSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(70);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "_",
            "_",
            "_",
            "-",
            "`",
            "`",
            "'",
            "´",
            "-",
            "_",
            "_",
            "_",
        };
    }

    private sealed class HamburgerSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "☱",
            "☲",
            "☴",
        };
    }

    private sealed class GrowVerticalSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▁",
            "▃",
            "▄",
            "▅",
            "▆",
            "▇",
            "▆",
            "▅",
            "▄",
            "▃",
        };
    }

    private sealed class GrowHorizontalSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▏",
            "▎",
            "▍",
            "▌",
            "▋",
            "▊",
            "▉",
            "▊",
            "▋",
            "▌",
            "▍",
            "▎",
        };
    }

    private sealed class BalloonSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(140);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            " ",
            ".",
            "o",
            "O",
            "@",
            "*",
            " ",
        };
    }

    private sealed class Balloon2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            ".",
            "o",
            "O",
            "°",
            "O",
            "o",
            ".",
        };
    }

    private sealed class NoiseSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▓",
            "▒",
            "░",
        };
    }

    private sealed class BounceSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⠁",
            "⠂",
            "⠄",
            "⠂",
        };
    }

    private sealed class BoxBounceSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▖",
            "▘",
            "▝",
            "▗",
        };
    }

    private sealed class BoxBounce2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▌",
            "▀",
            "▐",
            "▄",
        };
    }

    private sealed class TriangleSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◢",
            "◣",
            "◤",
            "◥",
        };
    }

    private sealed class BinarySpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "010010",
            "001100",
            "100101",
            "111010",
            "111101",
            "010111",
            "101011",
            "111000",
            "110011",
            "110101",
        };
    }

    private sealed class ArcSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◜",
            "◠",
            "◝",
            "◞",
            "◡",
            "◟",
        };
    }

    private sealed class CircleSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◡",
            "⊙",
            "◠",
        };
    }

    private sealed class SquareCornersSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(180);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◰",
            "◳",
            "◲",
            "◱",
        };
    }

    private sealed class CircleQuartersSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◴",
            "◷",
            "◶",
            "◵",
        };
    }

    private sealed class CircleHalvesSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◐",
            "◓",
            "◑",
            "◒",
        };
    }

    private sealed class SquishSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "╫",
            "╪",
        };
    }

    private sealed class ToggleSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(250);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⊶",
            "⊷",
        };
    }

    private sealed class Toggle2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▫",
            "▪",
        };
    }

    private sealed class Toggle3Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "□",
            "■",
        };
    }

    private sealed class Toggle4Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "■",
            "□",
            "▪",
            "▫",
        };
    }

    private sealed class Toggle5Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▮",
            "▯",
        };
    }

    private sealed class Toggle6Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(300);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "ဝ",
            "၀",
        };
    }

    private sealed class Toggle7Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⦾",
            "⦿",
        };
    }

    private sealed class Toggle8Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◍",
            "◌",
        };
    }

    private sealed class Toggle9Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "◉",
            "◎",
        };
    }

    private sealed class Toggle10Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "㊂",
            "㊀",
            "㊁",
        };
    }

    private sealed class Toggle11Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⧇",
            "⧆",
        };
    }

    private sealed class Toggle12Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "☗",
            "☖",
        };
    }

    private sealed class Toggle13Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "=",
            "*",
            "-",
        };
    }

    private sealed class ArrowSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "←",
            "↖",
            "↑",
            "↗",
            "→",
            "↘",
            "↓",
            "↙",
        };
    }

    private sealed class Arrow2Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "⬆️ ",
            "↗️ ",
            "➡️ ",
            "↘️ ",
            "⬇️ ",
            "↙️ ",
            "⬅️ ",
            "↖️ ",
        };
    }

    private sealed class Arrow3Spinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▹▹▹▹▹",
            "▸▹▹▹▹",
            "▹▸▹▹▹",
            "▹▹▸▹▹",
            "▹▹▹▸▹",
            "▹▹▹▹▸",
        };
    }

    private sealed class BouncingBarSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "[    ]",
            "[=   ]",
            "[==  ]",
            "[=== ]",
            "[====]",
            "[ ===]",
            "[  ==]",
            "[   =]",
            "[    ]",
            "[   =]",
            "[  ==]",
            "[ ===]",
            "[====]",
            "[=== ]",
            "[==  ]",
            "[=   ]",
        };
    }

    private sealed class BouncingBallSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "( ●    )",
            "(  ●   )",
            "(   ●  )",
            "(    ● )",
            "(     ●)",
            "(    ● )",
            "(   ●  )",
            "(  ●   )",
            "( ●    )",
            "(●     )",
        };
    }

    private sealed class SmileySpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(200);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "😄 ",
            "😝 ",
        };
    }

    private sealed class MonkeySpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(300);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🙈 ",
            "🙈 ",
            "🙉 ",
            "🙊 ",
        };
    }

    private sealed class HeartsSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "💛 ",
            "💙 ",
            "💜 ",
            "💚 ",
            "❤️ ",
        };
    }

    private sealed class ClockSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🕛 ",
            "🕐 ",
            "🕑 ",
            "🕒 ",
            "🕓 ",
            "🕔 ",
            "🕕 ",
            "🕖 ",
            "🕗 ",
            "🕘 ",
            "🕙 ",
            "🕚 ",
        };
    }

    private sealed class EarthSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(180);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🌍 ",
            "🌎 ",
            "🌏 ",
        };
    }

    private sealed class MaterialSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(17);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "█▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "███▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "████▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "██████▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "██████▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "███████▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "████████▁▁▁▁▁▁▁▁▁▁▁▁",
            "█████████▁▁▁▁▁▁▁▁▁▁▁",
            "█████████▁▁▁▁▁▁▁▁▁▁▁",
            "██████████▁▁▁▁▁▁▁▁▁▁",
            "███████████▁▁▁▁▁▁▁▁▁",
            "█████████████▁▁▁▁▁▁▁",
            "██████████████▁▁▁▁▁▁",
            "██████████████▁▁▁▁▁▁",
            "▁██████████████▁▁▁▁▁",
            "▁██████████████▁▁▁▁▁",
            "▁██████████████▁▁▁▁▁",
            "▁▁██████████████▁▁▁▁",
            "▁▁▁██████████████▁▁▁",
            "▁▁▁▁█████████████▁▁▁",
            "▁▁▁▁██████████████▁▁",
            "▁▁▁▁██████████████▁▁",
            "▁▁▁▁▁██████████████▁",
            "▁▁▁▁▁██████████████▁",
            "▁▁▁▁▁██████████████▁",
            "▁▁▁▁▁▁██████████████",
            "▁▁▁▁▁▁██████████████",
            "▁▁▁▁▁▁▁█████████████",
            "▁▁▁▁▁▁▁█████████████",
            "▁▁▁▁▁▁▁▁████████████",
            "▁▁▁▁▁▁▁▁████████████",
            "▁▁▁▁▁▁▁▁▁███████████",
            "▁▁▁▁▁▁▁▁▁███████████",
            "▁▁▁▁▁▁▁▁▁▁██████████",
            "▁▁▁▁▁▁▁▁▁▁██████████",
            "▁▁▁▁▁▁▁▁▁▁▁▁████████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁██████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
            "█▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
            "██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
            "██▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
            "███▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
            "████▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
            "█████▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
            "█████▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
            "██████▁▁▁▁▁▁▁▁▁▁▁▁▁█",
            "████████▁▁▁▁▁▁▁▁▁▁▁▁",
            "█████████▁▁▁▁▁▁▁▁▁▁▁",
            "█████████▁▁▁▁▁▁▁▁▁▁▁",
            "█████████▁▁▁▁▁▁▁▁▁▁▁",
            "█████████▁▁▁▁▁▁▁▁▁▁▁",
            "███████████▁▁▁▁▁▁▁▁▁",
            "████████████▁▁▁▁▁▁▁▁",
            "████████████▁▁▁▁▁▁▁▁",
            "██████████████▁▁▁▁▁▁",
            "██████████████▁▁▁▁▁▁",
            "▁██████████████▁▁▁▁▁",
            "▁██████████████▁▁▁▁▁",
            "▁▁▁█████████████▁▁▁▁",
            "▁▁▁▁▁████████████▁▁▁",
            "▁▁▁▁▁████████████▁▁▁",
            "▁▁▁▁▁▁███████████▁▁▁",
            "▁▁▁▁▁▁▁▁█████████▁▁▁",
            "▁▁▁▁▁▁▁▁█████████▁▁▁",
            "▁▁▁▁▁▁▁▁▁█████████▁▁",
            "▁▁▁▁▁▁▁▁▁█████████▁▁",
            "▁▁▁▁▁▁▁▁▁▁█████████▁",
            "▁▁▁▁▁▁▁▁▁▁▁████████▁",
            "▁▁▁▁▁▁▁▁▁▁▁████████▁",
            "▁▁▁▁▁▁▁▁▁▁▁▁███████▁",
            "▁▁▁▁▁▁▁▁▁▁▁▁███████▁",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁███████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁████",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁███",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁██",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁█",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
            "▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁▁",
        };
    }

    private sealed class MoonSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🌑 ",
            "🌒 ",
            "🌓 ",
            "🌔 ",
            "🌕 ",
            "🌖 ",
            "🌗 ",
            "🌘 ",
        };
    }

    private sealed class RunnerSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(140);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🚶 ",
            "🏃 ",
        };
    }

    private sealed class PongSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▐⠂       ▌",
            "▐⠈       ▌",
            "▐ ⠂      ▌",
            "▐ ⠠      ▌",
            "▐  ⡀     ▌",
            "▐  ⠠     ▌",
            "▐   ⠂    ▌",
            "▐   ⠈    ▌",
            "▐    ⠂   ▌",
            "▐    ⠠   ▌",
            "▐     ⡀  ▌",
            "▐     ⠠  ▌",
            "▐      ⠂ ▌",
            "▐      ⠈ ▌",
            "▐       ⠂▌",
            "▐       ⠠▌",
            "▐       ⡀▌",
            "▐      ⠠ ▌",
            "▐      ⠂ ▌",
            "▐     ⠈  ▌",
            "▐     ⠂  ▌",
            "▐    ⠠   ▌",
            "▐    ⡀   ▌",
            "▐   ⠠    ▌",
            "▐   ⠂    ▌",
            "▐  ⠈     ▌",
            "▐  ⠂     ▌",
            "▐ ⠠      ▌",
            "▐ ⡀      ▌",
            "▐⠠       ▌",
        };
    }

    private sealed class SharkSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(120);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▐|\\____________▌",
            "▐_|\\___________▌",
            "▐__|\\__________▌",
            "▐___|\\_________▌",
            "▐____|\\________▌",
            "▐_____|\\_______▌",
            "▐______|\\______▌",
            "▐_______|\\_____▌",
            "▐________|\\____▌",
            "▐_________|\\___▌",
            "▐__________|\\__▌",
            "▐___________|\\_▌",
            "▐____________|\\▌",
            "▐____________/|▌",
            "▐___________/|_▌",
            "▐__________/|__▌",
            "▐_________/|___▌",
            "▐________/|____▌",
            "▐_______/|_____▌",
            "▐______/|______▌",
            "▐_____/|_______▌",
            "▐____/|________▌",
            "▐___/|_________▌",
            "▐__/|__________▌",
            "▐_/|___________▌",
            "▐/|____________▌",
        };
    }

    private sealed class DqpbSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "d",
            "q",
            "p",
            "b",
        };
    }

    private sealed class WeatherSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "☀️ ",
            "☀️ ",
            "☀️ ",
            "🌤 ",
            "⛅️ ",
            "🌥 ",
            "☁️ ",
            "🌧 ",
            "🌨 ",
            "🌧 ",
            "🌨 ",
            "🌧 ",
            "🌨 ",
            "⛈ ",
            "🌨 ",
            "🌧 ",
            "🌨 ",
            "☁️ ",
            "🌥 ",
            "⛅️ ",
            "🌤 ",
            "☀️ ",
            "☀️ ",
        };
    }

    private sealed class ChristmasSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(400);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🌲",
            "🎄",
        };
    }

    private sealed class GrenadeSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "،  ",
            "′  ",
            " ´ ",
            " ‾ ",
            "  ⸌",
            "  ⸊",
            "  |",
            "  ⁎",
            "  ⁕",
            " ෴ ",
            "  ⁓",
            "   ",
            "   ",
            "   ",
        };
    }

    private sealed class PointSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(125);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "∙∙∙",
            "●∙∙",
            "∙●∙",
            "∙∙●",
            "∙∙∙",
        };
    }

    private sealed class LayerSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(150);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "-",
            "=",
            "≡",
        };
    }

    private sealed class BetaWaveSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "ρββββββ",
            "βρβββββ",
            "ββρββββ",
            "βββρβββ",
            "ββββρββ",
            "βββββρβ",
            "ββββββρ",
        };
    }

    private sealed class FingerDanceSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(160);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🤘 ",
            "🤟 ",
            "🖖 ",
            "✋ ",
            "🤚 ",
            "👆 ",
        };
    }

    private sealed class FistBumpSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🤜　　　　🤛 ",
            "🤜　　　　🤛 ",
            "🤜　　　　🤛 ",
            "　🤜　　🤛　 ",
            "　　🤜🤛　　 ",
            "　🤜✨🤛　　 ",
            "🤜　✨　🤛　 ",
        };
    }

    private sealed class SoccerHeaderSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            " 🧑⚽️       🧑 ",
            "🧑  ⚽️      🧑 ",
            "🧑   ⚽️     🧑 ",
            "🧑    ⚽️    🧑 ",
            "🧑     ⚽️   🧑 ",
            "🧑      ⚽️  🧑 ",
            "🧑       ⚽️🧑  ",
            "🧑      ⚽️  🧑 ",
            "🧑     ⚽️   🧑 ",
            "🧑    ⚽️    🧑 ",
            "🧑   ⚽️     🧑 ",
            "🧑  ⚽️      🧑 ",
        };
    }

    private sealed class MindblownSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(160);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "😐 ",
            "😐 ",
            "😮 ",
            "😮 ",
            "😦 ",
            "😦 ",
            "😧 ",
            "😧 ",
            "🤯 ",
            "💥 ",
            "✨ ",
            "　 ",
            "　 ",
            "　 ",
        };
    }

    private sealed class SpeakerSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(160);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🔈 ",
            "🔉 ",
            "🔊 ",
            "🔉 ",
        };
    }

    private sealed class OrangePulseSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🔸 ",
            "🔶 ",
            "🟠 ",
            "🟠 ",
            "🔶 ",
        };
    }

    private sealed class BluePulseSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🔹 ",
            "🔷 ",
            "🔵 ",
            "🔵 ",
            "🔷 ",
        };
    }

    private sealed class OrangeBluePulseSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🔸 ",
            "🔶 ",
            "🟠 ",
            "🟠 ",
            "🔶 ",
            "🔹 ",
            "🔷 ",
            "🔵 ",
            "🔵 ",
            "🔷 ",
        };
    }

    private sealed class TimeTravelSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(100);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "🕛 ",
            "🕚 ",
            "🕙 ",
            "🕘 ",
            "🕗 ",
            "🕖 ",
            "🕕 ",
            "🕔 ",
            "🕓 ",
            "🕒 ",
            "🕑 ",
            "🕐 ",
        };
    }

    private sealed class AestheticSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            "▰▱▱▱▱▱▱",
            "▰▰▱▱▱▱▱",
            "▰▰▰▱▱▱▱",
            "▰▰▰▰▱▱▱",
            "▰▰▰▰▰▱▱",
            "▰▰▰▰▰▰▱",
            "▰▰▰▰▰▰▰",
            "▰▱▱▱▱▱▱",
        };
    }

    private sealed class DwarfFortressSpinner : SpinnerKind
    {
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(80);

        public override IReadOnlyList<string> Frames => new List<string>
        {
            " ██████£££  ",
            "☺██████£££  ",
            "☺██████£££  ",
            "☺▓█████£££  ",
            "☺▓█████£££  ",
            "☺▒█████£££  ",
            "☺▒█████£££  ",
            "☺░█████£££  ",
            "☺░█████£££  ",
            "☺ █████£££  ",
            " ☺█████£££  ",
            " ☺█████£££  ",
            " ☺▓████£££  ",
            " ☺▓████£££  ",
            " ☺▒████£££  ",
            " ☺▒████£££  ",
            " ☺░████£££  ",
            " ☺░████£££  ",
            " ☺ ████£££  ",
            "  ☺████£££  ",
            "  ☺████£££  ",
            "  ☺▓███£££  ",
            "  ☺▓███£££  ",
            "  ☺▒███£££  ",
            "  ☺▒███£££  ",
            "  ☺░███£££  ",
            "  ☺░███£££  ",
            "  ☺ ███£££  ",
            "   ☺███£££  ",
            "   ☺███£££  ",
            "   ☺▓██£££  ",
            "   ☺▓██£££  ",
            "   ☺▒██£££  ",
            "   ☺▒██£££  ",
            "   ☺░██£££  ",
            "   ☺░██£££  ",
            "   ☺ ██£££  ",
            "    ☺██£££  ",
            "    ☺██£££  ",
            "    ☺▓█£££  ",
            "    ☺▓█£££  ",
            "    ☺▒█£££  ",
            "    ☺▒█£££  ",
            "    ☺░█£££  ",
            "    ☺░█£££  ",
            "    ☺ █£££  ",
            "     ☺█£££  ",
            "     ☺█£££  ",
            "     ☺▓£££  ",
            "     ☺▓£££  ",
            "     ☺▒£££  ",
            "     ☺▒£££  ",
            "     ☺░£££  ",
            "     ☺░£££  ",
            "     ☺ £££  ",
            "      ☺£££  ",
            "      ☺£££  ",
            "      ☺▓££  ",
            "      ☺▓££  ",
            "      ☺▒££  ",
            "      ☺▒££  ",
            "      ☺░££  ",
            "      ☺░££  ",
            "      ☺ ££  ",
            "       ☺££  ",
            "       ☺££  ",
            "       ☺▓£  ",
            "       ☺▓£  ",
            "       ☺▒£  ",
            "       ☺▒£  ",
            "       ☺░£  ",
            "       ☺░£  ",
            "       ☺ £  ",
            "        ☺£  ",
            "        ☺£  ",
            "        ☺▓  ",
            "        ☺▓  ",
            "        ☺▒  ",
            "        ☺▒  ",
            "        ☺░  ",
            "        ☺░  ",
            "        ☺   ",
            "        ☺  &",
            "        ☺ ☼&",
            "       ☺ ☼ &",
            "       ☺☼  &",
            "      ☺☼  & ",
            "      ‼   & ",
            "     ☺   &  ",
            "    ‼    &  ",
            "   ☺    &   ",
            "  ‼     &   ",
            " ☺     &    ",
            "‼      &    ",
            "      &     ",
            "      &     ",
            "     &   ░  ",
            "     &   ▒  ",
            "    &    ▓  ",
            "    &    £  ",
            "   &    ░£  ",
            "   &    ▒£  ",
            "  &     ▓£  ",
            "  &     ££  ",
            " &     ░££  ",
            " &     ▒££  ",
            "&      ▓££  ",
            "&      £££  ",
            "      ░£££  ",
            "      ▒£££  ",
            "      ▓£££  ",
            "      █£££  ",
            "     ░█£££  ",
            "     ▒█£££  ",
            "     ▓█£££  ",
            "     ██£££  ",
            "    ░██£££  ",
            "    ▒██£££  ",
            "    ▓██£££  ",
            "    ███£££  ",
            "   ░███£££  ",
            "   ▒███£££  ",
            "   ▓███£££  ",
            "   ████£££  ",
            "  ░████£££  ",
            "  ▒████£££  ",
            "  ▓████£££  ",
            "  █████£££  ",
            " ░█████£££  ",
            " ▒█████£££  ",
            " ▓█████£££  ",
            " ██████£££  ",
            " ██████£££  ",
        };
    }


    /// <summary>
    /// Gets the "Default" spinner.
    /// </summary>
    public static SpinnerKind Default { get; } = new DefaultSpinner();

    /// <summary>
    /// Gets the "Ascii" spinner.
    /// </summary>
    public static SpinnerKind Ascii { get; } = new AsciiSpinner();

    /// <summary>
    /// Gets the "dots" spinner.
    /// </summary>
    public static SpinnerKind Dots { get; } = new DotsSpinner();

    /// <summary>
    /// Gets the "dots2" spinner.
    /// </summary>
    public static SpinnerKind Dots2 { get; } = new Dots2Spinner();

    /// <summary>
    /// Gets the "dots3" spinner.
    /// </summary>
    public static SpinnerKind Dots3 { get; } = new Dots3Spinner();

    /// <summary>
    /// Gets the "dots4" spinner.
    /// </summary>
    public static SpinnerKind Dots4 { get; } = new Dots4Spinner();

    /// <summary>
    /// Gets the "dots5" spinner.
    /// </summary>
    public static SpinnerKind Dots5 { get; } = new Dots5Spinner();

    /// <summary>
    /// Gets the "dots6" spinner.
    /// </summary>
    public static SpinnerKind Dots6 { get; } = new Dots6Spinner();

    /// <summary>
    /// Gets the "dots7" spinner.
    /// </summary>
    public static SpinnerKind Dots7 { get; } = new Dots7Spinner();

    /// <summary>
    /// Gets the "dots8" spinner.
    /// </summary>
    public static SpinnerKind Dots8 { get; } = new Dots8Spinner();

    /// <summary>
    /// Gets the "dots9" spinner.
    /// </summary>
    public static SpinnerKind Dots9 { get; } = new Dots9Spinner();

    /// <summary>
    /// Gets the "dots10" spinner.
    /// </summary>
    public static SpinnerKind Dots10 { get; } = new Dots10Spinner();

    /// <summary>
    /// Gets the "dots11" spinner.
    /// </summary>
    public static SpinnerKind Dots11 { get; } = new Dots11Spinner();

    /// <summary>
    /// Gets the "dots12" spinner.
    /// </summary>
    public static SpinnerKind Dots12 { get; } = new Dots12Spinner();

    /// <summary>
    /// Gets the "dots13" spinner.
    /// </summary>
    public static SpinnerKind Dots13 { get; } = new Dots13Spinner();

    /// <summary>
    /// Gets the "dots14" spinner.
    /// </summary>
    public static SpinnerKind Dots14 { get; } = new Dots14Spinner();

    /// <summary>
    /// Gets the "dots8Bit" spinner.
    /// </summary>
    public static SpinnerKind Dots8Bit { get; } = new Dots8BitSpinner();

    /// <summary>
    /// Gets the "dotsCircle" spinner.
    /// </summary>
    public static SpinnerKind DotsCircle { get; } = new DotsCircleSpinner();

    /// <summary>
    /// Gets the "sand" spinner.
    /// </summary>
    public static SpinnerKind Sand { get; } = new SandSpinner();

    /// <summary>
    /// Gets the "line" spinner.
    /// </summary>
    public static SpinnerKind Line { get; } = new LineSpinner();

    /// <summary>
    /// Gets the "line2" spinner.
    /// </summary>
    public static SpinnerKind Line2 { get; } = new Line2Spinner();

    /// <summary>
    /// Gets the "pipe" spinner.
    /// </summary>
    public static SpinnerKind Pipe { get; } = new PipeSpinner();

    /// <summary>
    /// Gets the "simpleDots" spinner.
    /// </summary>
    public static SpinnerKind SimpleDots { get; } = new SimpleDotsSpinner();

    /// <summary>
    /// Gets the "simpleDotsScrolling" spinner.
    /// </summary>
    public static SpinnerKind SimpleDotsScrolling { get; } = new SimpleDotsScrollingSpinner();

    /// <summary>
    /// Gets the "star" spinner.
    /// </summary>
    public static SpinnerKind Star { get; } = new StarSpinner();

    /// <summary>
    /// Gets the "star2" spinner.
    /// </summary>
    public static SpinnerKind Star2 { get; } = new Star2Spinner();

    /// <summary>
    /// Gets the "flip" spinner.
    /// </summary>
    public static SpinnerKind Flip { get; } = new FlipSpinner();

    /// <summary>
    /// Gets the "hamburger" spinner.
    /// </summary>
    public static SpinnerKind Hamburger { get; } = new HamburgerSpinner();

    /// <summary>
    /// Gets the "growVertical" spinner.
    /// </summary>
    public static SpinnerKind GrowVertical { get; } = new GrowVerticalSpinner();

    /// <summary>
    /// Gets the "growHorizontal" spinner.
    /// </summary>
    public static SpinnerKind GrowHorizontal { get; } = new GrowHorizontalSpinner();

    /// <summary>
    /// Gets the "balloon" spinner.
    /// </summary>
    public static SpinnerKind Balloon { get; } = new BalloonSpinner();

    /// <summary>
    /// Gets the "balloon2" spinner.
    /// </summary>
    public static SpinnerKind Balloon2 { get; } = new Balloon2Spinner();

    /// <summary>
    /// Gets the "noise" spinner.
    /// </summary>
    public static SpinnerKind Noise { get; } = new NoiseSpinner();

    /// <summary>
    /// Gets the "bounce" spinner.
    /// </summary>
    public static SpinnerKind Bounce { get; } = new BounceSpinner();

    /// <summary>
    /// Gets the "boxBounce" spinner.
    /// </summary>
    public static SpinnerKind BoxBounce { get; } = new BoxBounceSpinner();

    /// <summary>
    /// Gets the "boxBounce2" spinner.
    /// </summary>
    public static SpinnerKind BoxBounce2 { get; } = new BoxBounce2Spinner();

    /// <summary>
    /// Gets the "triangle" spinner.
    /// </summary>
    public static SpinnerKind Triangle { get; } = new TriangleSpinner();

    /// <summary>
    /// Gets the "binary" spinner.
    /// </summary>
    public static SpinnerKind Binary { get; } = new BinarySpinner();

    /// <summary>
    /// Gets the "arc" spinner.
    /// </summary>
    public static SpinnerKind Arc { get; } = new ArcSpinner();

    /// <summary>
    /// Gets the "circle" spinner.
    /// </summary>
    public static SpinnerKind Circle { get; } = new CircleSpinner();

    /// <summary>
    /// Gets the "squareCorners" spinner.
    /// </summary>
    public static SpinnerKind SquareCorners { get; } = new SquareCornersSpinner();

    /// <summary>
    /// Gets the "circleQuarters" spinner.
    /// </summary>
    public static SpinnerKind CircleQuarters { get; } = new CircleQuartersSpinner();

    /// <summary>
    /// Gets the "circleHalves" spinner.
    /// </summary>
    public static SpinnerKind CircleHalves { get; } = new CircleHalvesSpinner();

    /// <summary>
    /// Gets the "squish" spinner.
    /// </summary>
    public static SpinnerKind Squish { get; } = new SquishSpinner();

    /// <summary>
    /// Gets the "toggle" spinner.
    /// </summary>
    public static SpinnerKind Toggle { get; } = new ToggleSpinner();

    /// <summary>
    /// Gets the "toggle2" spinner.
    /// </summary>
    public static SpinnerKind Toggle2 { get; } = new Toggle2Spinner();

    /// <summary>
    /// Gets the "toggle3" spinner.
    /// </summary>
    public static SpinnerKind Toggle3 { get; } = new Toggle3Spinner();

    /// <summary>
    /// Gets the "toggle4" spinner.
    /// </summary>
    public static SpinnerKind Toggle4 { get; } = new Toggle4Spinner();

    /// <summary>
    /// Gets the "toggle5" spinner.
    /// </summary>
    public static SpinnerKind Toggle5 { get; } = new Toggle5Spinner();

    /// <summary>
    /// Gets the "toggle6" spinner.
    /// </summary>
    public static SpinnerKind Toggle6 { get; } = new Toggle6Spinner();

    /// <summary>
    /// Gets the "toggle7" spinner.
    /// </summary>
    public static SpinnerKind Toggle7 { get; } = new Toggle7Spinner();

    /// <summary>
    /// Gets the "toggle8" spinner.
    /// </summary>
    public static SpinnerKind Toggle8 { get; } = new Toggle8Spinner();

    /// <summary>
    /// Gets the "toggle9" spinner.
    /// </summary>
    public static SpinnerKind Toggle9 { get; } = new Toggle9Spinner();

    /// <summary>
    /// Gets the "toggle10" spinner.
    /// </summary>
    public static SpinnerKind Toggle10 { get; } = new Toggle10Spinner();

    /// <summary>
    /// Gets the "toggle11" spinner.
    /// </summary>
    public static SpinnerKind Toggle11 { get; } = new Toggle11Spinner();

    /// <summary>
    /// Gets the "toggle12" spinner.
    /// </summary>
    public static SpinnerKind Toggle12 { get; } = new Toggle12Spinner();

    /// <summary>
    /// Gets the "toggle13" spinner.
    /// </summary>
    public static SpinnerKind Toggle13 { get; } = new Toggle13Spinner();

    /// <summary>
    /// Gets the "arrow" spinner.
    /// </summary>
    public static SpinnerKind Arrow { get; } = new ArrowSpinner();

    /// <summary>
    /// Gets the "arrow2" spinner.
    /// </summary>
    public static SpinnerKind Arrow2 { get; } = new Arrow2Spinner();

    /// <summary>
    /// Gets the "arrow3" spinner.
    /// </summary>
    public static SpinnerKind Arrow3 { get; } = new Arrow3Spinner();

    /// <summary>
    /// Gets the "bouncingBar" spinner.
    /// </summary>
    public static SpinnerKind BouncingBar { get; } = new BouncingBarSpinner();

    /// <summary>
    /// Gets the "bouncingBall" spinner.
    /// </summary>
    public static SpinnerKind BouncingBall { get; } = new BouncingBallSpinner();

    /// <summary>
    /// Gets the "smiley" spinner.
    /// </summary>
    public static SpinnerKind Smiley { get; } = new SmileySpinner();

    /// <summary>
    /// Gets the "monkey" spinner.
    /// </summary>
    public static SpinnerKind Monkey { get; } = new MonkeySpinner();

    /// <summary>
    /// Gets the "hearts" spinner.
    /// </summary>
    public static SpinnerKind Hearts { get; } = new HeartsSpinner();

    /// <summary>
    /// Gets the "clock" spinner.
    /// </summary>
    public static SpinnerKind Clock { get; } = new ClockSpinner();

    /// <summary>
    /// Gets the "earth" spinner.
    /// </summary>
    public static SpinnerKind Earth { get; } = new EarthSpinner();

    /// <summary>
    /// Gets the "material" spinner.
    /// </summary>
    public static SpinnerKind Material { get; } = new MaterialSpinner();

    /// <summary>
    /// Gets the "moon" spinner.
    /// </summary>
    public static SpinnerKind Moon { get; } = new MoonSpinner();

    /// <summary>
    /// Gets the "runner" spinner.
    /// </summary>
    public static SpinnerKind Runner { get; } = new RunnerSpinner();

    /// <summary>
    /// Gets the "pong" spinner.
    /// </summary>
    public static SpinnerKind Pong { get; } = new PongSpinner();

    /// <summary>
    /// Gets the "shark" spinner.
    /// </summary>
    public static SpinnerKind Shark { get; } = new SharkSpinner();

    /// <summary>
    /// Gets the "dqpb" spinner.
    /// </summary>
    public static SpinnerKind Dqpb { get; } = new DqpbSpinner();

    /// <summary>
    /// Gets the "weather" spinner.
    /// </summary>
    public static SpinnerKind Weather { get; } = new WeatherSpinner();

    /// <summary>
    /// Gets the "christmas" spinner.
    /// </summary>
    public static SpinnerKind Christmas { get; } = new ChristmasSpinner();

    /// <summary>
    /// Gets the "grenade" spinner.
    /// </summary>
    public static SpinnerKind Grenade { get; } = new GrenadeSpinner();

    /// <summary>
    /// Gets the "point" spinner.
    /// </summary>
    public static SpinnerKind Point { get; } = new PointSpinner();

    /// <summary>
    /// Gets the "layer" spinner.
    /// </summary>
    public static SpinnerKind Layer { get; } = new LayerSpinner();

    /// <summary>
    /// Gets the "betaWave" spinner.
    /// </summary>
    public static SpinnerKind BetaWave { get; } = new BetaWaveSpinner();

    /// <summary>
    /// Gets the "fingerDance" spinner.
    /// </summary>
    public static SpinnerKind FingerDance { get; } = new FingerDanceSpinner();

    /// <summary>
    /// Gets the "fistBump" spinner.
    /// </summary>
    public static SpinnerKind FistBump { get; } = new FistBumpSpinner();

    /// <summary>
    /// Gets the "soccerHeader" spinner.
    /// </summary>
    public static SpinnerKind SoccerHeader { get; } = new SoccerHeaderSpinner();

    /// <summary>
    /// Gets the "mindblown" spinner.
    /// </summary>
    public static SpinnerKind Mindblown { get; } = new MindblownSpinner();

    /// <summary>
    /// Gets the "speaker" spinner.
    /// </summary>
    public static SpinnerKind Speaker { get; } = new SpeakerSpinner();

    /// <summary>
    /// Gets the "orangePulse" spinner.
    /// </summary>
    public static SpinnerKind OrangePulse { get; } = new OrangePulseSpinner();

    /// <summary>
    /// Gets the "bluePulse" spinner.
    /// </summary>
    public static SpinnerKind BluePulse { get; } = new BluePulseSpinner();

    /// <summary>
    /// Gets the "orangeBluePulse" spinner.
    /// </summary>
    public static SpinnerKind OrangeBluePulse { get; } = new OrangeBluePulseSpinner();

    /// <summary>
    /// Gets the "timeTravel" spinner.
    /// </summary>
    public static SpinnerKind TimeTravel { get; } = new TimeTravelSpinner();

    /// <summary>
    /// Gets the "aesthetic" spinner.
    /// </summary>
    public static SpinnerKind Aesthetic { get; } = new AestheticSpinner();

    /// <summary>
    /// Gets the "dwarfFortress" spinner.
    /// </summary>
    public static SpinnerKind DwarfFortress { get; } = new DwarfFortressSpinner();
}