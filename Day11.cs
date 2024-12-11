static partial class Aoc2024
{
    public static void Day11()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                125 17
                """.ToLines();
            Part1(input).Should().Be(55312);
            Part2(input).Should().Be(65601038650482);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(197357);
            Part2(input).Should().Be(234568186890978);
        }

        long Part1(string[] lines) => Blinks(Parse(lines), 25);
        long Part2(string[] lines) => Blinks(Parse(lines), 75);

        static long Blinks(long[] stones, int blinks)
        {
            var seededStones = new Queue<(long id, long count)>(stones.Select(x => (x, 1L)));
            var seen = new Dictionary<long, long>();

            for (var i = 0; i < blinks; i++)
            {
                Blink(seededStones, seen);
            }

            return seededStones.ToArray().Sum(x => x.count);

            static void Blink(Queue<(long id, long count)> seededStones, Dictionary<long, long> seen)
            {
                while (seededStones.Count > 0)
                {
                    var stone = seededStones.Dequeue();

                    if (stone.id == 0)
                    {
                        Add(seen, 1, stone.count);
                    }
                    else
                    {
                        var (splitted, left, right) = Split(stone.id);
                        if (splitted)
                        {
                            Add(seen, left, stone.count);
                            Add(seen, right, stone.count);
                        }
                        else
                        {
                            Add(seen, stone.id * 2024, stone.count);
                        }
                    }
                }

                foreach (var (id, count) in seen)
                {
                    seededStones.Enqueue((id, count));
                }
                seen.Clear();

                static void Add(Dictionary<long, long> seen, long id, long count)
                {
                    if (seen.TryGetValue(id, out var c))
                    {
                        seen[id] = c + count;
                    }
                    else
                    {
                        seen[id] = count;
                    }
                }
            }

            static (bool splitted, long left, long right) Split(long n)
            {
                var numberOfDigits = NumberOfDigits(n);
                if (numberOfDigits % 2 != 0) return (false, 0, 0);

                var pow10 = Pow10(numberOfDigits / 2);
                return (true, n / pow10, n % pow10);

                static int NumberOfDigits(long n) => (int)Math.Floor(Math.Log10(n) + 1);
                static long Pow10(int p) => (long)Math.Pow(10, p);
            }
        }

        static long[] Parse(string[] lines) => lines[0].ToLongs(" ");
    }
}