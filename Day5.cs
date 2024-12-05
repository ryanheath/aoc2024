static partial class Aoc2024
{
    public static void Day5()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                47|53
                97|13
                97|61
                97|47
                75|29
                61|13
                75|53
                29|13
                97|29
                53|29
                61|53
                97|53
                61|29
                47|13
                75|47
                97|75
                47|61
                75|61
                47|29
                75|13
                53|13

                75,47,61,53,29
                97,61,53,29,13
                75,29,13
                75,97,47,61,53
                61,13,29
                97,13,75,29,47
                """.ToLines();
            Part1(input).Should().Be(143);
            Part2(input).Should().Be(6142);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(5391);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines)
        {
            HashSet<(int, int)> rules = [];
            List<int[]> pages = [];
            ComparePages compare = new(rules);

            Parse();

            return pages.Where(SortedCorrectly).Sum(x => x[x.Length/2]);

            void Parse()
            {
                var i = 0;
                for (i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (line == "") break;
                    rules.Add(line.AsSpan().To2Ints("|"));
                }
                for (i++; i < lines.Length; i++)
                {
                    pages.Add(lines[i].ToInts(","));
                }
            }

            bool SortedCorrectly(int[] page) => page.SequenceEqual(page.Order(compare));
        }
        int Part2(string[] lines)
        {
            HashSet<(int, int)> rules = [];
            List<int[]> pages = [];
            ComparePages compare = new(rules);

            Parse();

            return pages
                .Where(x => !SortedCorrectly(x))
                .Sum(x => x.Order(compare).ElementAt(x.Length/2));

            void Parse()
            {
                var i = 0;
                for (i = 0; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (line == "") break;
                    rules.Add(line.AsSpan().To2Ints("|"));
                }
                for (i++; i < lines.Length; i++)
                {
                    pages.Add(lines[i].ToInts(","));
                }
            }

            bool SortedCorrectly(int[] page) => page.SequenceEqual(page.Order(compare));
        }
    }

    class ComparePages(HashSet<(int, int)> rules) : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if (rules.Contains((x, y))) return -1;
            if (rules.Contains((y, x))) return 1;
            return 0;
        }
    }
}