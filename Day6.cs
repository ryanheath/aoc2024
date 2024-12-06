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

            return GetPath(obstructions, guard, dim).Select(p => (p.x, p.y)).ToHashSet().Count;
        }

        int Part2(string[] lines)
        {
            var (obstructions, guard, dim) = Parse(lines);

            var originalPath = GetPath(obstructions, guard, dim);
            var path = new Stack<(int x, int y, Direction d)>(originalPath);
            var seen = originalPath.Select(p => HashCode.Combine(p.x, p.y, p.d)).ToHashSet();

            var loops = 0;

            // travese the path backwards 
            // so we have an easy bookkeeping of seen positions
            // without copying
            while (path.Count > 1)
            {
                // remove the position from the path
                var (x, y, d) = path.Pop();

                // remove the position from seen
                seen.Remove(HashCode.Combine(x, y, d));

                var obs = HashCode.Combine(x, y);

                if (obstructions.Contains(obs)) continue;

                // don't add positions that are already in the path
                if (d != Direction.N && seen.Contains(HashCode.Combine(x, y, Direction.N))) continue;
                if (d != Direction.E && seen.Contains(HashCode.Combine(x, y, Direction.E))) continue;
                if (d != Direction.S && seen.Contains(HashCode.Combine(x, y, Direction.S))) continue;
                if (d != Direction.W && seen.Contains(HashCode.Combine(x, y, Direction.W))) continue;

                // add a obstruction at this position
                obstructions.Add(obs);

                if (IsLoopPath(obstructions, path.Peek(), dim, [..seen]))
                {
                    loops++;
                }

                // remove the obstruction
                obstructions.Remove(obs);
            }

            return loops;
        }

        static List<(int x, int y, Direction d)> GetPath(HashSet<int> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim)
        {
            var (x, y, d) = guard;
            List<(int x, int y, Direction d)> path = [];
            path.Add((x, y, d));

            while (true)
            {
                var (nextX, nextY) = NextPosition(x, y, d);

                if (OutOfBounds(nextX, nextY, dim)) break;

                if (obstructions.Contains(HashCode.Combine(nextX, nextY)))
                {
                    d = Rotate90(d);
                }
                else
                {
                    x = nextX;
                    y = nextY;
                }

                path.Add((x, y, d));
            }

            return path;
        }

        static bool IsLoopPath(HashSet<int> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim, HashSet<int> seen)
        {
            var (x, y, d) = guard;
            seen.Add(HashCode.Combine(x, y, d));

            while (true)
            {
                var (nextX, nextY) = NextPosition(x, y, d);

                if (OutOfBounds(nextX, nextY, dim)) return false;

                if (obstructions.Contains(HashCode.Combine(nextX, nextY)))
                {
                    d = Rotate90(d);
                }
                else
                {
                    if (seen.Contains(HashCode.Combine(nextX, nextY, d))) return true;
                    x = nextX;
                    y = nextY;
                }

                seen.Add(HashCode.Combine(x, y, d));
            }
        }

        static Direction Rotate90(Direction d) => d = (Direction)(((int)d + 1) % 4);

        static bool OutOfBounds(int x, int y, (int maxX, int maxY) dim) => x < 0 || x >= dim.maxX || y < 0 || y >= dim.maxY;

        static (int x, int y) NextPosition(int x, int y, Direction d) => d switch
        {
            Direction.N => (x, y - 1),
            Direction.E => (x + 1, y),
            Direction.S => (x, y + 1),
            Direction.W => (x - 1, y),
            _ => throw new InvalidOperationException()
        };

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