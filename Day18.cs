static partial class Aoc2024
{
    public static void Day18()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                5,4
                4,2
                4,5
                3,0
                2,1
                6,3
                2,4
                1,5
                0,6
                3,3
                2,6
                5,1
                1,2
                5,5
                2,5
                6,5
                1,4
                0,4
                6,4
                1,1
                6,1
                1,0
                0,5
                1,6
                2,0
                """.ToLines();
            Part1(input, (7, 7), 12).Should().Be(22);
            Part2(input, (7, 7)).Should().Be((6,1));
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, (71, 71), 1024).Should().Be(370);
            Part2(input, (71, 71)).Should().Be((65,6));
        }

        int Part1(string[] lines, (int x, int y) dim, int bytesFallen) => GetSmallestStep(dim, new (Parse(lines).Take(bytesFallen)));
        (int, int) Part2(string[] lines, (int x, int y) dim) => GetHaltingByte(dim, new (Parse(lines)));

        static (int,int) GetHaltingByte((int x, int y) dim, List<(int x, int y)> bytes)
        {
            for (var i = 0; i < bytes.Count; i++)
                if (GetSmallestStep(dim, new (bytes[..i])) == int.MaxValue)
                    return bytes[i - 1];

            return (0, 0);
        }

        static int GetSmallestStep((int x, int y) dim, HashSet<(int x, int y)> bytes)
        {
            var trailheads = new Queue<(int x, int y, int steps)>();
            trailheads.Enqueue(new (0, 0, 1));

            var seen = new Dictionary<(int x, int y), int>();
            seen[(0, 0)] = 0;

            while (trailheads.Count > 0)
            {
                var trail = trailheads.Dequeue(); 
                Extend( 0, -1);
                Extend(+1,  0);
                Extend( 0, +1);
                Extend(-1,  0);

                void Extend(int dx, int dy)
                {
                    var nx = trail.x + dx;
                    var ny = trail.y + dy;
                    var steps = trail.steps + 1;

                    if (nx < 0 || nx >= dim.x || ny < 0 || ny >= dim.y
                        || bytes.Contains((nx, ny))
                        || seen.TryGetValue((nx, ny), out var seenSteps) && seenSteps <= steps)
                        return;

                    seen[(nx, ny)] = steps;

                    if (nx == dim.x - 1 && ny == dim.y - 1)
                        return;
                    
                    trailheads.Enqueue(new(nx, ny, steps));
                }
            }

            return seen.TryGetValue((dim.x-1, dim.y-1), out var steps) ? steps - 1 : int.MaxValue;
        }

        static List<(int x, int y)> Parse(string[] lines) 
            => lines.Select(l => l.AsSpan().To2Ints(",")).ToList();
    }
}