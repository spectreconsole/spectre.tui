namespace Spectre.Tui;

[PublicAPI]
public sealed class TableColumn
{
    public TextLine Header { get; set; }
    public ColumnWidth Width { get; set; } = ColumnWidth.Auto;
    public Justify Alignment { get; set; } = Justify.Left;
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
    public bool Wrap { get; set; }

    public TableColumn(TextLine header)
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
    }

    public static implicit operator TableColumn(string header) => new(header);
}

[PublicAPI]
public readonly record struct ColumnWidth
{
    internal enum Mode
    {
        Auto,
        Fixed,
        Star,
    }

    internal Mode Kind { get; }
    internal int Value { get; }

    private ColumnWidth(Mode kind, int value)
    {
        Kind = kind;
        Value = value;
    }

    public static ColumnWidth Auto { get; } = new(Mode.Auto, 0);

    public static ColumnWidth Fixed(int width)
    {
        return new ColumnWidth(Mode.Fixed, Math.Max(0, width));
    }

    public static ColumnWidth Star(int weight = 1)
    {
        return new ColumnWidth(Mode.Star, Math.Max(1, weight));
    }
}

[PublicAPI]
public static class TableColumnExtensions
{
    extension(TableColumn column)
    {
        public TableColumn AutoWidth()
        {
            column.Width = ColumnWidth.Auto;
            return column;
        }

        public TableColumn FixedWidth(int width)
        {
            column.Width = ColumnWidth.Fixed(width);
            return column;
        }

        public TableColumn StarWidth(int weight)
        {
            column.Width = ColumnWidth.Star(weight);
            return column;
        }

        public TableColumn SetWidth(ColumnWidth width)
        {
            column.Width = width;
            return column;
        }

        public TableColumn SetAlignment(Justify alignment)
        {
            column.Alignment = alignment;
            return column;
        }

        public TableColumn LeftAligned()
        {
            column.Alignment = Justify.Left;
            return column;
        }

        public TableColumn RightAligned()
        {
            column.Alignment = Justify.Right;
            return column;
        }

        public TableColumn Centered()
        {
            column.Alignment = Justify.Center;
            return column;
        }

        public TableColumn WrapText(bool wrap = true)
        {
            column.Wrap = wrap;
            return column;
        }

        public TableColumn SetVerticalAlignment(VerticalAlignment alignment)
        {
            column.VerticalAlignment = alignment;
            return column;
        }

        public TableColumn VerticallyTop()
        {
            column.VerticalAlignment = VerticalAlignment.Top;
            return column;
        }

        public TableColumn VerticallyMiddle()
        {
            column.VerticalAlignment = VerticalAlignment.Middle;
            return column;
        }

        public TableColumn VerticallyBottom()
        {
            column.VerticalAlignment = VerticalAlignment.Bottom;
            return column;
        }
    }
}
