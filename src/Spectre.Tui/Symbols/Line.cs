namespace Spectre.Tui;

public sealed class Line
{
    public required char Vertical { get; init; }
    public required char Horizontal { get; init; }
    public required char TopLeft { get; init; }
    public required char TopRight { get; init; }
    public required char BottomLeft { get; init; }
    public required char BottomRight { get; init; }
    public required char VerticalLeft { get; init; }
    public required char VerticalRight { get; init; }
    public required char HorizontalUp { get; init; }
    public required char HorizontalDown { get; init; }
    public required char Cross { get; init; }

    public static class Symbols
    {
        public const char Vertical = '│';
        public const char DoubleVertical = '║';
        public const char BoldVertical = '┃';

        public const char Horizontal = '─';
        public const char DoubleHorizontal = '═';
        public const char BoldHorizontal = '━';

        public const char TopLeft = '┌';
        public const char RoundedTopLeft = '╭';
        public const char DoubleTopLeft = '╔';
        public const char BoldTopLeft = '┏';

        public const char TopRight = '┐';
        public const char RoundedTopRight = '╮';
        public const char DoubleTopRight = '╗';
        public const char BoldTopRight = '┓';

        public const char BottomLeft = '└';
        public const char RoundedBottomLeft = '╰';
        public const char DoubleBottomLeft = '╚';
        public const char BoldBottomLeft = '┗';

        public const char BottomRight = '┘';
        public const char RoundedBottomRight = '╯';
        public const char DoubleBottomRight = '╝';
        public const char BoldBottomRight = '┛';

        public const char VerticalLeft = '┤';
        public const char DoubleVerticalLeft = '╣';
        public const char BoldVerticalLeft = '┫';

        public const char VerticalRight = '├';
        public const char DoubleVerticalRight = '╠';
        public const char BoldVerticalRight = '┣';

        public const char HorizontalUp = '┴';
        public const char DoubleHorizontalUp = '╩';
        public const char BoldHorizontalUp = '┻';

        public const char HorizontalDown = '┬';
        public const char DoubleHorizontalDown = '╦';
        public const char BoldHorizontalDown = '┳';

        public const char Cross = '┼';
        public const char DoubleCross = '╬';
        public const char BoldCross = '╋';
    }

    public static Line Plain { get; } = new Line
    {
        Vertical = Symbols.Vertical,
        Horizontal = Symbols.Horizontal,
        TopLeft = Symbols.TopLeft,
        TopRight = Symbols.TopRight,
        BottomLeft = Symbols.BottomLeft,
        BottomRight = Symbols.BottomRight,
        VerticalLeft = Symbols.VerticalLeft,
        VerticalRight = Symbols.VerticalRight,
        HorizontalUp = Symbols.HorizontalUp,
        HorizontalDown = Symbols.HorizontalDown,
        Cross = Symbols.Cross,
    };

    public static Line Rounded { get; } = new Line
    {
        Vertical = Symbols.Vertical,
        Horizontal = Symbols.Horizontal,
        TopLeft = Symbols.RoundedTopLeft,
        TopRight = Symbols.RoundedTopRight,
        BottomLeft = Symbols.RoundedBottomLeft,
        BottomRight = Symbols.RoundedBottomRight,
        VerticalLeft = Symbols.VerticalLeft,
        VerticalRight = Symbols.VerticalRight,
        HorizontalUp = Symbols.HorizontalUp,
        HorizontalDown = Symbols.HorizontalDown,
        Cross = Symbols.Cross,
    };

    public static Line Double { get; } = new Line
    {
        Vertical = Symbols.DoubleVertical,
        Horizontal = Symbols.DoubleHorizontal,
        TopLeft = Symbols.DoubleTopLeft,
        TopRight = Symbols.DoubleTopRight,
        BottomLeft = Symbols.DoubleBottomLeft,
        BottomRight = Symbols.DoubleBottomRight,
        VerticalLeft = Symbols.DoubleVerticalLeft,
        VerticalRight = Symbols.DoubleVerticalRight,
        HorizontalUp = Symbols.DoubleHorizontalUp,
        HorizontalDown = Symbols.DoubleHorizontalDown,
        Cross = Symbols.DoubleCross,
    };

    public static Line Bold { get; } = new Line
    {
        Vertical = Symbols.BoldVertical,
        Horizontal = Symbols.BoldHorizontal,
        TopLeft = Symbols.BoldTopLeft,
        TopRight = Symbols.BoldTopRight,
        BottomLeft = Symbols.BoldBottomLeft,
        BottomRight = Symbols.BoldBottomRight,
        VerticalLeft = Symbols.BoldVerticalLeft,
        VerticalRight = Symbols.BoldVerticalRight,
        HorizontalUp = Symbols.BoldHorizontalUp,
        HorizontalDown = Symbols.BoldHorizontalDown,
        Cross = Symbols.BoldCross,
    };
}