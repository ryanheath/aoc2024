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

        int Part1(string[] lines) => GetSumGPS(RunInstructions(lines), 'O');
        int Part2(string[] lines) => GetSumGPS(RunInstructionsScaleUp(lines), '[');

        static int GetSumGPS(char[][] map, char search)
        {
            var sum = 0;

            for (var y = 0; y < map.Length; y++)
            for (var x = 0; x < map[y].Length; x++)
                if (map[y][x] == search)
                    sum += y * 100 + x;

            return sum;
        }

        static char[][] RunInstructions(string[] lines)
        {
            var (map, instructions) = Parse(lines);

            var (rx, ry) = GetRobot(map);

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
                if (c == '.')
                {
                    DoMoveRobot();
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
                        // move box(es)
                        map[ry + ddy][rx + ddx] = 'O';

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

            foreach (var m in instructions)
            {
                var (dx, dy) = m switch
                {
                    '<' => (-1, 0),
                    '>' => (+1, 0),
                    '^' => (0, -1),
                    'v' or _  => (0, +1),
                };
                MoveRobot(dx, dy, m);
            }

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
                        if (m == '<')
                        {
                            var c = map[boxy][boxx - 1];
                            if (c == '#') return false;
                            moveBoxes.Push((boxx, boxy));
                            if (c == ']') checkCells.Enqueue((boxx - 2, boxy));
                        }
                        else if (m == '>')
                        {
                            var c = map[boxy][boxx + 2];
                            if (c == '#') return false;
                            moveBoxes.Push((boxx, boxy));
                            if (c == '[') checkCells.Enqueue((boxx + 2, boxy));
                        }
                        else
                        {
                            var (c1, c2) = (map[boxy + dy][boxx], map[boxy + dy][boxx + 1]);
                            if (c1 == '#' || c2 == '#') return false;
                            moveBoxes.Push((boxx, boxy));
                            if (c1 == '[' && c2 == ']') checkCells.Enqueue((boxx, boxy + dy));
                            if (c1 == ']') checkCells.Enqueue((boxx - 1, boxy + dy));
                            if (c2 == '[') checkCells.Enqueue((boxx + 1, boxy + dy));
                        }
                    }
                
                    // if we got here, we can move the boxes
                    while (moveBoxes.Count > 0)
                    {
                        var (boxx, boxy) = moveBoxes.Pop();
                        map[boxy][boxx] = '.';
                        map[boxy][boxx + 1] = '.';
                        map[boxy + dy][boxx + dx] = '[';
                        map[boxy + dy][boxx + dx + 1] = ']';
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
                {
                    var (c1, c2) = line[x] switch
                    {
                        '#' => ('#', '#'),
                        '.' => ('.', '.'),
                        'O' => ('[', ']'),
                        '@' or _ => ('@', '.'),
                    };
                    newLine[x * 2] = c1;
                    newLine[x * 2 + 1] = c2;
                }
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