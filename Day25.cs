static partial class Aoc2024
{
    public static void Day25()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                #####
                .####
                .####
                .####
                .#.#.
                .#...
                .....

                #####
                ##.##
                .#.##
                ...##
                ...#.
                ...#.
                .....

                .....
                #....
                #....
                #...#
                #.#.#
                #.###
                #####

                .....
                .....
                #.#..
                ###..
                ###.#
                ###.#
                #####

                .....
                .....
                .....
                #....
                #.#..
                #.#.#
                #####
                """.ToLines();
            Part1(input).Should().Be(3);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(2586);
        }

        int Part1(string[] lines) => GetUniqueKeyLockPairs(Parse(lines));

        static int GetUniqueKeyLockPairs((List<(int, int, int, int, int)> keys, List<(int, int, int, int, int)> locks) input)
        {
            var combos = 0;

            foreach (var key in input.keys)
            foreach (var @lock in input.locks)
                combos += Fits(key, @lock) ? 1 : 0;

            return combos;

            static bool Fits((int a, int b, int c, int d, int e) key, (int a, int b, int c, int d, int e) @lock) =>
                (key.a + @lock.a) <= 5 &&
                (key.b + @lock.b) <= 5 &&
                (key.c + @lock.c) <= 5 &&
                (key.d + @lock.d) <= 5 &&
                (key.e + @lock.e) <= 5;
        }

        static (List<(int, int, int, int, int)> keys, List<(int, int, int, int, int)> locks) Parse(string[] lines)
        {
            var keys = new List<(int, int, int, int, int)>();
            var locks = new List<(int, int, int, int, int)>();

            for (var y = 0; y < lines.Length; y++)
                if (lines[y] == "#####") locks.Add(ReadPins(ref y));
                else if (lines[y] == ".....") keys.Add(ReadPins(ref y));

            return (keys, locks);

            (int, int, int, int, int) ReadPins(ref int ry)
            {
                int a = 0, b = 0, c = 0, d = 0, e = 0;
                for (var y = ry + 1; y <= ry + 5; y++)
                {
                    a += lines[y][0] == '#' ? 1 : 0;
                    b += lines[y][1] == '#' ? 1 : 0;
                    c += lines[y][2] == '#' ? 1 : 0;
                    d += lines[y][3] == '#' ? 1 : 0;
                    e += lines[y][4] == '#' ? 1 : 0;
                }
                ry += 6;
                return (a, b, c, d, e);
            }
        }
    }
}