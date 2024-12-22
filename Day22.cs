static partial class Aoc2024
{
    public static void Day22()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                123
                """.ToLines();
            Part1(input, 10).Should().Be(5908254);
            input = 
                """
                1
                10
                100
                2024
                """.ToLines();
            Part1(input, 2000).Should().Be(37327623);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, 2000).Should().Be(15608699004);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines, int nth) => lines.ToInts().Sum(s => GetNextSecret(s, nth));
        int Part2(string[] lines) => 0;

        static long GetNextSecret(long secret, int nth)
        {
            for (var i = 0; i < nth; i++)
            {
                secret = MixAndPrune(secret, secret * 64);
                secret = MixAndPrune(secret, secret / 32);
                secret = MixAndPrune(secret, secret * 2048);
            }

            return secret;

            static long MixAndPrune(long a, long b) => Prune(Mix(a, b));
            static long Mix(long a, long b) => a ^ b;
            static long Prune(long n) => n % 0x1000000;
        }
    }
}