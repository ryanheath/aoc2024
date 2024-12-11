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
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(197357);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => Blinks(Parse(lines), 25).Count;
        int Part2(string[] lines) => 0;

        static LinkedList<long> Blinks(LinkedList<long> stones, int blinks)
        {
            for (var i = 0; i < blinks; i++)
            {
                Blink();
            }

            return stones;

            void Blink()
            {
                var stone = stones.First!;
                while (stone != null)
                {
                    if (stone.Value == 0)
                    {
                        stone.Value = 1;
                    }
                    else
                    {
                        var (splitted, left, right) = Split(stone.Value);
                        if (splitted)
                        {
                            stones.AddBefore(stone, left);
                            stone.Value = right;
                        }
                        else
                        {
                            stone.Value *= 2024;
                        }
                    }

                    stone = stone.Next;
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

        static LinkedList<long> Parse(string[] lines) => new(lines[0].ToLongs(" "));
    }
}