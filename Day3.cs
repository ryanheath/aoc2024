using System.Text.RegularExpressions;

static partial class Aoc2024
{
    public static void Day3()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
                """.ToLines();
            Part1(input).Should().Be(161);
            input = 
                """
                xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
                """.ToLines();
            Part2(input).Should().Be(48);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(161289189L);
            Part2(input).Should().Be(83595109L);
        }

        long Part1(string[] lines) => RunInstructions(lines, doMode: false);
        long Part2(string[] lines) => RunInstructions(lines, doMode: true);

        long RunInstructions(string[] lines, bool doMode)
        {
            var sum = 0L;
            var doSum = true;
            

            foreach (var line in lines)
            foreach (var match in InstructionsRegex.EnumerateMatches(line))
            {
                var instruction = line.AsSpan(match.Index, match.Length);
                sum += instruction switch
                {
                    ['m', ..] => doSum ? MUL(line.AsSpan(match.Index + 4, match.Length - 5)) : 0,
                    ['d', 'o', 'n', ..] => DONT(),
                    ['d', ..] => DO(),
                    _ => 0
                };
            }

            return sum;

            static long MUL(ReadOnlySpan<char> args)
            {
                var (a, b) = args.To2Longs(",");
                return a * b;
            }

            long DO()   { if (doMode) { doSum = true; } return 0; }
            long DONT() { if (doMode) { doSum = false; } return 0; }
        }
    }

    [GeneratedRegex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)")]
    private static partial Regex InstructionsRegex { get; }
}