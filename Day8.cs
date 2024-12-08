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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(276);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            var (antennas, dim) = Parse(lines);

            // group the antennas
            var groupedAntennas = antennas.GroupBy(a => a.antenna).Select(g => g.ToArray()).ToArray();

            HashSet<(int x, int y)> antidiotes = [];

            foreach (var positions in groupedAntennas)
            foreach (var (i, pos1) in positions.Index())
            foreach (var pos2 in positions.Skip(i + 1))
            {
                var dx = pos2.x - pos1.x;
                var dy = pos2.y - pos1.y;

                AddAntidiote(pos1.x - dx, pos1.y - dy);
                AddAntidiote(pos2.x + dx, pos2.y + dy);

                void AddAntidiote(int x, int y)
                {
                    if (x >= 0 && x < dim.maxX && y >= 0 && y < dim.maxY) 
                    {
                        antidiotes.Add((x, y));
                    }
                }
            }

            return antidiotes.Count;
        }
        int Part2(string[] lines) => 0;

        (List<(char antenna, int x, int y)>, (int maxX, int maxY) dim) Parse(string[] lines)
        {
            var antennas = new List<(char antenna, int x, int y)>();

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[y].Length; x++)
                if (lines[y][x] != '.') antennas.Add((lines[y][x], x, y));

            return (antennas, (lines[0].Length, lines.Length));
        }
    }
}