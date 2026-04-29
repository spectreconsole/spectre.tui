using System.Text;

namespace Spectre.Tui.Tests;

public sealed class ScrollViewWidgetTests
{
    private sealed class LinesWidget(int count, int width = 8) : IWidget
    {
        public void Render(RenderContext context)
        {
            for (var i = 0; i < count; i++)
            {
                var text = $"L{i:D2}".PadRight(width, '.');
                context.SetString(0, i, text);
            }
        }
    }

    private sealed class GridWidget(int width, int height) : IWidget
    {
        public void Render(RenderContext context)
        {
            for (var y = 0; y < height; y++)
            {
                var sb = new StringBuilder();
                for (var x = 0; x < width; x++)
                {
                    sb.Append((char)('A' + ((x + y) % 26)));
                }

                context.SetString(0, y, sb.ToString());
            }
        }
    }

    public sealed class ShortContent
    {
        [Fact]
        public void Should_Not_Render_Scrollbar_When_Content_Fits()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget().Inner(new LinesWidget(4));

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L00.....••
                L01.....••
                L02.....••
                L03.....••
                ••••••••••
                ••••••••••
                """);
        }

        [Fact]
        public void Should_Render_Scrollbar_When_Visibility_Is_Always()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(4))
                .VerticalScroll(ScrollMode.Always);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L00.....•▲
                L01.....•█
                L02.....•█
                L03.....•█
                •••••••••█
                •••••••••▼
                """);
        }
    }

    public sealed class TallContent
    {
        [Fact]
        public void Should_Render_Top_Of_Content_At_Offset_Zero()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget().Inner(new LinesWidget(12));

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L00.....•▲
                L01.....•█
                L02.....•█
                L03.....•║
                L04.....•║
                L05.....•▼
                """);
        }

        [Fact]
        public void Should_Render_Middle_Of_Content_At_Mid_Offset()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(12))
                .VerticalOffset(3);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L03.....•▲
                L04.....•║
                L05.....•█
                L06.....•█
                L07.....•║
                L08.....•▼
                """);
        }

        [Fact]
        public void Should_Render_Bottom_Of_Content_At_Max_Offset()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(12))
                .VerticalOffset(6);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L06.....•▲
                L07.....•║
                L08.....•║
                L09.....•█
                L10.....•█
                L11.....•▼
                """);
        }

        [Fact]
        public void Should_Clamp_Offset_Past_End()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(12))
                .VerticalOffset(999);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L06.....•▲
                L07.....•║
                L08.....•║
                L09.....•█
                L10.....•█
                L11.....•▼
                """);
        }
    }

    public sealed class WideContent
    {
        [Fact]
        public void Should_Render_Horizontal_Scrollbar_At_Bottom_When_Wide_Content_Overflows()
        {
            // Given
            var fixture = new TuiFixture(new Size(6, 4));
            var panel = new ScrollViewWidget().Inner(new GridWidget(12, 2));

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                ABCDEF
                BCDEFG
                ••••••
                ◄██══►
                """);
        }

        [Fact]
        public void Should_Shift_Visible_Window_With_Horizontal_Offset()
        {
            // Given
            var fixture = new TuiFixture(new Size(6, 4));
            var panel = new ScrollViewWidget()
                .Inner(new GridWidget(12, 2))
                .HorizontalOffset(6);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                GHIJKL
                HIJKLM
                ••••••
                ◄══██►
                """);
        }
    }

    public sealed class BothAxes
    {
        [Fact]
        public void Should_Leave_Corner_Cell_Empty_When_Both_Bars_Visible()
        {
            // Given
            var fixture = new TuiFixture(new Size(6, 6));
            var panel = new ScrollViewWidget().Inner(new GridWidget(10, 10));

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                ABCDE▲
                BCDEF█
                CDEFG█
                DEFGH║
                EFGHI▼
                ◄██═►•
                """);
        }
    }

    public sealed class Visibility
    {
        [Fact]
        public void Should_Not_Reserve_Column_When_Hidden_Even_If_Content_Overflows()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(12))
                .VerticalScroll(ScrollMode.Hidden);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L00.....••
                L01.....••
                L02.....••
                L03.....••
                L04.....••
                L05.....••
                """);
        }

        [Fact]
        public void Should_Honour_Vertical_Offset_When_Mode_Is_Hidden()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(12))
                .VerticalScroll(ScrollMode.Hidden)
                .VerticalOffset(5);

            // When
            var result = fixture.Render(panel);

            // Then
            panel.VerticalOffset.ShouldBe(5);
            result.ShouldBe(
                """
                L05.....••
                L06.....••
                L07.....••
                L08.....••
                L09.....••
                L10.....••
                """);
        }

        [Fact]
        public void Should_Force_Offset_To_Zero_When_Mode_Is_Disabled()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(12))
                .VerticalScroll(ScrollMode.Disabled)
                .VerticalOffset(5);

            // When
            var result = fixture.Render(panel);

            // Then
            panel.VerticalOffset.ShouldBe(0);
            result.ShouldBe(
                """
                L00.....••
                L01.....••
                L02.....••
                L03.....••
                L04.....••
                L05.....••
                """);
        }

        [Fact]
        public void Should_Suppress_Horizontal_Bar_When_Visibility_Is_Hidden()
        {
            // Given
            var fixture = new TuiFixture(new Size(6, 4));
            var panel = new ScrollViewWidget()
                .Inner(new GridWidget(12, 2))
                .HorizontalScroll(ScrollMode.Hidden);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                ABCDEF
                BCDEFG
                ••••••
                ••••••
                """);
        }
    }

    public sealed class ContentSizeOverride
    {
        [Fact]
        public void Should_Treat_ContentSize_As_Authoritative_For_Scrollbar()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget()
                .Inner(new LinesWidget(4))
                .ContentSize(8, 12);

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                L00.....•▲
                L01.....•█
                L02.....•█
                L03.....•║
                •••••••••║
                •••••••••▼
                """);
        }
    }

    public sealed class Movement
    {
        [Fact]
        public void Should_ScrollDown_Increment_Vertical_Offset()
        {
            // Given
            var panel = new ScrollViewWidget();

            // When
            panel.ScrollDown(3);

            // Then
            panel.VerticalOffset.ShouldBe(3);
        }

        [Fact]
        public void Should_ScrollUp_Clamp_To_Zero()
        {
            // Given
            var panel = new ScrollViewWidget { VerticalOffset = 2 };

            // When
            panel.ScrollUp(5);

            // Then
            panel.VerticalOffset.ShouldBe(0);
        }

        [Fact]
        public void Should_ScrollToBottom_Set_Sentinel_That_Render_Clamps()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget().Inner(new LinesWidget(12));

            // When
            panel.ScrollToBottom();
            var result = fixture.Render(panel);

            // Then
            panel.VerticalOffset.ShouldBe(6);
            result.ShouldBe(
                """
                L06.....•▲
                L07.....•║
                L08.....•║
                L09.....•█
                L10.....•█
                L11.....•▼
                """);
        }

        [Fact]
        public void Should_PageDown_Advance_By_Last_Page_Height()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var panel = new ScrollViewWidget().Inner(new LinesWidget(20));
            fixture.Render(panel);

            // When
            panel.PageDown();

            // Then
            panel.VerticalOffset.ShouldBe(6);
        }

        [Fact]
        public void Should_PageDown_Before_First_Render_Move_By_One()
        {
            // Given
            var panel = new ScrollViewWidget();

            // When
            panel.PageDown();

            // Then
            panel.VerticalOffset.ShouldBe(1);
        }
    }

    public sealed class EdgeCases
    {
        [Fact]
        public void Should_Not_Crash_With_Empty_Viewport()
        {
            // Given
            var fixture = new TuiFixture(new Size(0, 0));
            var panel = new ScrollViewWidget().Inner(new LinesWidget(12));

            // When
            var act = () => fixture.Render(panel);

            // Then
            act.ShouldNotThrow();
        }

        [Fact]
        public void Should_Render_Empty_When_Inner_Is_Null()
        {
            // Given
            var fixture = new TuiFixture(new Size(6, 3));
            var panel = new ScrollViewWidget();

            // When
            var result = fixture.Render(panel);

            // Then
            result.ShouldBe(
                """
                ••••••
                ••••••
                ••••••
                """);
        }
    }

    public sealed class Composition
    {
        [Fact]
        public void Should_Render_Inside_BoxWidget_With_Scrollbar_Inside_Border()
        {
            // Given
            var fixture = new TuiFixture(new Size(10, 6));
            var box = new BoxWidget()
                .Inner(new ScrollViewWidget().Inner(new LinesWidget(8, 5)));

            // When
            var result = fixture.Render(box);

            // Then
            result.ShouldBe(
                """
                ╭────────╮
                │L00..••▲│
                │L01..••█│
                │L02..••║│
                │L03..••▼│
                ╰────────╯
                """);
        }
    }
}
