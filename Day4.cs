static partial class Aoc2024
{
    public static void Day4()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                MMMSXXMASM
                MSAMXMSMSA
                AMXSXMAAMM
                MSAMASMSMX
                XMASAMXAMM
                XXAMMXXAMA
                SMSMSASXSS
                SAXAMASAAA
                MAMMMXMMMM
                MXMXAXMASX
                """.ToLines();
            Part1(input).Should().Be(18);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(2507);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => Count(lines, "XMAS");
        int Part2(string[] lines) => 0;

        static int Count(string[] lines, string target) =>
            CountWord(TraverseHorizontal(lines), target)
            + CountWord(TraverseVertical(lines), target)
            + CountWord(TraverseDiagonal(lines), target)
            + CountWord(TraverseDiagonalBackwards(lines), target)
            + CountWord(TraverseHorizontal(lines), target.ReverseString())
            + CountWord(TraverseVertical(lines), target.ReverseString())
            + CountWord(TraverseDiagonal(lines), target.ReverseString())
            + CountWord(TraverseDiagonalBackwards(lines), target.ReverseString())
            ;

        static int CountWord(IEnumerable<char> chars, string target)
        {
            var count = 0;
            var index = 0;

            foreach (var c in chars)
            {
                if (c == target[index])
                {
                    index++;
                    if (index == target.Length)
                    {
                        count++;
                        index = 0;
                    }
                }
                else
                {
                    index = 0;
                    if (c == target[0])
                    {
                        index++;
                    }
                }
            }

            return count;
        }

        static IEnumerable<char> TraverseHorizontal(string[] lines)
        {
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    yield return lines[y][x];
                }

                // yield a separator between lines
                yield return '\n';
            }
        }

        static IEnumerable<char> TraverseVertical(string[] lines)
        {
            for (var x = 0; x < lines[0].Length; x++)
            {
                for (var y = 0; y < lines.Length; y++)
                {
                    yield return lines[y][x];
                }

                // yield a separator between lines
                yield return '\n';
            }
        }

        static IEnumerable<char> TraverseDiagonal(string[] lines)
        {
            for (var y = 0; y < lines.Length; y++)
            {
                foreach (var c in TraverseLine(y, 0))
                {
                    yield return c;
                }
            }

            for (var x = 1; x < lines[0].Length; x++)
            {
                foreach (var c in TraverseLine(0, x))
                {
                    yield return c;
                }
            }

            IEnumerable<char> TraverseLine(int y, int x)
            {
                while (y < lines.Length && x < lines[y].Length)
                {
                    yield return lines[y][x];

                    y++;
                    x++;
                }

                // yield a separator between lines
                yield return '\n';
            }
        }

        static IEnumerable<char> TraverseDiagonalBackwards(string[] lines)
        {
            for (var y = 0; y < lines.Length; y++)
            {
                foreach (var c in TraverseLine(y, lines[y].Length - 1))
                {
                    yield return c;
                }
            }

            for (var x = lines[0].Length - 2; x >= 0; x--)
            {
                foreach (var c in TraverseLine(0, x))
                {
                    yield return c;
                }
            }

            IEnumerable<char> TraverseLine(int y, int x)
            {
                while (y < lines.Length && x >= 0)
                {
                    yield return lines[y][x];

                    y++;
                    x--;
                }

                // yield a separator between lines
                yield return '\n';
            }
        }
    }
}