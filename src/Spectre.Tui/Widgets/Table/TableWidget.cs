namespace Spectre.Tui;

[PublicAPI]
public sealed class TableWidget<TRow> : IWidget
    where TRow : ITableRow
{
    private int _offset;
    private int? _selectedIndex;

    public List<TRow> Rows { get; }
    public List<TableColumn> Columns { get; } = [];

    public Style? HighlightStyle { get; set; }
    public Style? HeaderStyle { get; set; }
    public bool WrapAround { get; set; }
    public bool ShowHeader { get; set; } = true;
    public bool ShowSeparator { get; set; } = true;
    public int ColumnSpacing { get; set; } = 2;

    public TRow? SelectedItem => GetSelectedItem();

    public int? SelectedIndex
    {
        get => _selectedIndex;
        set => SetSelectedIndex(value);
    }

    public TableWidget(params List<TRow> rows)
    {
        Rows = rows ?? throw new ArgumentNullException(nameof(rows));
    }

    public void MoveUp()
    {
        SetSelectedIndex(--_selectedIndex);
    }

    public void MoveDown()
    {
        SetSelectedIndex(++_selectedIndex);
    }

    public void MoveToStart()
    {
        SetSelectedIndex(0);
    }

    public void MoveToEnd()
    {
        SetSelectedIndex(Rows.Count - 1);
    }

    void IWidget.Render(RenderContext context)
    {
        if (Columns.Count == 0)
        {
            return;
        }

        var viewport = context.Viewport;
        if (viewport.Width <= 0 || viewport.Height <= 0)
        {
            return;
        }

        // Ensure the selected index is within range
        SetSelectedIndex(_selectedIndex);

        // Materialize all cells.
        // We should probably look into this in the future
        // since it's not a very good approach for large data sets.
        var rowCells = new Text[Rows.Count][];
        for (var rowIndex = 0; rowIndex < Rows.Count; rowIndex++)
        {
            rowCells[rowIndex] = NormalizeCells(Rows[rowIndex].CreateCells(rowIndex == _selectedIndex));
        }

        // Auto column measurements run over all rows
        // so layout stays stable while scrolling
        var autoMeasurements = MeasureAutoColumns(rowCells);

        // Compute column widths
        var widths = TableLayout.CalculateColumnWidths(
            Columns, autoMeasurements, viewport.Width, ColumnSpacing);

        // Compute column x-offsets
        var x = viewport.Left;
        var xOffsets = new int[Columns.Count];
        for (var i = 0; i < Columns.Count; i++)
        {
            xOffsets[i] = x;
            if (widths[i] > 0)
            {
                x += widths[i];
                if (i < Columns.Count - 1)
                {
                    x += ColumnSpacing;
                }
            }
        }

        // Resolve cell lines (apply soft-wrap where the column opts in) and row heights
        var rowLines = new TextLine[Rows.Count][][];
        var rowHeights = new int[Rows.Count];
        for (var r = 0; r < Rows.Count; r++)
        {
            rowLines[r] = new TextLine[Columns.Count][];
            var maxHeight = 1;
            for (var c = 0; c < Columns.Count; c++)
            {
                var lines = ResolveCellLines(rowCells[r][c], Columns[c], widths[c]);
                rowLines[r][c] = lines;
                if (lines.Length > maxHeight)
                {
                    maxHeight = lines.Length;
                }
            }

            rowHeights[r] = maxHeight;
        }

        var y = viewport.Top;
        var remainingHeight = viewport.Height;

        // Show header?
        if (ShowHeader && remainingHeight > 0)
        {
            for (var i = 0; i < Columns.Count; i++)
            {
                if (widths[i] <= 0)
                {
                    continue;
                }

                RenderCell(context, xOffsets[i], y, widths[i], Columns[i].Header, Columns[i].Alignment, HeaderStyle);
            }

            y++;
            remainingHeight--;
        }

        // Show separator?
        if (ShowHeader && ShowSeparator && remainingHeight > 0)
        {
            for (var sx = viewport.Left; sx < viewport.Right; sx++)
            {
                context.SetSymbol(sx, y, Border.Plain.HorizontalTop);
                if (HeaderStyle != null)
                {
                    context.SetStyle(sx, y, HeaderStyle);
                }
            }

            y++;
            remainingHeight--;
        }

        // Nothing to render?
        if (remainingHeight <= 0 || Rows.Count == 0)
        {
            return;
        }

        // Get the visible bounds and update the offset
        var (firstVisible, lastVisible) = GetVisibleBounds(rowHeights, remainingHeight);
        _offset = firstVisible;

        var rowY = y;
        for (var index = firstVisible; index < lastVisible; index++)
        {
            var height = rowHeights[index];
            var available = Math.Min(height, viewport.Top + viewport.Height - rowY);
            if (available <= 0)
            {
                break;
            }

            for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
            {
                if (widths[columnIndex] <= 0)
                {
                    continue;
                }

                var lines = rowLines[index][columnIndex];
                var slack = height - lines.Length;
                var lineOffset = Columns[columnIndex].VerticalAlignment switch
                {
                    VerticalAlignment.Bottom => slack,
                    VerticalAlignment.Middle => slack / 2,
                    _ => 0,
                };

                for (var li = 0; li < lines.Length; li++)
                {
                    var targetY = rowY + lineOffset + li;
                    if (targetY < rowY || targetY >= rowY + available)
                    {
                        continue;
                    }

                    RenderCell(
                        context, xOffsets[columnIndex], targetY,
                        widths[columnIndex], lines[li],
                        Columns[columnIndex].Alignment,
                        null);
                }
            }

            if (index == _selectedIndex && HighlightStyle != null)
            {
                context.SetStyle(
                    new Rectangle(viewport.Left, rowY, viewport.Width, available),
                    HighlightStyle);
            }

            rowY += available;
        }
    }

    private Text[] NormalizeCells(Text[] cells)
    {
        if (cells.Length == Columns.Count)
        {
            return cells;
        }

        var normalized = new Text[Columns.Count];
        for (var i = 0; i < Columns.Count; i++)
        {
            normalized[i] = i < cells.Length ? cells[i] : new Text();
        }

        return normalized;
    }

    private static TextLine[] ResolveCellLines(Text cell, TableColumn column, int width)
    {
        var source = cell?.Lines;
        if (source == null || source.Count == 0)
        {
            return [new TextLine()];
        }

        if (column.Wrap && width > 0)
        {
            return TextLineWrapper.Wrap(source, width).ToArray();
        }

        return source.ToArray();
    }

    private int[] MeasureAutoColumns(Text[][] rowCells)
    {
        var measurements = new int[Columns.Count];
        for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
        {
            if (Columns[columnIndex].Width.Kind != ColumnWidth.Mode.Auto)
            {
                continue;
            }

            var max = Columns[columnIndex].Header.GetWidth();
            foreach (var row in rowCells)
            {
                if (columnIndex >= row.Length)
                {
                    continue;
                }

                var text = row[columnIndex];
                if (text == null)
                {
                    continue;
                }

                foreach (var line in text.Lines)
                {
                    var width = line.GetWidth();
                    if (width > max)
                    {
                        max = width;
                    }
                }
            }

            measurements[columnIndex] = max;
        }

        return measurements;
    }

    private static void RenderCell(
        RenderContext context,
        int x,
        int y,
        int width,
        TextLine line,
        Justify alignment,
        Style? baseStyle)
    {
        var truncated = TextLineWrapper.TruncateWithEllipsis(line, width);
        var lineWidth = truncated.GetWidth();

        var offset = alignment switch
        {
            Justify.Right => Math.Max(0, width - lineWidth),
            Justify.Center => Math.Max(0, (width - lineWidth) / 2),
            _ => 0,
        };

        context.SetLine(x + offset, y, truncated, width - offset, baseStyle);
    }

    private (int FirstVisible, int LastVisible) GetVisibleBounds(int[] rowHeights, int viewportHeight)
    {
        var itemCount = rowHeights.Length;
        if (itemCount == 0 || viewportHeight <= 0)
        {
            return (0, 0);
        }

        var offset = Math.Clamp(_offset, 0, itemCount - 1);
        var selected = _selectedIndex ?? offset;
        selected = Math.Clamp(selected, 0, itemCount - 1);

        if (selected < offset)
        {
            offset = selected;
        }
        else
        {
            // Walk forward from the current offset, advancing it until the
            // selected row's full extent fits within the viewport height
            while (offset < selected && !FitsWithin(rowHeights, offset, selected, viewportHeight))
            {
                offset++;
            }
        }

        // Anchor to the bottom: if a longer tail would still fit, prefer to
        // show it instead of leaving empty space at the bottom of the viewport
        while (offset > 0 && FitsWithin(rowHeights, offset - 1, itemCount - 1, viewportHeight))
        {
            offset--;
        }

        var last = offset;
        var used = 0;
        while (last < itemCount && used + rowHeights[last] <= viewportHeight)
        {
            used += rowHeights[last];
            last++;
        }

        // Always render the offset row, even if it's taller than the viewport
        if (last == offset)
        {
            last = offset + 1;
        }

        return (offset, last);
    }

    private static bool FitsWithin(int[] rowHeights, int from, int to, int viewportHeight)
    {
        var sum = 0;
        for (var i = from; i <= to; i++)
        {
            sum += rowHeights[i];
            if (sum > viewportHeight)
            {
                return false;
            }
        }

        return true;
    }

    private TRow? GetSelectedItem()
    {
        if (_selectedIndex == null || Rows.Count == 0)
        {
            return default;
        }

        if (_selectedIndex < 0 || _selectedIndex > Rows.Count - 1)
        {
            return default;
        }

        return Rows[_selectedIndex.Value];
    }

    private void SetSelectedIndex(int? index)
    {
        if (index == null || Rows.Count == 0)
        {
            _selectedIndex = null;
            return;
        }

        if (WrapAround && Rows.Count > 1)
        {
            if (index >= Rows.Count)
            {
                _selectedIndex = 0;
                return;
            }

            if (index < 0)
            {
                _selectedIndex = Rows.Count - 1;
                return;
            }
        }

        _selectedIndex = Math.Clamp(index.Value, 0, Math.Max(0, Rows.Count - 1));
    }
}

