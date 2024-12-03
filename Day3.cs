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

        long Part1(string[] lines) => RunInstructions(lines, ignoreDo: true);
        long Part2(string[] lines) => RunInstructions(lines, ignoreDo: false);

        long RunInstructions(string[] lines, bool ignoreDo)
        {
            var mulRegex = MulRegex();
            var sum = 0L;
            var doSum = true;

            foreach (var line in lines)
            {
                var matches = mulRegex.Matches(line);
                foreach (Match match in matches)
                {
                    if (ignoreDo && match.Groups[0].Value.StartsWith(DONT))
                    {
                        continue;
                    }
                    if (ignoreDo && match.Groups[0].Value.StartsWith(DO))
                    {
                        continue;
                    }
                    if (!ignoreDo && match.Groups[0].Value.StartsWith(DONT))
                    {
                        doSum = false;
                        continue;
                    }
                    else if (!ignoreDo && match.Groups[0].Value.StartsWith(DO))
                    {
                        doSum = true;
                        continue;
                    }

                    if (!doSum)
                    {
                        continue;
                    }

                    var a = match.Groups[1].Value.ToLong();
                    var b = match.Groups[2].Value.ToLong();
                    sum += a * b;
                }
            }

            return sum;
        }
    }

    const string DO = "do";
    const string DONT = "don't";

    [GeneratedRegex(@$"mul\((\d+),(\d+)\)|{DO}\(\)|{DONT}\(\)")]
    private static partial Regex MulRegex();
}