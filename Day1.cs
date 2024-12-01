static partial class Aoc2024
{
    public static void Day1()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                3   4
                4   3
                2   5
                1   3
                3   9
                3   3
                """.ToLines();
            Part1(input).Should().Be(11);
            Part2(input).Should().Be(31);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(2344935);
            Part2(input).Should().Be(27647262);
        }

        int Part1(string[] lines)
        {
            List<int> left = [];
            List<int> right = [];

            foreach (var line in lines)
            {
                var (l, r) = line.ToInts2(" ");
                left.Add(l);
                right.Add(r);
            }

            left.Sort();
            right.Sort();

            return left
                .Zip(right, (l, r) => Math.Abs(r - l))
                .Sum();
        }

        int Part2(string[] lines)
        {
            List<int> left = [];
            Dictionary<int, int> right = [];

            foreach (var line in lines)
            {
                var (l, r) = line.ToInts2(" ");
                left.Add(l);
                CollectionsMarshal.GetValueRefOrAddDefault(right, r, out _) += 1;
            }

            return left.Sum(l => l * right.GetValueOrDefault(l));
        }
    }
}