[PublicAPI]
public static class TableWidgetExtensions
{
    extension<TRow>(TableWidget<TRow> widget)
        where TRow : ITableRow
    {
        public TableWidget<TRow> Rows(params IEnumerable<TRow> rows)
        {
            widget.Rows.Clear();
            widget.Rows.AddRange(rows);
            return widget;
        }

        public TableWidget<TRow> Columns(params IEnumerable<TableColumn> columns)
        {
            widget.Columns.Clear();
            widget.Columns.AddRange(columns);
            return widget;
        }

        public TableWidget<TRow> AddColumn(TableColumn column)
        {
            widget.Columns.Add(column);
            return widget;
        }

        public TableWidget<TRow> HighlightStyle(Style? style)
        {
            widget.HighlightStyle = style;
            return widget;
        }

        public TableWidget<TRow> HeaderStyle(Style? style)
        {
            widget.HeaderStyle = style;
            return widget;
        }

        public TableWidget<TRow> WrapAround(bool enable = true)
        {
            widget.WrapAround = enable;
            return widget;
        }

        public TableWidget<TRow> SelectedIndex(int? index)
        {
            widget.SelectedIndex = index;
            return widget;
        }

        public TableWidget<TRow> ShowHeader(bool show = true)
        {
            widget.ShowHeader = show;
            return widget;
        }

        public TableWidget<TRow> ShowSeparator(bool show = true)
        {
            widget.ShowSeparator = show;
            return widget;
        }

        public TableWidget<TRow> ColumnSpacing(int spacing)
        {
            widget.ColumnSpacing = Math.Max(0, spacing);
            return widget;
        }
    }

    extension<TRow>(TableWidget<TRow> widget)
        where TRow : ITableRow, ITableColumnDefinition
    {
        public TableWidget<TRow> AutoAddColumns()
        {
            widget.Columns.Clear();
            widget.Columns.AddRange(TRow.GetColumns());
            return widget;
        }
    }
}