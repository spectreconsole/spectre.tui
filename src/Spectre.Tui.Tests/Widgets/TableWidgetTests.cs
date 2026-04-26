namespace Spectre.Tui.Tests;

public sealed class TableWidgetTests
{
    private sealed class TestRow : TableRow
    {
        private readonly Text[] _cells;

        public TestRow(params string[] cells)
        {
            _cells = [.. cells.Select(c => Text.FromString(c))];
        }

        public TestRow(params Text[] cells)
        {
            _cells = cells;
        }

        protected override Text[] CreateCells(bool isSelected)
        {
            return _cells;
        }
    }

    [Fact]
    public void Should_Render_Header_Separator_And_Rows()
    {
        // Given
        var fixture = new TuiFixture(new Size(28, 4));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("Rank"),
                new TableColumn("City"),
                new TableColumn("Country"))
            .Rows(
                new TestRow("6", "Mexico City", "Mexico"),
                new TestRow("7", "Cairo", "Egypt"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            Rank••City•••••••••Country••
            ────────────────────────────
            6•••••Mexico•City••Mexico•••
            7•••••Cairo••••••••Egypt••••
            """);
    }

    [Fact]
    public void Should_Right_Align_Column()
    {
        // Given
        var fixture = new TuiFixture(new Size(22, 4));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("Rank"),
                new TableColumn("Population")
                {
                    Width = ColumnWidth.Fixed(15),
                    Alignment = Justify.Right,
                })
            .Rows(
                new TestRow("6", "22,085,140"),
                new TestRow("7", "21,750,020"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            Rank•••••••Population•
            ──────────────────────
            6••••••••••22,085,140•
            7••••••••••21,750,020•
            """);
    }

    [Fact]
    public void Should_Truncate_Cells_With_Ellipsis()
    {
        // Given
        var fixture = new TuiFixture(new Size(18, 4));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("Rank"),
                new TableColumn("City")
                {
                    Width = ColumnWidth.Fixed(10)
                })
            .Rows(
                new TestRow("6", "Mexico City"),
                new TestRow("7", "Cairo"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            Rank••City••••••••
            ──────────────────
            6•••••Mexico•Ci…••
            7•••••Cairo•••••••
            """);
    }

    [Fact]
    public void Should_Distribute_Star_Width_Across_Columns()
    {
        // Given
        var fixture = new TuiFixture(new Size(12, 3));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("L")
                {
                    Width = ColumnWidth.Star(1),
                },
                new TableColumn("R")
                {
                    Width = ColumnWidth.Star(1),
                })
            .Rows(
                new TestRow("a", "b"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            L••••••R••••
            ────────────
            a••••••b••••
            """);
    }

    [Fact]
    public void Should_Honor_Fixed_Column_Width()
    {
        // Given
        var fixture = new TuiFixture(new Size(15, 3));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("L")
                {
                    Width = ColumnWidth.Fixed(3),
                },
                new TableColumn("R")
                {
                    Width = ColumnWidth.Fixed(5),
                })
            .Rows(
                new TestRow("a", "b"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            L••••R•••••••••
            ───────────────
            a••••b•••••••••
            """);
    }

    [Fact]
    public void Should_Hide_Header_When_ShowHeader_Is_False()
    {
        // Given
        var fixture = new TuiFixture(new Size(6, 3));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("L"),
                new TableColumn("R"))
            .Rows(
                new TestRow("a", "b"),
                new TestRow("c", "d"))
            .ShowHeader(false);

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            a••b••
            c••d••
            ••••••
            """);
    }

