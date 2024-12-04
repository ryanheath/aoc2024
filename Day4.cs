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
            Part2(input).Should().Be(9);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(2507);
            Part2(input).Should().Be(1969);
        }

        int Part1(string[] lines) => Count(lines, "XMAS");
        int Part2(string[] lines) => CountXMAS(lines);

        static int Count(string[] lines, string target) =>
            CountWord(lines.TraverseHorizontal(), target)
            + CountWord(lines.TraverseVertical(), target)
            + CountWord(lines.TraverseDiagonal(), target)
            + CountWord(lines.TraverseDiagonalBackwards(), target)
            + CountWord(lines.TraverseHorizontal(), target.ReverseString())
            + CountWord(lines.TraverseVertical(), target.ReverseString())
            + CountWord(lines.TraverseDiagonal(), target.ReverseString())
            + CountWord(lines.TraverseDiagonalBackwards(), target.ReverseString())
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

        static int CountXMAS(string[] lines)
        {
            var count = 0;

            for (var y = 1; y < lines.Length - 1; y++)
            for (var x = 1; x < lines[y].Length - 1; x++)
            {
                if (lines[y][x] != 'A') continue;

                _ = (lines[y - 1][x - 1], lines[y + 1][x + 1],
                     lines[y + 1][x - 1], lines[y - 1][x + 1]) 
                switch
                {
                    ('S', 'M', 'S', 'M') or
                    ('S', 'M', 'M', 'S') or
                    ('M', 'S', 'S', 'M') or
                    ('M', 'S', 'M', 'S') => count++,
                    _ => 0
                };
            }

            return count;
        }
    }
}