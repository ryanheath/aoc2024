using Microsoft.VisualBasic;

static partial class Aoc2024
{
    public static void Day6()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                ....#.....
                .........#
                ..........
                ..#.......
                .......#..
                ..........
                .#..^.....
                ........#.
                #.........
                ......#...
                """.ToLines();
            Part1(input).Should().Be(41);
            Part2(input).Should().Be(6);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(4722);
            Part2(input).Should().Be(1602);
        }

        int Part1(string[] lines)
        {
            var (obstructions, guard, dim) = Parse(lines);

            return GetPath(obstructions, guard, dim, [])!.Select(p => (p.x, p.y)).ToHashSet().Count;
        }

        int Part2(string[] lines)
        {
            var (obstructions, guard, dim) = Parse(lines);

            var path = GetPath(obstructions, guard, dim, [])!;

            var loops = 0;

            // just brute force each position to find a loop
            // ignore the guard position
            for (var i = 1; i < path.Count; i++)
            {
                var (x, y, d) = path[i];

                var obs = HashCode.Combine(x, y);

                if (obstructions.Contains(obs)) continue;

                var startPath = path[..i];

                // don't add positions that are already in the path
                if (d != Direction.N && startPath.Contains((x, y, Direction.N))) continue;
                if (d != Direction.E && startPath.Contains((x, y, Direction.E))) continue;
                if (d != Direction.S && startPath.Contains((x, y, Direction.S))) continue;
                if (d != Direction.W && startPath.Contains((x, y, Direction.W))) continue;

                // add a obstruction at this position
                obstructions.Add(obs);

                if (GetPath(obstructions, path[i-1], dim, startPath) is null)
                {
                    loops++;
                }

                // remove the obstruction
                obstructions.Remove(obs);
            }

            return loops;
        }

        static List<(int x, int y, Direction d)>? GetPath(HashSet<int> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim, List<(int x, int y, Direction d)> startPath)
        {
            var (x, y, d) = guard;
            List<(int x, int y, Direction d)>? path = [];
            HashSet<int> seen = [..startPath.Select(p => HashCode.Combine(p.x, p.y, p.d))];
            if (startPath is []) path!.Add((x, y, d));
            seen.Add(HashCode.Combine(x, y, d));

            while (Walk());

            return path;

            bool Walk()
            {
                var (nextX, nextY) = NextPosition();
                if (OutOfBounds(nextX, nextY)) return false;
                if (obstructions.Contains(HashCode.Combine(nextX, nextY)))
                {
                    Rotate90();
                }
                else
                {
                    if (Seen(nextX, nextY, d)) { path = null; return false; };
                    x = nextX;
                    y = nextY;
                }
                if (startPath is []) path.Add((x, y, d));
                seen.Add(HashCode.Combine(x, y, d));
                return true;

                (int x, int y) NextPosition() => d switch
                {
                    Direction.N => (x, y - 1),
                    Direction.E => (x + 1, y),
                    Direction.S => (x, y + 1),
                    Direction.W => (x - 1, y),
                    _ => throw new InvalidOperationException()
                };

                bool OutOfBounds(int x, int y) => x < 0 || x >= dim.maxX || y < 0 || y >= dim.maxY;

                void Rotate90() => d = (Direction)(((int)d + 1) % 4);

                bool Seen(int x, int y, Direction d) => seen.Contains(HashCode.Combine(x, y, d));
            }
        }

        static (HashSet<int> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim) Parse(string[] lines)
        {
            HashSet<int> obstructions = [];
            (int x, int y, Direction d) guard = (0, 0, Direction.N);

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
            {
                switch (lines[y][x])
                {
                    case '#': obstructions.Add(HashCode.Combine(x, y)); break;
                    case '^': guard = (x, y, Direction.N); break;
                }   
            }

            return (obstructions, guard, (lines[0].Length, lines.Length));
        }
    }

    enum Direction { N, E, S, W }
}