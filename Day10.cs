static partial class Aoc2024
{
    public static void Day10()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                89010123
                78121874
                87430965
                96549874
                45678903
                32019012
                01329801
                10456732
                """.ToLines();
            Part1(input).Should().Be(36);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(0);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            var (map, startingPositions) = Parse(lines);

            var trailheads = new Queue<(int sx, int sy, int ex, int ey)>();
            foreach (var (x, y) in startingPositions)
                trailheads.Enqueue((x, y, x, y));

            var trails = new HashSet<(int sx, int sy, int ex, int ey)>();

            while (trailheads.Count > 0)
            {
                var trail = trailheads.Dequeue(); 
                Extend( 0, -1);
                Extend(+1,  0);
                Extend( 0, +1);
                Extend(-1,  0);

                void Extend(int dx, int dy)
                {
                    var nx = trail.ex + dx;
                    var ny = trail.ey + dy;
                    if (nx < 0 || nx >= map[0].Length || ny < 0 || ny >= map.Length)
                    {
                        return;
                    }
                    var nheight = map[ny][nx];
                    if (map[trail.ey][trail.ex] + 1 != nheight)
                    {
                        return;
                    }
                    if (nheight == 9)
                    {
                        trails.Add((trail.sx, trail.sy, nx, ny));
                    }
                    else
                    {
                        trailheads.Enqueue((trail.sx, trail.sy, nx, ny));
                    }
                }
            }

            var score = trails.Count;

            return score;
        }

        int Part2(string[] lines) => 0;

        (int[][] map, List<(int x, int y)> trailheads) Parse(string[] lines)
        {
            var trailheads = new List<(int x, int y)>();
            var map = new int[lines.Length][];
            for (var y = 0; y < lines.Length; y++)
            {
                map[y] = new int [lines[0].Length]; 
                for (var x = 0; x < lines[0].Length; x++)
                {
                    var height = lines[y][x].ToInt();
                    if (height == 0)
                        trailheads.Add((x, y));
                    else
                        map[y][x] = height;
                }
            }

            return (map, trailheads);
        }
    }
}