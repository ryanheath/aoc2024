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
                    CollectionsMarshal.GetValueRefOrAddDefault(seen, id, out _) += count;
                }
            }

            static (bool splitted, long left, long right) Split(long n)
            {
                var numberOfDigits = NumberOfDigitsApprox(n);
                if (numberOfDigits % 2 != 0) return (false, 0, 0);

                var pow10 = Pow10Approx(numberOfDigits / 2);
                var div = Math.DivRem(n, pow10, out var left);
                return (true, div, left);

                static long Pow10Approx(int p) => p switch
                {
                    0 => 1,
                    1 => 10,
                    2 => 100,
                    3 => 1000,
                    4 => 10000,
                    5 => 100000,
                    _ => 1000000,
                };
                static int NumberOfDigitsApprox(long n) => n switch
                {
                    < 10 => 1,
                    < 100 => 2,
                    < 1000 => 3,
                    < 10000 => 4,
                    < 100000 => 5,
                    < 1000000 => 6,
                    < 10000000 => 7,
                    < 100000000 => 8,
                    < 1000000000 => 9,
                    < 10000000000 => 10,
                    < 100000000000 => 11,
                    _ => 12
                };
            }
        }
        static long[] Parse(string[] lines) => lines[0].ToLongs(" ");
    }
}