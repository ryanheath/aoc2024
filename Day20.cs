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
            /*
            There are 32 cheats that save 50 picoseconds.
            There are 31 cheats that save 52 picoseconds.
            There are 29 cheats that save 54 picoseconds.
            There are 39 cheats that save 56 picoseconds.
            There are 25 cheats that save 58 picoseconds.
            There are 23 cheats that save 60 picoseconds.
            There are 20 cheats that save 62 picoseconds.
            There are 19 cheats that save 64 picoseconds.
            There are 12 cheats that save 66 picoseconds.
            There are 14 cheats that save 68 picoseconds.
            There are 12 cheats that save 70 picoseconds.
            There are 22 cheats that save 72 picoseconds.
            There are 4 cheats that save 74 picoseconds.
            There are 3 cheats that save 76 picoseconds.
            */
            Part2(input, 76).Should().Be(3);
            Part2(input, 74).Should().Be(7);
            Part2(input, 72).Should().Be(29);
            Part2(input, 70).Should().Be(41);
            Part2(input, 68).Should().Be(55);
            Part2(input, 66).Should().Be(67);
            Part2(input, 64).Should().Be(86);
            Part2(input, 62).Should().Be(106);
            Part2(input, 60).Should().Be(129);
            Part2(input, 58).Should().Be(154);
            Part2(input, 56).Should().Be(193);
            Part2(input, 54).Should().Be(222);
            Part2(input, 52).Should().Be(253);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, 100).Should().Be(1293);
            Part2(input, 100).Should().Be(977747);
        }

        int Part1(string[] lines, int saveAtLeast) => GetCheats(lines, saveAtLeast, 2);
        int Part2(string[] lines, int saveAtLeast) => GetCheats(lines, saveAtLeast, 20);

        static int GetCheats(string[] map, int saveAtLeast, int upTo)
        {
            var path = GetPath(map);
            var pathSet = path.Index().ToDictionary(p => Hash(p.Item.x, p.Item.y), p => p.Index);
            var seen = new HashSet<(int,int)>();

            var cheats = 0;
            for (var j = 0; j < path.Count; j++)
            {
                var (x, y) = path[j];
                for (var i = 2; i <= upTo; i++)
                {
                    seen.Clear();
                    for (var d = 0; d <= i; d++)
                    {
                        var cutCorner = pathSet.GetValueOrDefault(Hash(x+d, y+i-d), -1);
                        if (cutCorner != -1 && (cutCorner - j - i) >= saveAtLeast && seen.Add((j, cutCorner)))
                            cheats++;

                        cutCorner = pathSet.GetValueOrDefault(Hash(x-d, y+i-d), -1);
                        if (cutCorner != -1 && (cutCorner - j - i) >= saveAtLeast && seen.Add((j, cutCorner)))
                            cheats++;

                        cutCorner = pathSet.GetValueOrDefault(Hash(x+d, y-i+d), -1);
                        if (cutCorner != -1 && (cutCorner - j - i) >= saveAtLeast && seen.Add((j, cutCorner)))
                            cheats++;

                        cutCorner = pathSet.GetValueOrDefault(Hash(x-d, y-i+d), -1);
                        if (cutCorner != -1 && (cutCorner - j - i) >= saveAtLeast && seen.Add((j, cutCorner)))
                            cheats++;
                    }
                }
            }

            return cheats;

            static int Hash(int x, int y) => x * 1000 + y;
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