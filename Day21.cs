static partial class Aoc2024
{
    public static void Day21()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                029A
                980A
                179A
                456A
                379A
                """.ToLines();
            Part1(input).Should().Be(126384);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(163920);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => GetSumOfComplexity(lines);
        int Part2(string[] lines) => 0;

        static int GetSumOfComplexity(string[] lines)
        {
            var codes = Parse(lines);

            var sum = 0;
            foreach (var code in codes)
            {
                var smallest = int.MaxValue;
                foreach (var code2 in TraversePaths(GetShortestPaths(code, NumericKeyPad)))
                foreach (var code3 in TraversePaths(GetShortestPaths(code2, DirectionalKeyPad)))
                foreach (var code4 in TraversePaths(GetShortestPaths(code3, DirectionalKeyPad)).Take(1))
                    smallest = Math.Min(smallest, code4.Count);
                
                var num = new string(code.Where(c => c != 'A').ToArray()).ToInt();
                sum += smallest * num;
            }

            return sum;

            static IEnumerable<List<char>> TraversePaths(List<List<List<char>>> paths)
            {
                var idxs = new int[paths.Count];
                var path = new List<char>();
                
                while (true)
                {
                    path.Clear();
                    for (var i = 0; i < paths.Count; i++)
                    {
                        path.AddRange(paths[i][idxs[i]]);
                    }
                    yield return path;

                    var pos = paths.Count - 1;
                    while (pos >= 0)
                    {
                        idxs[pos]++;
                        if (idxs[pos] < paths[pos].Count) break;
                        
                        idxs[pos] = 0;
                        pos--;
                    }

                    if (pos < 0) break;
                }
            }
        }

        static List<List<List<char>>> GetShortestPaths(List<char> code, Dictionary<char, int> keypad)
        {
            var paths = new List<List<List<char>>>();
            var posi = keypad['A'];
            var block = keypad['#'];

            foreach (var c in code)
            {
                var idx = keypad[c];

                List<List<char>> segmentPaths = new();
                var distance = idx - posi;
                var (dx, dy) = (Math.Clamp(idx % 3 - posi % 3, -1, 1), idx / 3 == posi / 3 ? 0 : 3 * Math.Sign(distance));
                var (cx, cy) = (dx > 0 ? '>' : '<', dy > 0 ? 'v' : '^');

                var queue = new Queue<(int, List<char>)>();
                queue.Enqueue((posi, new List<char>()));

                while (queue.Count > 0)
                {
                    var (pos, p) = queue.Dequeue();

                    if (pos == block) 
                        continue;
                    
                    if (pos == idx)
                    {
                        p.Add('A');
                        segmentPaths.Add(p);
                        continue;
                    }

                    if (dx != 0 && (pos % 3) != (idx % 3))
                        queue.Enqueue((pos + dx, [..p, cx]));
                    if  (dy != 0 && (pos / 3) != (idx / 3))
                        queue.Enqueue((pos + dy, [..p, cy]));
                }
            
                paths.Add(segmentPaths);
                posi = idx;
            }

            return paths;
        }
        static List<List<char>> Parse(string[] lines) => lines.Select(line => line.ToList()).ToList();
    }

    static readonly Dictionary<char, int> NumericKeyPad = new Dictionary<char, int>
    {
        ['7'] = 0x0, ['8'] = 0x1, ['9'] = 0x2,
        ['4'] = 0x3, ['5'] = 0x4, ['6'] = 0x5,
        ['1'] = 0x6, ['2'] = 0x7, ['3'] = 0x8,
        ['#'] = 0x9, ['0'] = 0xA, ['A'] = 0xB,
    };

    static readonly Dictionary<char, int> DirectionalKeyPad = new Dictionary<char, int>
    {
        ['#'] = 0x0, ['^'] = 0x1, ['A'] = 0x2,
        ['<'] = 0x3, ['v'] = 0x4, ['>'] = 0x5,
    };
}