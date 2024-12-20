static partial class Aoc2024
{
    public static void Day20()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                ###############
                #...#...#.....#
                #.#.#.#.#.###.#
                #S#...#.#.#...#
                #######.#.#.###
                #######.#.#...#
                #######.#.###.#
                ###..E#...#...#
                ###.#######.###
                #...###...#...#
                #.#####.#.###.#
                #.#...#.#.#...#
                #.#.#.#.#.#.###
                #...#...#...###
                ###############
                """.ToLines();
            /*
            There are 14 cheats that save 2 picoseconds.
            There are 14 cheats that save 4 picoseconds.
            There are 2 cheats that save 6 picoseconds.
            There are 4 cheats that save 8 picoseconds.
            There are 2 cheats that save 10 picoseconds.
            There are 3 cheats that save 12 picoseconds.
            There is one cheat that saves 20 picoseconds.
            There is one cheat that saves 36 picoseconds.
            There is one cheat that saves 38 picoseconds.
            There is one cheat that saves 40 picoseconds.
            There is one cheat that saves 64 picoseconds.
            */
            Part1(input, 64).Should().Be(1);
            Part1(input, 40).Should().Be(2);
            Part1(input, 38).Should().Be(3);
            Part1(input, 36).Should().Be(4);
            Part1(input, 20).Should().Be(5);
            Part1(input, 12).Should().Be(8);
            Part1(input, 10).Should().Be(10);
            Part1(input, 8).Should().Be(14);
            Part1(input, 6).Should().Be(16);
            Part1(input, 4).Should().Be(30);
            Part1(input, 2).Should().Be(44);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, 100).Should().Be(1293);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines, int saveAtLeast) => GetCheats(lines, saveAtLeast);
        int Part2(string[] lines) => 0;

        static int GetCheats(string[] map, int saveAtLeast)
        {
            var path = GetPath(map);
            var removedBlocks = new HashSet<(int x, int y)>();

            var cheats = 0;
            for (var j = 0; j < path.Count; j++)
            {
                var (x, y) = path[j];
                for (var i = 2; i >= 2; i--)
                {
                    CheckCheats(+i, 0);
                    CheckCheats(-i, 0);
                    CheckCheats(0, +i);
                    CheckCheats(0, -i);
                }

                void CheckCheats(int dx, int dy)
                {
                    var block = (x: x + Math.Sign(dx), y: y + Math.Sign(dy));
                    if (map[block.y][block.x] != '#') return;

                    var cut = (x + dx, y + dy);
                    var cutCorner = path.IndexOf(cut);
                    if (cutCorner == -1) return;

                    if (!removedBlocks.Add(block)) return;

                    if ((cutCorner - j - 2) >= saveAtLeast)
                        cheats++;
                }
            }

            return cheats;
        }

        static List<(int x, int y)> GetPath(string[] map)
        {
            var S = map.Select((line, y) => (x: line.IndexOf('S'), y: y)).First(p => p.x >= 0);
            var E = map.Select((line, y) => (x: line.IndexOf('E'), y: y)).First(p => p.x >= 0);

            var path = new List<(int x, int y)>();
            var visited = new HashSet<(int x, int y)>();

            var queue = new Queue<(int x, int y)>();
            queue.Enqueue(S);

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                if (map[y][x] == '#') continue;
                if (!visited.Add((x, y))) continue;

                path.Add((x, y));
                if (x == E.x && y == E.y) break;

                queue.Enqueue((x + 1, y));
                queue.Enqueue((x - 1, y));
                queue.Enqueue((x, y + 1));
                queue.Enqueue((x, y - 1));
            }

            return path;
        }
    }
}