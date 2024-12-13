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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(37128);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => Parse(lines).Select(m => CheapestPrize(m, 3, 1)).Where(x => x != int.MaxValue).Sum();
        int Part2(string[] lines) => 0;

        static int CheapestPrize((int ax, int ay, int bx, int by, int px, int py) machine, int costA, int costB)
        {
            var (ax, ay, bx, by, px, py) = machine;
            var maxATimes = px / ax;

            for (var i = maxATimes; i > 0; i -= 1)
            {
                var timesA = i;
                var timesB = Math.DivRem(px - timesA * ax, bx, out var remainder);
                if (remainder != 0) continue;
                if (timesA * ay + timesB * by != py) continue;
                return timesA * costA + timesB * costB;
            }

            return int.MaxValue;
        }

        static (int ax, int ay, int bx, int by, int px, int py)[] Parse(string[] lines)
        {
            var list = new List<(int ax, int ay, int bx, int by, int px, int py)>();

            for (var i = 0; i < lines.Length; i++)
            {
                var (ax, ay) = ParseLine("Button A: ");
                var (bx, by) = ParseLine("Button B: ");
                var (px, py) = ParseLine("Prize: ");
                list.Add((ax, ay, bx, by, px, py));

                (int x, int y) ParseLine(string prefix)
                {
                    var parts = lines[i++][prefix.Length..].Split(", ");
                    return (parts[0]["X_".Length..].ToInt(), parts[1]["Y_".Length..].ToInt());
                }
            }

            return list.ToArray();
        }
    }
}