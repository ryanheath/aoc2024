static partial class Aoc2024
{
    public static void Day2()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                7 6 4 2 1
                1 2 7 8 9
                9 7 6 2 1
                1 3 2 4 5
                8 6 4 4 1
                1 3 6 7 9
                """.ToLines();
            Part1(input).Should().Be(2);
            Part2(input).Should().Be(4);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(359);
            Part2(input).Should().Be(418);
        }

        int Part1(string[] lines) => lines.ToInts(" ").Count(IsSafe);
        int Part2(string[] lines) => lines.ToInts(" ").Count(IsSafeDampened);

        static bool IsSafe(int[] values)
        {
            var (isSafe, _) = IsSafeLevels(values);
            return isSafe;
        }

        static bool IsSafeDampened(int[] values)
        {
            var (isSafe, unsafeIndex) = IsSafeLevels(values);

            if (isSafe)
            {
                return true;
            }

            // retry at unsafeIndex-1 , unsafeIndex, unsafeIndex+1
            for (var i = unsafeIndex > 0 ? unsafeIndex - 1 : unsafeIndex; i <= unsafeIndex + 1; i++)
            {
                (isSafe, _) = IsSafeLevels([..values[..i], ..values[(i + 1)..]]);
                if (isSafe)
                {
                    return true;
                }
            }

            return false;
        }

        static (bool, int) IsSafeLevels(int[] values)
        {
            var (increasing, isSafeDiff) = Change(values[0], values[1]);

            if (!isSafeDiff)
            {
                return (false, 0);
            }

            for (var i = 1; i < values.Length - 1; i++)
            {
                var (increase, safeDiff) = Change(values[i], values[i + 1]);
                if (!safeDiff || increase != increasing)
                {
                    return (false, i);
                }
            }

            return (true, -1);

            (bool, bool) Change(int a, int b)
            {
                var diff = b - a;
                var increase = Math.Sign(diff) == 1;
                var amount = Math.Abs(diff);
                return (increase, amount is 1 or 2 or 3);
            } 
        }
    }
}