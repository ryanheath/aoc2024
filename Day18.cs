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
            var paths = GetPaths(dim, [], returnEarly: true);
            HashSet<(int x, int y)> fallenBytes = new();

            for (var i = 0; i < bytes.Count; i++)
            {
                var bite = bytes[i];
                fallenBytes.Add(bite);

                // if next byte is on a path remove it from the paths
                paths.RemoveAll(p => p.GetEnumerator().Any(b => b == bite));
                if (paths.Count != 0) continue;

                paths = GetPaths(dim, fallenBytes, returnEarly: true);
                
                if (paths.Count == 0) return bite;
            }

            return (0, 0);
        }

        static int GetSmallestStep((int x, int y) dim, HashSet<(int x, int y)> bytes)
        {
            var paths = GetPaths(dim, bytes);
            return paths.Any() ? paths.Min(p => p.steps) - 1: int.MaxValue;
        }

        static List<ByteNode> GetPaths((int x, int y) dim, HashSet<(int x, int y)> bytes, bool returnEarly = false)
        {
            var paths = new List<ByteNode>();
            var trailheads = new Queue<ByteNode>();
            trailheads.Enqueue(new (0, 0, 1));

            var seen = new Dictionary<(int x, int y), int>();
            seen[(0, 0)] = 0;

            while (trailheads.Count > 0)
            {
                var trail = trailheads.Dequeue(); 
                if (Extend( 0, -1) || Extend(+1,  0) || Extend( 0, +1) || Extend(-1,  0))
                    break;

                bool Extend(int dx, int dy)
                {
                    var nx = trail.x + dx;
                    var ny = trail.y + dy;
                    var steps = trail.steps + 1;

                    if (nx < 0 || nx >= dim.x || ny < 0 || ny >= dim.y
                        || bytes.Contains((nx, ny))
                        || seen.TryGetValue((nx, ny), out var seenSteps) && seenSteps <= steps)
                        return false;

                    seen[(nx, ny)] = steps;

                    if (nx == dim.x - 1 && ny == dim.y - 1)
                    {
                        paths.Add(new(nx, ny, steps, trail));
                        return returnEarly;
                    }
                    
                    trailheads.Enqueue(new(nx, ny, steps, trail));
                    return false;
                }
            }

            return paths;
        }

        static List<(int x, int y)> Parse(string[] lines) 
            => lines.Select(l => l.AsSpan().To2Ints(",")).ToList();
    }
    record class ByteNode(int x, int y, int steps, ByteNode? Previous = null)
    {
        public IEnumerable<(int x, int y)> GetEnumerator()
        {
            for (var node = this; node != null; node = node.Previous)
            {
                yield return (node.x, node.y);
            }
        }
    }
}