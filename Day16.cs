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
                }
            }

            var lowestScore = int.MaxValue;
            var visited = new Dictionary<(int x, int y, Direction d), int>();
            var paths = new List<HashSet<(int, int)>>();
            var trailheads = new Queue<(int x, int y, Direction d, int score, HashSet<(int, int)> path)>();
            trailheads.Enqueue((sx, sy, Direction.E, 0, new HashSet<(int, int)>([(sx, sy)])));
            visited[(sx, sy, Direction.E)] = 0;

            while (trailheads.Count > 0)
            {
                var trail = trailheads.Dequeue();
                var score = trail.score;
                if (trail.d == Direction.E)
                {
                    Extend( 0, -1, Direction.N, score + 1001);
                    Extend(+1,  0, Direction.E, score + 1);
                    Extend( 0, +1, Direction.S, score + 1001);
                }
                else if (trail.d == Direction.S)
                {
                    Extend(+1,  0, Direction.E, score + 1001);
                    Extend( 0, +1, Direction.S, score + 1);
                    Extend(-1,  0, Direction.W, score + 1001);
                }
                else if (trail.d == Direction.W)
                {
                    Extend( 0, +1, Direction.S, score + 1001);
                    Extend(-1,  0, Direction.W, score + 1);
                    Extend( 0, -1, Direction.N, score + 1001);
                }
                else if (trail.d == Direction.N)
                {
                    Extend(-1,  0, Direction.W, score + 1001);
                    Extend( 0, -1, Direction.N, score + 1);
                    Extend(+1,  0, Direction.E, score + 1001);
                }

                void Extend(int dx, int dy, Direction d, int score)
                {
                    var newPath = new HashSet<(int, int)>(trail.path);
                    newPath.Add((trail.x + dx, trail.y + dy));
                    var c = map[trail.y + dy][trail.x + dx];

                    if (c == '#')
                    {
                        return;
                    }

                    if (c == 'E')
                    {
                        if (score <= lowestScore)
                        {
                            if (score < lowestScore)
                                // start over
                                paths.Clear();
                            lowestScore = score;
                            paths.Add(newPath);
                        }
                        return;
                    }

                    if (visited.TryGetValue((trail.x + dx, trail.y + dy, d), out var visitedScore) && visitedScore < score)
                    {
                        return;
                    }

                    visited[(trail.x + dx, trail.y + dy, d)] = score;

                    trailheads.Enqueue((trail.x + dx, trail.y + dy, d, score, newPath));
                }
            }

            return returnLowestScore ? lowestScore : paths.SelectMany(p => p).ToHashSet().Count;
        }
    }
}