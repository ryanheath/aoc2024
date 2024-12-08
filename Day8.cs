static partial class Aoc2024
{
    public static void Day8()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                ............
                ........0...
                .....0......
                .......0....
                ....0.......
                ......A.....
                ............
                ............
                ........A...
                .........A..
                ............
                ............
                """.ToLines();
            Part1(input).Should().Be(14);
            Part2(input).Should().Be(34);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(276);
            Part2(input).Should().Be(991);
        }

        int Part1(string[] lines) => CountAntidiotes(lines, addAll: false);
        int Part2(string[] lines) => CountAntidiotes(lines, addAll: true);

        static int CountAntidiotes(string[] lines, bool addAll)
        {
            var (antennas, dim) = Parse(lines);

            // group the antennas
            var groupedAntennas = antennas.GroupBy(a => a.antenna, (_, g) => g.ToArray()).ToArray();

            HashSet<(int x, int y)> antidiotes = [];

            foreach (var positions in groupedAntennas)
            foreach (var (i, pos1) in positions.Index())
            foreach (var pos2 in positions.Skip(i + 1))
            {
                var dx = pos2.x - pos1.x;
                var dy = pos2.y - pos1.y;

                AddAntidiotes(pos1.x, pos1.y, -dx, -dy);
                AddAntidiotes(pos2.x, pos2.y, +dx, +dy);

                void AddAntidiotes(int x, int y, int dx, int dy)
                {
                    if (addAll) antidiotes.Add((x, y));
                    while (true)
                    {
                        x += dx;
                        y += dy;
                        if (x < 0 || x >= dim.maxX || y < 0 || y >= dim.maxY) break;
                        antidiotes.Add((x, y));
                        if (!addAll) break;
                    }
                }
            }

            return antidiotes.Count;
        }

        static (List<(char antenna, int x, int y)>, (int maxX, int maxY) dim) Parse(string[] lines)
        {
            var antennas = new List<(char antenna, int x, int y)>();

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
                if (lines[y][x] != '.') antennas.Add((lines[y][x], x, y));

            return (antennas, (lines[0].Length, lines.Length));
        }
    }
}