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
            List<int> leftList = [];
            List<int> rightList = [];

            foreach (var line in lines)
            {
                var parts = line.ToInts(" ");
                leftList.Add(parts[0]);
                rightList.Add(parts[1]);
            }

            leftList.Sort();
            rightList.Sort();

            return leftList
                .Zip(rightList, (l, r) => Math.Abs(r - l))
                .Sum();
        }

        int Part2(string[] lines)
        {
            List<int> leftList = [];
            Dictionary<int, int> rightList = [];

            foreach (var line in lines)
            {
                var parts = line.ToInts(" ");
                leftList.Add(parts[0]);
                var r = parts[1];
                rightList.TryGetValue(r, out var count);
                count++;
                rightList[r] = count;
            }

            return leftList
                .Select(l => 
                {
                    rightList.TryGetValue(l, out var count);
                    return l * count;
                })
                .Sum();
        }
    }
}