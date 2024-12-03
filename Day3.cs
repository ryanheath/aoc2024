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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(161289189L);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines)
        {
            Regex mulRegex = new(@"mul\((\d+),(\d+)\)");
            long sum = 0;

            foreach (var line in lines)
            {
                var matches = mulRegex.Matches(line);
                foreach (Match match in matches)
                {
                    var a = match.Groups[1].Value.ToLong();
                    var b = match.Groups[2].Value.ToLong();
                    sum += a * b;
                }
            }

            return sum;
        }
        int Part2(string[] lines) => 0;
    }
}