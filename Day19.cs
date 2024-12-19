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
            Part2(input).Should().Be(16);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(355);
            Part2(input).Should().Be(732978410442050);
        }

        int Part1(string[] lines) => CountValids(Parse(lines));
        long Part2(string[] lines) => CountAll(Parse(lines));

        static long CountAll((string[] blocks, string[] configurations) input)
        {
            var founds = new Dictionary<string, long>();
            var flookup = founds.GetAlternateLookup<ReadOnlySpan<char>>();

            var notFounds = new HashSet<string>();
            var nlookup = notFounds.GetAlternateLookup<ReadOnlySpan<char>>();

            return input.configurations.Sum(x => PermutateAll(x.AsSpan()));

            long PermutateAll(ReadOnlySpan<char> config)
            {
                if (flookup.TryGetValue(config, out var count))
                    return count;
                if (nlookup.Contains(config))
                    return 0;

                count = 0L;
                foreach (var b in input.blocks)
                {
                    if (!config.EndsWith(b)) continue;
                    if (b.Length == config.Length)
                    {
                        count++;
                    }
                    else
                    {
                        count += PermutateAll(config[..^(b.Length)]);
                    }
                }

                if (count > 0)
                {
                    founds[config.ToString()] = count;
                }
                else
                {
                    notFounds.Add(config.ToString());
                }

                return count;
            }
        }

        static int CountValids((string[] blocks, string[] configurations) input)
        {
            var founds = new HashSet<string>();
            var flookup = founds.GetAlternateLookup<ReadOnlySpan<char>>();
 
            var notFounds = new HashSet<string>();
            var nlookup = notFounds.GetAlternateLookup<ReadOnlySpan<char>>();

            return input.configurations.Where(x => IsValid(x.AsSpan())).Count();

            bool IsValid(ReadOnlySpan<char> config)
            {
                if (flookup.Contains(config))
                    return true;
                if (nlookup.Contains(config))
                    return false;

                foreach (var b in input.blocks)
                {
                    if (!config.EndsWith(b)) continue;
                    if (b.Length == config.Length || IsValid(config[..^(b.Length)]))
                    {
                        founds.Add(config.ToString());
                        return true;
                    }
                }

                notFounds.Add(config.ToString());
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