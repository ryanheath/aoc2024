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

            return GetVisited(obstructions, guard, dim)!.Count;
        }

        int Part2(string[] lines)
        {
            var (obstructions, guard, dim) = Parse(lines);

            var path = GetVisited(obstructions, guard, dim)!;

            var loops = 0;

            // just brute force each position to find a loop
            foreach (var (x, y) in path)
            {
                if (obstructions.Contains((x, y))) continue;
                if (guard.x == x && guard.y == y) continue;

                // add a obstruction at this position
                obstructions.Add((x, y));

                if (GetVisited(obstructions, guard, dim) is null)
                {
                    loops++;
                }

                // remove the obstruction
                obstructions.Remove((x, y));
            }

            return loops;
        }

        static HashSet<(int x, int y)>? GetVisited(HashSet<(int x, int y)> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim)
        {
            var (x, y, d) = guard;
            HashSet<(int x, int y, Direction d)>? visited = [];
            visited.Add((x, y, d));

            while (Walk());

            // strip the direction from the visited positions
            return visited?.Select(v => (v.x, v.y)).ToHashSet();

            bool Walk()
            {
                var (nextX, nextY) = NextPosition();
                if (OutOfBounds(nextX, nextY)) return false;
                if (obstructions.Contains((nextX, nextY)))
                {
                    Rotate90();
                }
                else
                {
                    if (Seen(nextX, nextY, d)) { visited = null; return false; };
                    x = nextX;
                    y = nextY;
                }
                visited.Add((x, y, d));
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

                bool Seen(int x, int y, Direction d) => visited.Contains((x, y, d));
            }
        }

        static (HashSet<(int x, int y)> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim) Parse(string[] lines)
        {
            HashSet<(int x, int y)> obstructions = [];
            (int x, int y, Direction d) guard = (0, 0, Direction.N);

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
            {
                switch (lines[y][x])
                {
                    case '#': obstructions.Add((x, y)); break;
                    case '^': guard = (x, y, Direction.N); break;
                }   
            }

            return (obstructions, guard, (lines[0].Length, lines.Length));
        }
    }

    enum Direction { N, E, S, W }
}