static partial class Aoc2024
{
    public static void Day16()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                #################
                #...#...#...#..E#
                #.#.#.#.#.#.#.#.#
                #.#.#.#...#...#.#
                #.#.#.#.###.#.#.#
                #...#.#.#.....#.#
                #.#.#.#.#.#####.#
                #.#...#.#.#.....#
                #.#.#####.#.###.#
                #.#.#.......#...#
                #.#.###.#####.###
                #.#.#...#.....#.#
                #.#.#.#####.###.#
                #.#.#.........#.#
                #.#.#.#########.#
                #S#.............#
                #################
                """.ToLines();
            Part1(input).Should().Be(11048);
            Part2(input).Should().Be(64);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(115500);
            Part2(input).Should().Be(679);
        }

        int Part1(string[] lines) => RunMaze(lines, returnLowestScore: true);
        int Part2(string[] lines) => RunMaze(lines, returnLowestScore: false);

        static int RunMaze(string[] map, bool returnLowestScore)
        {
            var h = map.Length;
            // find S
            int sx = 0, sy = 0;
            foreach (var (y, line) in map.Index())
            {
                var x = line.IndexOf('S');
                if (x != -1)
                {
                    (sx, sy) = (x, y);
                    break;
                }
            }

            var lowestScore = int.MaxValue;
            var lowestScorePaths = new List<List<int>>();
            var visited = new Dictionary<int, int>();
            var trailheads = new Queue<(int x, int y, Direction d, int score, List<int> path)>();

            visited[CellDirectionIndex(sx, sy, Direction.E, h)] = 0;
            trailheads.Enqueue((sx, sy, Direction.E, 0, [CellIndex(sx, sy, h)]));

            while (trailheads.Count > 0)
            {
                var trail = trailheads.Dequeue();
                Extend(RotateCC90(trail.d), trail.score + 1001);
                Extend(            trail.d, trail.score + 1);
                Extend( RotateC90(trail.d), trail.score + 1001);

                void Extend(Direction d, int score)
                {
                    var (x, y) = NextPosition(trail.x, trail.y, d);
                    var c = map[y][x];
                    
                    if (c == '#') return;

                    if (c == 'E')
                    {
                        if (score <= lowestScore)
                        {
                            if (score < lowestScore) lowestScorePaths.Clear(); // start over
                            lowestScore = score;
                            lowestScorePaths.Add(trail.path);
                        }
                        return;
                    }

                    if (visited.TryGetValue(CellDirectionIndex(x, y, d, h), out var visitedScore) && visitedScore < score) return;

                    visited[CellDirectionIndex(x, y, d, h)] = score;
                    trailheads.Enqueue((x, y, d, score, [..trail.path, CellIndex(x, y, h)]));
                }
            }

            return returnLowestScore ? lowestScore : lowestScorePaths.SelectMany(p => p).Distinct().Count() + 1; // +1 for E

            static int CellIndex(int x, int y, int h) => y * h + x;
            static int CellDirectionIndex(int x, int y, Direction d, int h) => y * 4 + x * h * 4 + (int)d;
            static (int x, int y) NextPosition(int x, int y, Direction d) => d switch
            {
                Direction.N => (x, y - 1),
                Direction.E => (x + 1, y),
                Direction.S => (x, y + 1),
                Direction.W or _ => (x - 1, y)
            };
        }

        static Direction RotateC90(Direction d) => (Direction)(((int)d + 1) % 4);
        static Direction RotateCC90(Direction d) => (Direction)PositiveMod(((int)d - 1), 4);
    }
}