    [Fact]
    public void Should_Hide_Separator_When_ShowSeparator_Is_False()
    {
        // Given
        var fixture = new TuiFixture(new Size(6, 3));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("L"),
                new TableColumn("R"))
            .Rows(
                new TestRow("a", "b"),
                new TestRow("c", "d"))
            .ShowSeparator(false);

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            L••R••
            a••b••
            c••d••
            """);
    }

    [Fact]
    public void Should_Scroll_When_Selection_Is_Past_Visible_Area()
    {
        // Given
        var fixture = new TuiFixture(new Size(8, 5));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("#"),
                new TableColumn("Name"))
            .Rows(
                new TestRow("1", "A"),
                new TestRow("2", "B"),
                new TestRow("3", "C"),
                new TestRow("4", "D"),
                new TestRow("5", "E"))
            .SelectedIndex(4);

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            #••Name•
            ────────
            3••C••••
            4••D••••
            5••E••••
            """);
    }

    [Fact]
    public void Should_Wrap_Around_When_Enabled()
    {
        // Given
        var table = new TableWidget<TestRow>()
            .Columns(new TableColumn("L"))
            .Rows(
                new TestRow("a"),
                new TestRow("b"),
                new TestRow("c"))
            .WrapAround()
            .SelectedIndex(0);

        // When
        table.MoveUp();

        // Then
        table.SelectedIndex.ShouldBe(2);
    }

    [Fact]
    public void Should_Render_Without_Crashing_When_Rows_Are_Empty()
    {
        // Given
        var fixture = new TuiFixture(new Size(6, 3));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("L"),
                new TableColumn("R"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            L••R••
            ──────
            ••••••
            """);
    }

    [Fact]
    public void Should_Render_Multi_Line_Cells_From_Explicit_Lines()
    {
        // Given
        var fixture = new TuiFixture(new Size(14, 5));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("#"),
                new TableColumn("Body"))
            .Rows(
                new TestRow("1", "single"),
                new TestRow("2", new Text([
                    TextLine.FromString("line one"),
                    TextLine.FromString("line two"),
                ])));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            #••Body•••••••
            ──────────────
            1••single•••••
            2••line•one•••
            •••line•two•••
            """);
    }

    [Fact]
    public void Should_Soft_Wrap_When_Column_Wrap_Is_Enabled()
    {
        // Given
        var fixture = new TuiFixture(new Size(14, 5));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("#"),
                new TableColumn("Note").FixedWidth(8).WrapText())
            .Rows(
                new TestRow("1", Text.FromMarkup("alpha beta gamma")));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            #••Note•••••••
            ──────────────
            1••alpha••••••
            •••beta•••••••
            •••gamma••••••
            """);
    }

    [Fact]
    public void Should_Vertically_Align_Bottom_When_Configured()
    {
        // Given
        var fixture = new TuiFixture(new Size(14, 5));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("Tag")
                {
                    VerticalAlignment = VerticalAlignment.Bottom
                },
                new TableColumn("Body"))
            .Rows(
                new TestRow("X", "a\nb\nc"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            Tag••Body•••••
            ──────────────
            •••••a••••••••
            •••••b••••••••
            X••••c••••••••
            """);
    }

    [Fact]
    public void Should_Vertically_Center_When_Configured()
    {
        // Given
        var fixture = new TuiFixture(new Size(14, 5));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("Tag")
                {
                    VerticalAlignment = VerticalAlignment.Middle
                },
                new TableColumn("Body"))
            .Rows(
                new TestRow("X", "a\nb\nc"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            Tag••Body•••••
            ──────────────
            •••••a••••••••
            X••••b••••••••
            •••••c••••••••
            """);
    }

    [Fact]
    public void Should_Account_For_Tall_Rows_When_Scrolling()
    {
        // Given
        var fixture = new TuiFixture(new Size(10, 5));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("#"),
                new TableColumn("Body"))
            .Rows(
                new TestRow("1", "one"),
                new TestRow("2", "aa\nbb"),
                new TestRow("3", "three"),
                new TestRow("4", "four"))
            .SelectedIndex(3);

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            #••Body•••
            ──────────
            3••three••
            4••four•••
            ••••••••••
            """);
    }

    [Fact]
    public void Should_Auto_Measure_Widest_Line_In_Multi_Line_Cells()
    {
        // Given
        var fixture = new TuiFixture(new Size(14, 4));
        var table = new TableWidget<TestRow>()
            .Columns(
                new TableColumn("Body"),
                new TableColumn("End"))
            .Rows(
                new TestRow("a\nlongest", "X"));

        // When
        var result = fixture.Render(table);

        // Then
        result.ShouldBe(
            """
            Body•••••End••
            ──────────────
            a••••••••X••••
            longest•••••••
            """);
    }

    [Fact]
    public void Should_Move_Selection_With_MoveDown_And_MoveUp()
    {
        // Given
        var table = new TableWidget<TestRow>()
            .Columns(new TableColumn("L"))
            .Rows(
                new TestRow("a"),
                new TestRow("b"),
                new TestRow("c"))
            .SelectedIndex(0);

        // When
        table.MoveDown();
        table.MoveDown();
        table.MoveUp();

        // Then
        table.SelectedIndex.ShouldBe(1);
    }
}