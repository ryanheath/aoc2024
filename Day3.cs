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
            var mulRegex = InstructionsRegex();
            var sum = 0L;
            var doSum = true;

            foreach (var line in lines)
            foreach (Match match in mulRegex.Matches(line))
            {
                if (match.Groups[xMUL].Success)
                {
                    MUL(match.Groups[xMUL1].Value, match.Groups[xMUL2].Value);
                }
                else if (match.Groups[xDO].Success)
                {
                    DO();
                }
                else if (match.Groups[xDONT].Success)
                {
                    DONT();
                }
            }

            return sum;

            void MUL(string a, string b) { if (doSum) { sum += a.ToLong() * b.ToLong(); } }
            void DO()   { if (!ignoreDo) { doSum = true;  } }
            void DONT() { if (!ignoreDo) { doSum = false; } }
        }
    }

    const string xMUL = "mul";
    const string xMUL1 = "mul1";
    const string xMUL2 = "mul2";
    const string xDO = "do";
    const string xDONT = "dont";

    [GeneratedRegex(@$"(?<{xMUL}>mul)\((?<{xMUL1}>\d+),(?<{xMUL2}>\d+)\)|(?<{xDO}>do)\(\)|(?<{xDONT}>don't)\(\)")]
    private static partial Regex InstructionsRegex();
}