namespace Spectre.Tui;

internal sealed class TextBoxBuffer
{
    private readonly List<List<string>> _lines = [[]];

    public int LineCount => _lines.Count;
    public int Length { get; private set; }

    public List<string> GetLine(int row)
    {
        EnsureRow(row);
        return _lines[row];
    }

    public int GetLineLength(int row)
    {
        EnsureRow(row);
        return _lines[row].Count;
    }

    public void Clear()
    {
        _lines.Clear();
        _lines.Add([]);
        Length = 0;
    }

    public void SetText(string? text)
    {
        _lines.Clear();
        if (string.IsNullOrEmpty(text))
        {
            _lines.Add([]);
            Length = 0;
            return;
        }

        var normalized = text.NormalizeNewLines();

        var total = 0;
        var current = new List<string>();
        foreach (var grapheme in normalized.Graphemes())
        {
            if (grapheme == "\n")
            {
                _lines.Add(current);
                current = [];
                continue;
            }

            current.Add(grapheme);
            total++;
        }

        _lines.Add(current);
        Length = total;
    }

    public string GetText()
    {
        if (_lines.Count == 1)
        {
            return string.Concat(_lines[0]);
        }

        var builder = new StringBuilder();
        for (var i = 0; i < _lines.Count; i++)
        {
            if (i > 0)
            {
                builder.Append('\n');
            }

            foreach (var g in _lines[i])
            {
                builder.Append(g);
            }
        }

        return builder.ToString();
    }

    public void InsertGrapheme(int row, int col, string grapheme)
    {
        EnsureRow(row);
        ArgumentOutOfRangeException.ThrowIfNegative(col);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(col, _lines[row].Count);
        ArgumentException.ThrowIfNullOrEmpty(grapheme);

        _lines[row].Insert(col, grapheme);
        Length++;
    }

    public void DeleteGrapheme(int row, int col)
    {
        EnsureRow(row);
        ArgumentOutOfRangeException.ThrowIfNegative(col);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(col, _lines[row].Count);

        _lines[row].RemoveAt(col);
        Length--;
    }

    public void SplitLine(int row, int col)
    {
        EnsureRow(row);
        ArgumentOutOfRangeException.ThrowIfNegative(col);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(col, _lines[row].Count);

        var current = _lines[row];
        var tail = current.GetRange(col, current.Count - col);
        current.RemoveRange(col, current.Count - col);
        _lines.Insert(row + 1, tail);
    }

    public void JoinNextLine(int row)
    {
        EnsureRow(row);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(row, _lines.Count - 1);

        var next = _lines[row + 1];
        _lines[row].AddRange(next);
        _lines.RemoveAt(row + 1);
    }

    private void EnsureRow(int row, [CallerArgumentExpression(nameof(row))] string? paramName = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(row, paramName);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(row, _lines.Count, paramName);
    }
}
