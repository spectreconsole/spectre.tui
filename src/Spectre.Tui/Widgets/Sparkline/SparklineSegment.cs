namespace Spectre.Tui;

[PublicAPI]
public readonly record struct SparklineSegment(ulong? Value, Style? Style = null)
{
    public static implicit operator SparklineSegment(ulong value) => new(value);
    public static implicit operator SparklineSegment(ulong? value) => new(value);
}
