static partial class Aoc2024
{
    public static void Day19()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                r, wr, b, g, bwu, rb, gb, br

                brwrr
                bggr
                gbbr
                rrbgbr
                ubwu
                bwurrg
                brgr
                bbrgwb
                """.ToLines();
            Part1(input).Should().Be(6);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(355);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => CountValids(Parse(lines));
        int Part2(string[] lines) => 0;

        static int CountValids((string[] blocks, string[] configurations) input)
        {
            var founds = new HashSet<string>();
            var notFounds = new HashSet<string>();

            return input.configurations.Where(IsValid).Count();

            bool IsValid(string config)
            {
                if (founds.Contains(config))
                    return true;
                if (notFounds.Contains(config))
                    return false;

                foreach (var b in input.blocks)
                {
                    if (config.Length < b.Length || !config.EndsWith(b)) continue;
                    if (b.Length == config.Length || IsValid(config[..^(b.Length)]))
                    {
                        founds.Add(config);
                        return true;
                    }
                }

                notFounds.Add(config);
                return false;
            }
        }

        static (string[] blocks, string[] configurations) Parse(string[] lines)
        {
            var blocks = lines[0].Split(", ");

            var configurations = lines[2..];

            return (blocks, configurations);
        }
    }
}