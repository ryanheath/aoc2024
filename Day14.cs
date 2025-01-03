﻿static partial class Aoc2024
{
    public static void Day14()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                p=0,4 v=3,-3
                p=6,3 v=-1,-3
                p=10,3 v=-1,2
                p=2,0 v=2,-1
                p=0,0 v=1,3
                p=3,0 v=-2,-2
                p=7,6 v=-1,-3
                p=3,0 v=-1,-2
                p=9,3 v=2,3
                p=7,3 v=-1,2
                p=2,4 v=2,-3
                p=9,5 v=-3,-3
                """.ToLines();
            Part1(input, 11, 7).Should().Be(12);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, 101, 103).Should().Be(231782040);
            Part2(input, 101, 103).Should().Be(6475);
        }

        int Part1(string[] lines, int w, int h) => SafetyFactor(Parse(lines), w, h, 100);
        int Part2(string[] lines, int w, int h) => FindChristmasTree(Parse(lines), w, h);

        static int SafetyFactor((int x, int y, int vx, int vy)[] robots, int w, int h, int s)
        {
            int q1 = 0, q2 = 0, q3 = 0, q4 = 0;
            var mw = w / 2;
            var mh = h / 2;

            foreach (var (rx, ry, vx, vy) in robots)
            {
                var x = PositiveMod(rx + s * vx, w);
                var y = PositiveMod(ry + s * vy, h);

                if (x < mw && y < mh) q1++;
                if (x > mw && y < mh) q2++;
                if (x < mw && y > mh) q3++;
                if (x > mw && y > mh) q4++;
            }

            return q1 * q2 * q3 * q4;
        }

        static int FindChristmasTree((int x, int y, int vx, int vy)[] robots, int w, int h)
        {
            var s = 0;

            while (true)
            { 
                for (var i = 0; i < robots.Length; i++)
                {
                    var r = robots[i];
                    robots[i] = r with { x = PositiveMod(r.x + r.vx, w), y = PositiveMod(r.y + r.vy, h) };
                }
                s++;

                // just find the first time when all robots are in a unique cell
                var cells = robots.Select(r => r.y * h + r.x).ToHashSet();
                if (cells.Count == robots.Length) return s;
            }
        }

        static (int x, int y, int vx, int vy)[] Parse(string[] lines)
            => lines
                .Select(line =>
                    {
                        var parts = line.Split(" ");
                        var (x, y) = parts[0]["p=".Length..].AsSpan().To2Ints(",");
                        var (vx, vy) = parts[1]["v=".Length..].AsSpan().To2Ints(",");
                        return (x, y, vx, vy);
                    })
                .ToArray();
    }
}