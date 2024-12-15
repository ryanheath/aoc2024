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
            Part2(input).Should().Be(9021);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1487337);
            Part2(input).Should().Be(1521952);
        }

        int Part1(string[] lines) => GetSumGPS(RunInstructions(lines));
        int Part2(string[] lines) => GetSumGPS(RunInstructionsScaleUp(lines));

        static int GetSumGPS(char[][] map)
        {
            var sum = 0;

            for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                if (map[y][x] is 'O' or '[')
                    sum += y * 100 + x;

            return sum;
        }

        static char[][] RunInstructions(string[] lines)
        {
            var (map, instructions) = Parse(lines);

            var (rx, ry) = GetRobot(map);

            ExecuteInstructions(instructions, MoveRobot);

            return map;

            void MoveRobot(int dx, int dy, char _)
            {
                var c = map[ry + dy][rx + dx];
                if (c == '.')
                {
                    DoMoveRobot();
                }
                else if (c == 'O')
                {
                    var (ddx, ddy) = (rx + dx * 2, ry + dy * 2);

                    c = map[ddy][ddx];
                    while (c == 'O')
                    {
                        ddx += dx;
                        ddy += dy;
                        c = map[ddy][ddx];
                    }
                    if (c == '.')
                    {
                        // move box(es)
                        map[ddy][ddx] = 'O';

                        DoMoveRobot();
                    }
                }

                void DoMoveRobot()
                {
                    // move robot
                    map[ry][rx] = '.';
                    ry += dy;
                    rx += dx;
                    map[ry][rx] = '@';
                }
            }
        }

        static char[][] RunInstructionsScaleUp(string[] lines)
        {
            var (map, instructions) = Parse(lines);
            ScaleUp(map);

            var (rx, ry) = GetRobot(map);

            ExecuteInstructions(instructions, MoveRobot);

            return map;

            void MoveRobot(int dx, int dy, char m)
            {
                var c = map[ry + dy][rx + dx];
                if (c == '.')
                {
                    DoMoveRobot();
                }
                else if (c != '#' && MoveBoxes())
                {
                    DoMoveRobot();
                }

                bool MoveBoxes()
                {
                    int bx = rx + dx, by = ry + dy;
                    if (map[by][bx] == ']') bx--;

                    var checkCells = new Queue<(int, int)>();
                    checkCells.Enqueue((bx, by));

                    var moveBoxes = new Stack<(int, int)>();

                    while (checkCells.Count > 0)
                    {
                        var (boxx, boxy) = checkCells.Dequeue();
                        if (m is '<' or '>')
                        {
                            var c = map[boxy][boxx + (m == '<' ? -1 : 2)];
                            if (c == '#') return false;
                            moveBoxes.Push((boxx, boxy));
                            if (c == (m == '<' ? ']' : '[')) checkCells.Enqueue((boxx + (m == '<' ? -2 : 2), boxy));
                        }
                        else
                        {
                            var (c1, c2) = (map[boxy + dy][boxx], map[boxy + dy][boxx + 1]);
                            if (c1 is '#' || c2 is '#') return false;
                            moveBoxes.Push((boxx, boxy));
                            if (c1 == '[') checkCells.Enqueue((boxx, boxy + dy)); // straight ahead
                            if (c1 == ']') checkCells.Enqueue((boxx - 1, boxy + dy)); // left
                            if (c2 == '[') checkCells.Enqueue((boxx + 1, boxy + dy)); // right
                        }
                    }

                    // if we got here, we can move the boxes
                    while (moveBoxes.Count > 0)
                    {
                        var (boxx, boxy) = moveBoxes.Pop();
                        (map[boxy]     [boxx],      map[boxy]     [boxx + 1])      = ('.', '.');
                        (map[boxy + dy][boxx + dx], map[boxy + dy][boxx + dx + 1]) = ('[', ']');
                    }

                    return true;
                }

                void DoMoveRobot()
                {
                    // move robot
                    map[ry][rx] = '.';
                    ry += dy;
                    rx += dx;
                    map[ry][rx] = '@';
                }
            }
        }

        static void ExecuteInstructions(string instructions, Action<int, int, char> moveRobot)
        {
            foreach (var m in instructions)
            {
                var (dx, dy) = m switch
                {
                    '<' => (-1, 0),
                    '>' => (+1, 0),
                    '^' => (0, -1),
                    'v' or _  => (0, +1),
                };
                moveRobot(dx, dy, m);
            }
        }

        static (int x, int y) GetRobot(char[][] map)
        {
            for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                if (map[y][x] == '@')
                    return (x, y);

            throw new Exception("Robot not found");
        }

        static void ScaleUp(char[][] map)
        {
            for (var y = 0; y < map.Length; y++)
            {
                var line = map[y];
                var newLine = map[y] = new char[line.Length * 2];

                for (var x = 0; x < line.Length; x++)
                    (newLine[x * 2], newLine[x * 2 + 1]) = line[x] switch
                    {
                        '#' => ('#', '#'),
                        '.' => ('.', '.'),
                        'O' => ('[', ']'),
                        '@' or _ => ('@', '.'),
                    };
            }
        }

        static (char[][] map, string instructions) Parse(string[] lines)
        {
            var y = lines.Count(l => l.Length > 0 && l[0] == '#');
            var map = new char[y][];

            for (var i = 0; i < y; i++)
                map[i] = lines[i].ToCharArray();

            var instructions = "";

            for (var i = y + 1; i < lines.Length; i++)
                instructions += lines[i];

            return (map, instructions);
        }
    }
}