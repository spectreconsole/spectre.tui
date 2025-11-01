using System.Text;
using Shouldly;

namespace Spectre.Tui.Tests.Rendering;

public sealed class BufferTests
{
    [Fact]
    public void Should_Create_Empty_Buffer_Using_Region()
    {
        // Given, When
        var buffer = Buffer.Empty(new Region(1, 2, 10, 13));

        // Then
        buffer.Region.ShouldBe(new Region(1, 2, 10, 13));
        buffer.Length.ShouldBe(130);
    }

    [Fact]
    public void Should_Create_Empty_Buffer_Using_Size()
    {
        // Given, When
        var buffer = Buffer.Empty(new Size(11, 13));

        // Then
        buffer.Region.ShouldBe(new Region(0, 0, 11, 13));
        buffer.Length.ShouldBe(143);
    }

    [Fact]
    public void Should_Create_Filled_Buffer_Using_Region()
    {
        // Given, When
        var buffer = Buffer.Filled(
            new Region(1, 2, 10, 13),
            new Cell() { Rune = new Rune('x') });

        // Then
        buffer.Region.ShouldBe(new Region(1, 2, 10, 13));
        buffer.Length.ShouldBe(130);
        buffer.Cells[10].Rune.ShouldBe(new Rune('x'));
    }

    [Fact]
    public void Should_Create_Filled_Buffer_Using_Size()
    {
        // Given, When
        var buffer = Buffer.Filled(
            new Size(11, 13),
            new Cell() { Rune = new Rune('y') });

        // Then
        buffer.Region.ShouldBe(new Region(0, 0, 11, 13));
        buffer.Length.ShouldBe(143);
        buffer.Cells[12].Rune.ShouldBe(new Rune('y'));
    }

    [Fact]
    public void Should_Get_Cell_By_Coordinates()
    {
        // Given
        var buffer = Buffer.Filled(
            new Size(10, 10),
            new Cell() { Rune = new Rune('z') });

        // When
        var cell = buffer.GetCell(5, 5);

        // Then
        cell.Rune.ShouldBe(new Rune('z'));
    }

    [Fact]
    public void Should_Get_Cell_By_Index()
    {
        // Given
        var buffer = Buffer.Filled(
            new Size(10, 10),
            new Cell() { Rune = new Rune('z') });

        // When
        var cell = buffer.GetCell(10);

        // Then
        cell.Rune.ShouldBe(new Rune('z'));
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(15, 5)]
    [InlineData(5, 15)]
    public void Should_Return_Empty_Cell_If_Coordinate_Is_Out_Of_Bounds(int x, int y)
    {
        // Given
        var buffer = Buffer.Filled(
            new Size(10, 10),
            new Cell() { Rune = new Rune('z') });

        // When
        var cell = buffer.GetCell(x, y);

        // Then
        cell.Rune.ShouldBe(new Rune(' '));
    }

    [Fact]
    public void Should_Set_Cell()
    {
        // Given
        var buffer = Buffer.Empty(new Size(10, 10));

        // When
        buffer.SetCell(5, 5, new Cell { Rune = new Rune('x'), });

        // Then
        buffer.GetCell(5, 5).Rune
            .ShouldBe(new Rune('x'));
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(15, 5)]
    [InlineData(5, 15)]
    public void Should_Not_Throw_If_Setting_Cell_Out_Of_Bounds(int x, int y)
    {
        // Given
        var buffer = Buffer.Empty(new Size(10, 10));

        // When, Then
        buffer.SetCell(x, y, new Cell { Rune = new Rune('x'), });
    }
}