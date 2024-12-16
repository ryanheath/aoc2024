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
            var lowestScorePaths = new List<List<(int, int)>>();
            var visited = new Dictionary<(int x, int y, Direction d), int>();
            var trailheads = new Queue<(int x, int y, Direction d, int score, List<(int, int)> path)>();

            visited[(sx, sy, Direction.E)] = 0;
            trailheads.Enqueue((sx, sy, Direction.E, 0, [(sx, sy)]));

            while (trailheads.Count > 0)
            {
                var trail = trailheads.Dequeue();
                var (dxcc, dycc, dcc, dx, dy, dxc, dyc, dc) = trail.d switch
                {
                    Direction.E =>      ( 0, -1, Direction.N, +1,  0,  0, +1, Direction.S),
                    Direction.S =>      (+1,  0, Direction.E,  0, +1, -1,  0, Direction.W),
                    Direction.W =>      ( 0, +1, Direction.S, -1,  0,  0, -1, Direction.N),
                    Direction.N or _ => (-1,  0, Direction.W,  0, -1, +1,  0, Direction.E)
                };
                Extend(trail.x + dxcc, trail.y + dycc,     dcc, trail.score + 1001);
                Extend(trail.x + dx,     trail.y + dy, trail.d, trail.score + 1);
                Extend(trail.x + dxc,   trail.y + dyc,      dc, trail.score + 1001);

                void Extend(int x, int y, Direction d, int score)
                {
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

                    if (visited.TryGetValue((x, y, d), out var visitedScore) && visitedScore < score) return;

                    visited[(x, y, d)] = score;
                    trailheads.Enqueue((x, y, d, score, [..trail.path, (x, y)]));
                }
            }

            return returnLowestScore ? lowestScore : lowestScorePaths.SelectMany(p => p).Distinct().Count() + 1; // +1 for E
        }
    }
}