static partial class Aoc2024
{
    public static void Day12()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                RRRRIICCFF
                RRRRIICCCF
                VVRRRCCFFF
                VVRCCCJFFF
                VVVVCJJCFE
                VVIVCCJJEE
                VVIIICJJEE
                MIIIIIJJEE
                MIIISIJEEE
                MMMISSJEEE
                """.ToLines();
            Part1(input).Should().Be(1930);
            Part2(input).Should().Be(1206);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(1431316);
            Part2(input).Should().Be(821428);
        }

        int Part1(string[] lines) => Parse(lines).Sum(x => x.plots.Count * GetPerimeter(x.plots));
        int Part2(string[] lines) => Parse(lines).Sum(x => x.plots.Count * GetSides(x.plots));

        static int GetSides(HashSet<(int x, int y)> plots)
        {
            var sides = new List<((int p, Direction d) ax, HashSet<int> qs)>();

            foreach (var (x, y) in plots)
            {
                ConstructSide(Direction.N);
                ConstructSide(Direction.E);
                ConstructSide(Direction.S);
                ConstructSide(Direction.W);

                void ConstructSide(Direction d)
                {
                    var (dx, dy, ax, q) = d switch
                    {
                        Direction.N => (x, y - 1, y, x),
                        Direction.E => (x + 1, y, x, y),
                        Direction.S => (x, y + 1, y, x),
                        Direction.W => (x - 1, y, x, y),
                        _ => throw new ArgumentOutOfRangeException(nameof(d))
                    };
                    if (!plots.Contains((dx, dy)))
                    {
                        var others = sides.FindAll(s => s.ax == (ax, d) && (s.qs.Contains(q - 1) || s.qs.Contains(q + 1)));
                        if (others is []) 
                        { 
                            sides.Add(((ax, d), [q]));
                        }
                        else 
                        {
                            var keep = others[0];
                            foreach (var s in others.Skip(1))
                            {
                                keep.qs.UnionWith(s.qs);
                                sides.Remove(s);
                            }
                            keep.qs.Add(q);
                        }
                    }
                }
            }

            return sides.Count;
        }

        static int GetPerimeter(HashSet<(int x, int y)> plots)
        {
            var p = 0;

            foreach (var (x, y) in plots)
            {
                var fences = 4;

                if (plots.Contains((x, y - 1))) fences--;
                if (plots.Contains((x + 1, y))) fences--;
                if (plots.Contains((x, y + 1))) fences--;
                if (plots.Contains((x - 1, y))) fences--;

                p += fences;
            }

            return p;
        }

        static List<(char plant, HashSet<(int x, int y)> plots)> Parse(string[] lines)
        {
            var regions = new List<(char plant, HashSet<(int x, int y)> plots)>();

            for (var y = 0; y < lines.Length; y++)
            for (var x = 0; x < lines[0].Length; x++)
            {
                var plant = lines[y][x];

                var nIndex = regions.FindIndex(r => r.plant == plant && r.plots.Contains((x, y - 1)));
                var wIndex = regions.FindIndex(r => r.plant == plant && r.plots.Contains((x - 1, y)));

                if (nIndex == -1 && wIndex == -1)
                {
                    regions.Add((plant, [(x, y)]));
                }
                else if (nIndex == -1 && wIndex != -1)
                {
                    regions[wIndex].plots.Add((x, y));
                }
                else if (nIndex != -1 && wIndex == -1)
                {
                    regions[nIndex].plots.Add((x, y));
                }
                else
                {
                    regions[nIndex].plots.Add((x, y));
                    if (nIndex != wIndex)
                    {
                        regions[nIndex].plots.UnionWith(regions[wIndex].plots);
                        regions.RemoveAt(wIndex);
                    }
                }
            }

            return regions;
        }
    }
}