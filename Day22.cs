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
            input = 
                """
                1
                2
                3
                2024
                """.ToLines();
            Part2(input, 2000).Should().Be(23);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input, 2000).Should().Be(15608699004);
            Part2(input, 2000).Should().Be(1791);
        }

        long Part1(string[] lines, int nth) => lines.ToInts().Sum(s => GetNextSecret(s, nth));
        int Part2(string[] lines, int nth) => GetBestSequencePrice(lines.ToLongs(), nth);

        static int GetBestSequencePrice(long[] secrets, int nth)
        {
            var secretSequences = new Dictionary<(int, int, int, int), int>();
            var prices = new int[nth];
            var changes = new int[nth + 1];
            Dictionary<(int, int, int, int), int> priceSequences = new();

            for (var i = 0; i < secrets.Length; i++)
            {
                GetPricesAndChanges(secrets[i], nth, prices, changes);
                GetPriceSequences(prices, changes, priceSequences);
                foreach (var (sequence, price) in priceSequences)
                    CollectionsMarshal.GetValueRefOrAddDefault(secretSequences, sequence, out _) += price;
            }

            return secretSequences.Max(kv => kv.Value);
        }

        static void GetPricesAndChanges(long secret, int nth, int[] prices, int[] changes)
        {
            for (var i = 0; i < nth + 1; i++)
            {
                secret = NextSecret(secret);
                var price = secret % 10;
                if (i != nth) prices[i] = (int)price;
                if (i != 0) changes[i] = (int)price - prices[i - 1];
            }
        }

        static void GetPriceSequences(int[] prices, int[] changes, Dictionary<(int, int, int, int), int> priceSequences)
        {
            priceSequences.Clear();
            for (var i = 1; i < prices.Length - 4; i++)
            {
                var sequence = (changes[i+0], changes[i+1], changes[i+2], changes[i+3]);
                if (!priceSequences.ContainsKey(sequence))
                    priceSequences.Add(sequence, prices[i+3]);
            }
        }

        static long GetNextSecret(long secret, int nth)
        {
            for (var i = 0; i < nth; i++)
                secret = NextSecret(secret);

            return secret;
        }

        static long NextSecret(long secret)
        {
            secret = MixAndPrune(secret, secret * 64);
            secret = MixAndPrune(secret, secret / 32);
            secret = MixAndPrune(secret, secret * 2048);
            
            return secret;

            static long MixAndPrune(long a, long b) => Prune(Mix(a, b));
            static long Mix(long a, long b) => a ^ b;
            static long Prune(long n) => n % 0x1000000;
        }
    }
}