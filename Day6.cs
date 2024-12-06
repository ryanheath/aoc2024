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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(4722);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            var (obstructions, guard, dim) = Parse(lines);

            return GetVisited(obstructions, guard, dim).Count;

            static HashSet<(int x, int y)> GetVisited(HashSet<(int x, int y)> obstructions, (int x, int y, Direction d) guard, (int maxX, int maxY) dim)
            {
                var (x, y, d) = guard;
                HashSet<(int x, int y)> visited = [];
                visited.Add((x, y));

                while (Walk());

                return visited;

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
                        x = nextX;
                        y = nextY;
                        visited.Add((x, y));
                    }
                    return true;

                    (int x, int y) NextPosition() => d switch
                    {
                        Direction.N => (x, y - 1),
                        Direction.E => (x + 1, y),
                        Direction.S => (x, y + 1),
                        Direction.W => (x - 1, y),
                        _ => throw new InvalidOperationException()
                    };

                    bool OutOfBounds(int nextX, int nextY) => nextX < 0 || nextX >= dim.maxX || nextY < 0 || nextY >= dim.maxY;

                    void Rotate90() => d = (Direction)(((int)d + 1) % 4);
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

        int Part2(string[] lines) => 0;
    }

    enum Direction { N, E, S, W }
}