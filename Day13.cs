static partial class Aoc2024
{
    public static void Day13()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                Button A: X+94, Y+34
                Button B: X+22, Y+67
                Prize: X=8400, Y=5400

                Button A: X+26, Y+66
                Button B: X+67, Y+21
                Prize: X=12748, Y=12176

                Button A: X+17, Y+86
                Button B: X+84, Y+37
                Prize: X=7870, Y=6450

                Button A: X+69, Y+23
                Button B: X+27, Y+71
                Prize: X=18641, Y=10279
                """.ToLines();
            Part1(input).Should().Be(480);
            Part2(input).Should().Be(875318608908);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(37128);
            Part2(input).Should().Be(74914228471331);
        }

        long Part1(string[] lines) => SumPrizes(Parse(lines));
        long Part2(string[] lines) => SumPrizes(Parse(lines).Select(m => m with { px = m.px + 10000000000000, py = m.py + 10000000000000 }));

        static long SumPrizes(IEnumerable<(long ax, long ay, long bx, long by, long px, long py)> machines)
            => machines.Select(Prize).Sum(x => x != long.MaxValue ? x : 0);

        static long Prize((long ax, long ay, long bx, long by, long px, long py) machine)
        {
            var (ax, ay, bx, by, px, py) = machine;

            // a = (5400*22)-(67*8400) / ((22*34) + (-94*67))
            var d1 = py * bx - by * px;
            var d2 = bx * ay - ax * by;
            var a = Math.DivRem(d1, d2, out var remainder);

            if (remainder != 0)
            {
                return long.MaxValue;
            }

            var b = (px - ax * a) / bx;

            return 3 * a + 1 * b;
        }

        static (long ax, long ay, long bx, long by, long px, long py)[] Parse(string[] lines)
        {
            var list = new List<(long ax, long ay, long bx, long by, long px, long py)>();

            for (var i = 0; i < lines.Length; i++)
            {
                var (ax, ay) = ParseLine("Button A: ");
                var (bx, by) = ParseLine("Button B: ");
                var (px, py) = ParseLine("Prize: ");
                list.Add((ax, ay, bx, by, px, py));

                (long x, long y) ParseLine(string prefix)
                {
                    var parts = lines[i++][prefix.Length..].Split(", ");
                    return (parts[0]["X_".Length..].ToLong(), parts[1]["Y_".Length..].ToLong());
                }
            }

            return list.ToArray();
        }
    }
}