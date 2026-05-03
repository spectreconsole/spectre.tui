namespace Spectre.Tui;

[PublicAPI]
public sealed class TextBoxWidget : IWidget
{
    private readonly TextBoxBuffer _buffer = new();
    private int _cursorRow;
    private int _cursorColumn;
    private int _desiredColumn;
    private int _horizontalOffset;
    private int _verticalOffset;

    public TextBoxMode Mode { get; set; } = TextBoxMode.SingleLine;

    public string Text
    {
        get => _buffer.GetText();
        set
        {
            _buffer.SetText(value);
            ClampCursor();
            _desiredColumn = _cursorColumn;
        }
    }

    public string? Placeholder { get; set; }
    public Style? Style { get; set; }
    public Style? PlaceholderStyle { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsFocused { get; set; }
    public int? MaxLength { get; set; }
    public char? PasswordChar { get; set; }

    public TextBoxPosition Cursor => new(_cursorRow, _cursorColumn);
    public int Length => _buffer.Length;

    public void Insert(string text)
    {
        if (IsReadOnly || string.IsNullOrEmpty(text))
        {
            return;
        }

        var normalized = text.NormalizeNewLines();

        foreach (var grapheme in normalized.Graphemes())
        {
            if (grapheme == "\n")
            {
                if (Mode == TextBoxMode.SingleLine)
                {
                    continue;
                }

                _buffer.SplitLine(_cursorRow, _cursorColumn);
                _cursorRow++;
                _cursorColumn = 0;
                continue;
            }

            // Surrogate pairs and ZWJ sequences arrive across multiple Insert calls — merge so the buffer holds whole graphemes.
            if (_cursorColumn > 0 && TryCombineWithPrevious(grapheme))
            {
                continue;
            }

            if (MaxLength.HasValue && _buffer.Length >= MaxLength.Value)
            {
                break;
            }

            _buffer.InsertGrapheme(_cursorRow, _cursorColumn, grapheme);
            _cursorColumn++;
        }

        _desiredColumn = _cursorColumn;
    }

    public void InsertNewLine()
    {
        if (IsReadOnly || Mode == TextBoxMode.SingleLine)
        {
            return;
        }

        _buffer.SplitLine(_cursorRow, _cursorColumn);
        _cursorRow++;
        _cursorColumn = 0;
        _desiredColumn = 0;
    }

    public void DeleteBackward()
    {
        if (IsReadOnly)
        {
            return;
        }

        if (_cursorColumn > 0)
        {
            _cursorColumn--;
            _buffer.DeleteGrapheme(_cursorRow, _cursorColumn);
        }
        else if (_cursorRow > 0)
        {
            var prevLen = _buffer.GetLineLength(_cursorRow - 1);
            _buffer.JoinNextLine(_cursorRow - 1);
            _cursorRow--;
            _cursorColumn = prevLen;
        }

        _desiredColumn = _cursorColumn;
    }

    public void DeleteForward()
    {
        if (IsReadOnly)
        {
            return;
        }

        if (_cursorColumn < _buffer.GetLineLength(_cursorRow))
        {
            _buffer.DeleteGrapheme(_cursorRow, _cursorColumn);
        }
        else if (_cursorRow < _buffer.LineCount - 1)
        {
            _buffer.JoinNextLine(_cursorRow);
        }
    }

    public void Clear()
    {
        if (IsReadOnly)
        {
            return;
        }

        _buffer.Clear();
        _cursorRow = 0;
        _cursorColumn = 0;
        _desiredColumn = 0;
        _horizontalOffset = 0;
        _verticalOffset = 0;
    }

    public void MoveLeft(int count = 1)
    {
        if (count <= 0)
        {
            return;
        }

        for (var i = 0; i < count; i++)
        {
            if (_cursorColumn > 0)
            {
                _cursorColumn--;
            }
            else if (_cursorRow > 0)
            {
                _cursorRow--;
                _cursorColumn = _buffer.GetLineLength(_cursorRow);
            }
        }

        _desiredColumn = _cursorColumn;
    }

    public void MoveRight(int count = 1)
    {
        if (count <= 0)
        {
            return;
        }

        for (var i = 0; i < count; i++)
        {
            if (_cursorColumn < _buffer.GetLineLength(_cursorRow))
            {
                _cursorColumn++;
            }
            else if (_cursorRow < _buffer.LineCount - 1)
            {
                _cursorRow++;
                _cursorColumn = 0;
            }
            else
            {
                break;
            }
        }

        _desiredColumn = _cursorColumn;
    }

    public void MoveUp(int count = 1)
    {
        if (count <= 0)
        {
            return;
        }

        if (Mode == TextBoxMode.SingleLine || _cursorRow == 0)
        {
            return;
        }

        for (var i = 0; i < count; i++)
        {
            _cursorRow--;
            _cursorColumn = Math.Min(_desiredColumn, _buffer.GetLineLength(_cursorRow));
        }
    }

    public void MoveDown(int count = 1)
    {
        if (count <= 0)
        {
            return;
        }

        if (Mode == TextBoxMode.SingleLine || _cursorRow >= _buffer.LineCount - 1)
        {
            return;
        }

        for (var i = 0; i < count; i++)
        {
            _cursorRow++;
            _cursorColumn = Math.Min(_desiredColumn, _buffer.GetLineLength(_cursorRow));
        }
    }

    public void MoveHome()
    {
        _cursorColumn = 0;
        _desiredColumn = 0;
    }

    public void MoveEnd()
    {
        _cursorColumn = _buffer.GetLineLength(_cursorRow);
        _desiredColumn = _cursorColumn;
    }

    public void MoveToStart()
    {
        _cursorRow = 0;
        _cursorColumn = 0;
        _desiredColumn = 0;
    }

    public void MoveToEnd()
    {
        _cursorRow = _buffer.LineCount - 1;
        _cursorColumn = _buffer.GetLineLength(_cursorRow);
        _desiredColumn = _cursorColumn;
    }

    void IWidget.Render(RenderContext context)
    {
        var width = context.Viewport.Width;
        var height = context.Viewport.Height;
        if (width <= 0 || height <= 0)
        {
            return;
        }

        ClampCursor();

        if (Mode == TextBoxMode.SingleLine)
        {
            RenderSingleLine(context, width);
        }
        else
        {
            RenderMultiLine(context, width, height);
        }
    }

    private bool TryCombineWithPrevious(string grapheme)
    {
        var line = _buffer.GetLine(_cursorRow);
        var previous = line[_cursorColumn - 1];
        var combined = previous + grapheme;
        var firstGrapheme = combined.Graphemes().First();
        if (firstGrapheme.Length != combined.Length)
        {
            return false;
        }

        _buffer.DeleteGrapheme(_cursorRow, _cursorColumn - 1);
        _buffer.InsertGrapheme(_cursorRow, _cursorColumn - 1, firstGrapheme);
        return true;
    }

    private void ClampCursor()
    {
        if (_cursorRow < 0)
        {
            _cursorRow = 0;
        }

        if (_cursorRow >= _buffer.LineCount)
        {
            _cursorRow = _buffer.LineCount - 1;
        }

        var lineLen = _buffer.GetLineLength(_cursorRow);
        if (_cursorColumn < 0)
        {
            _cursorColumn = 0;
        }

        if (_cursorColumn > lineLen)
        {
            _cursorColumn = lineLen;
        }
    }

    private int CellWidthOf(string grapheme)
    {
        if (PasswordChar.HasValue)
        {
            var width = PasswordChar.Value.GetCellWidth();
            return width < 0 ? 0 : width;
        }
        else
        {
            var width = grapheme.GetCellWidth();
            return width < 0 ? 0 : width;
        }
    }

    private int CellPositionInLine(int row, int column)
    {
        var line = _buffer.GetLine(row);
        var pos = 0;
        for (var i = 0; i < column && i < line.Count; i++)
        {
            pos += CellWidthOf(line[i]);
        }

        return pos;
    }

    private void RenderSingleLine(RenderContext context, int width)
    {
        var line = _buffer.GetLine(0);

        if (line.Count == 0)
        {
            if (!string.IsNullOrEmpty(Placeholder))
            {
                var style = PlaceholderStyle ?? new Style(decoration: Decoration.Dim);
                context.SetString(0, 0, Placeholder, style, width);
            }

            if (IsFocused)
            {
                context.SetCursorPosition(0, 0);
            }

            return;
        }

        var cursorCellPos = CellPositionInLine(0, _cursorColumn);

        if (cursorCellPos < _horizontalOffset)
        {
            _horizontalOffset = cursorCellPos;
        }

        if (cursorCellPos >= _horizontalOffset + width)
        {
            _horizontalOffset = cursorCellPos - width + 1;
        }

        if (_horizontalOffset < 0)
        {
            _horizontalOffset = 0;
        }

        RenderGraphemeRow(context, line, 0, _horizontalOffset, width);

        var cursorScreenX = cursorCellPos - _horizontalOffset;
        if (IsFocused)
        {
            context.SetCursorPosition(cursorScreenX, 0);
        }
    }

    private void RenderMultiLine(RenderContext context, int width, int height)
    {
        if (_cursorRow < _verticalOffset)
        {
            _verticalOffset = _cursorRow;
        }

        if (_cursorRow >= _verticalOffset + height)
        {
            _verticalOffset = _cursorRow - height + 1;
        }

        if (_verticalOffset < 0)
        {
            _verticalOffset = 0;
        }

        var cursorCellPos = CellPositionInLine(_cursorRow, _cursorColumn);
        if (cursorCellPos < _horizontalOffset)
        {
            _horizontalOffset = cursorCellPos;
        }

        if (cursorCellPos >= _horizontalOffset + width)
        {
            _horizontalOffset = cursorCellPos - width + 1;
        }

        if (_horizontalOffset < 0)
        {
            _horizontalOffset = 0;
        }

        if (_buffer.LineCount == 1 && _buffer.GetLineLength(0) == 0 && !string.IsNullOrEmpty(Placeholder))
        {
            var style = PlaceholderStyle ?? new Style(decoration: Decoration.Dim);
            context.SetString(0, 0, Placeholder, style, width);
        }
        else
        {
            var lastRow = Math.Min(_buffer.LineCount, _verticalOffset + height);
            for (var row = _verticalOffset; row < lastRow; row++)
            {
                var screenY = row - _verticalOffset;
                RenderGraphemeRow(context, _buffer.GetLine(row), screenY, _horizontalOffset, width);
            }
        }

        var cursorScreenX = cursorCellPos - _horizontalOffset;
        var cursorScreenY = _cursorRow - _verticalOffset;
        if (IsFocused &&
            cursorScreenX >= 0 && cursorScreenX < width &&
            cursorScreenY >= 0 && cursorScreenY < height)
        {
            context.SetCursorPosition(cursorScreenX, cursorScreenY);
        }
    }

    private void RenderGraphemeRow(
        RenderContext context, List<string> graphemes,
        int screenY, int horizontalOffset, int width)
    {
        if (graphemes.Count == 0)
        {
            return;
        }

        var builder = new StringBuilder();
        var skip = horizontalOffset;
        foreach (var grapheme in graphemes)
        {
            var displayed = PasswordChar.HasValue ? PasswordChar.Value.ToString() : grapheme;
            var cellWidth = CellWidthOf(grapheme);

            if (cellWidth == 0)
            {
                builder.Append(displayed);
                continue;
            }

            if (skip > 0)
            {
                if (skip >= cellWidth)
                {
                    skip -= cellWidth;
                    continue;
                }

                skip = 0;
                continue;
            }

            builder.Append(displayed);
        }

        if (builder.Length > 0)
        {
            context.SetString(0, screenY, builder.ToString(), Style, width);
        }
    }
}

[PublicAPI]
public static class TextBoxWidgetExtensions
{
    extension(TextBoxWidget widget)
    {
        public TextBoxWidget Mode(TextBoxMode mode)
        {
            widget.Mode = mode;
            return widget;
        }

        public TextBoxWidget AsSingleLine()
        {
            widget.Mode = TextBoxMode.SingleLine;
            return widget;
        }

        public TextBoxWidget AsMultiLine()
        {
            widget.Mode = TextBoxMode.MultiLine;
            return widget;
        }

        public TextBoxWidget Text(string text)
        {
            widget.Text = text;
            return widget;
        }

        public TextBoxWidget Placeholder(string? placeholder)
        {
            widget.Placeholder = placeholder;
            return widget;
        }

        public TextBoxWidget MaxLength(int? max)
        {
            widget.MaxLength = max;
            return widget;
        }

        public TextBoxWidget ReadOnly(bool readOnly = true)
        {
            widget.IsReadOnly = readOnly;
            return widget;
        }

        public TextBoxWidget PasswordChar(char? character)
        {
            widget.PasswordChar = character;
            return widget;
        }

        public TextBoxWidget Password()
        {
            widget.PasswordChar = '•';
            return widget;
        }

        public TextBoxWidget Style(Style? style)
        {
            widget.Style = style;
            return widget;
        }

        public TextBoxWidget PlaceholderStyle(Style? style)
        {
            widget.PlaceholderStyle = style;
            return widget;
        }
    }
}