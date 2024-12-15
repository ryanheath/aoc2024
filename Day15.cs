static partial class Aoc2024
{
    public static void Day15()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                ##########
                #..O..O.O#
                #......O.#
                #.OO..O.O#
                #..O@..O.#
                #O#..O...#
                #O..O..O.#
                #.OO.O.OO#
                #....O...#
                ##########

                <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                """.ToLines();
            Part1(input).Should().Be(10092);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1487337);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => GetSumGPS(RunInstructions(lines));
        int Part2(string[] lines) => 0;

        static int GetSumGPS(char[][] map)
        {
            var sum = 0;

            for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                if (map[y][x] == 'O')
                    sum += y * 100 + x;

            return sum;
        }

        static char[][] RunInstructions(string[] lines)
        {
            var (map, instructions) = Parse(lines);

            var (rx, ry) = GetRobot();

            foreach (var m in instructions)
            {
                var (dx, dy) = m switch
                {
                    '<' => (-1, 0),
                    '>' => (+1, 0),
                    '^' => (0, -1),
                    'v' or _  => (0, +1),
                };
                MoveRobot(dx, dy);
            }

            return map;

            void MoveRobot(int dx, int dy)
            {
                var c = map[ry + dy][rx + dx];
                if (c == '#') return; // wall, do nothing
                if (c == '.')
                {
                    // move robot
                    map[ry][rx] = '.';
                    ry += dy;
                    rx += dx;
                    map[ry][rx] = '@';
                }
                else if (c == 'O')
                {
                    var (ddx, ddy) = (dx * 2, dy * 2);

                    var dc = map[ry + ddy][rx + ddx];
                    while (dc == 'O')
                    {
                        ddx += dx;
                        ddy += dy;
                        dc = map[ry + ddy][rx + ddx];
                    }
                    if (dc == '.')
                    {
                        // move boxe(s)
                        map[ry + ddy][rx + ddx] = 'O';

                        // move robot
                        map[ry][rx] = '.';
                        ry += dy;
                        rx += dx;
                        map[ry][rx] = '@';
                    }
                }
            }

            (int x, int y) GetRobot()
            {
                for (var y = 0; y < map.Length; y++)
                for (var x = 0; x < map[y].Length; x++)
                    if (map[y][x] == '@')
                        return (x, y);

                throw new Exception("Robot not found");
            }
        }

        static (char[][] map, string instructions) Parse(string[] lines)
        {
            var y = lines.Count(l => l.Length > 0 && l[0] == '#');
            var map = new char[y][];
            for (var i = 0; i < y; i++)
            {
                map[i] = lines[i].ToCharArray();
            }

            var instructions = "";
            for (var i = y + 1; i < lines.Length; i++)
            {
                instructions += lines[i];
            }

            return (map, instructions);
        }
    }
}