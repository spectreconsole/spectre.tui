namespace Spectre.Tui;

internal static class TableLayout
{
    public static int[] CalculateColumnWidths(
        IReadOnlyList<TableColumn> columns,
        IReadOnlyList<int> autoMeasurements,
        int totalWidth,
        int columnSpacing)
    {
        var count = columns.Count;
        var widths = new int[count];
        if (count == 0 || totalWidth <= 0)
        {
            return widths;
        }

        var spacingTotal = Math.Max(0, count - 1) * Math.Max(0, columnSpacing);
        var available = Math.Max(0, totalWidth - spacingTotal);

        // Phase 1: assign Fixed and Auto widths
        var rigidTotal = 0;
        for (var i = 0; i < count; i++)
        {
            var column = columns[i];
            switch (column.Width.Kind)
            {
                case ColumnWidth.Mode.Fixed:
                    widths[i] = column.Width.Value;
                    rigidTotal += widths[i];
                    break;
                case ColumnWidth.Mode.Auto:
                    widths[i] = autoMeasurements[i];
                    rigidTotal += widths[i];
                    break;
                case ColumnWidth.Mode.Star:
                    widths[i] = 0;
                    break;
            }
        }

        // Phase 2: shrink rigid columns proportionally if they overflow
        if (rigidTotal > available)
        {
            var overflow = rigidTotal - available;
            for (var i = 0; i < count && overflow > 0; i++)
            {
                if (widths[i] <= 0)
                {
                    continue;
                }

                var share = (int)Math.Ceiling((double)overflow * widths[i] / rigidTotal);
                var shrink = Math.Min(widths[i], share);
                widths[i] -= shrink;
                overflow -= shrink;
            }

            return widths;
        }

        // Phase 3: distribute remaining space across weighted (star) columns
        var remaining = available - rigidTotal;
        if (remaining <= 0)
        {
            return widths;
        }

        var totalWeight = 0;
        for (var i = 0; i < count; i++)
        {
            if (columns[i].Width.Kind == ColumnWidth.Mode.Star)
            {
                totalWeight += columns[i].Width.Value;
            }
        }

        if (totalWeight == 0)
        {
            return widths;
        }

        // Largest-remainder rounding so the totals add up exactly
        var fractions = new (int Index, int Floor, double Remainder)[count];
        var fractionCount = 0;
        var distributed = 0;
        for (var i = 0; i < count; i++)
        {
            if (columns[i].Width.Kind != ColumnWidth.Mode.Star)
            {
                continue;
            }

            var raw = (double)remaining * columns[i].Width.Value / totalWeight;
            var floor = (int)Math.Floor(raw);
            widths[i] = floor;
            distributed += floor;
            fractions[fractionCount++] = (i, floor, raw - floor);
        }

        var leftover = remaining - distributed;
        if (leftover <= 0)
        {
            return widths;
        }

        Array.Sort(fractions, 0, fractionCount, RemainderComparer.Shared);
        for (var i = 0; i < leftover && i < fractionCount; i++)
        {
            widths[fractions[i].Index]++;
        }

        return widths;
    }

    private sealed class RemainderComparer : IComparer<(int Index, int Floor, double Remainder)>
    {
        public static readonly RemainderComparer Shared = new();

        public int Compare(
            (int Index, int Floor, double Remainder) x,
            (int Index, int Floor, double Remainder) y)
        {
            // Larger remainder first
            return y.Remainder.CompareTo(x.Remainder);
        }
    }
}
