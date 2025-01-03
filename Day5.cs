﻿static partial class Aoc2024
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
            Part2(input).Should().Be(123);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(5391);
            Part2(input).Should().Be(6142);
        }

        int Part1(string[] lines)
        {
            var (comparer, pages) = Parse(lines);

            return pages
                .Where(x => SortedCorrectly(x, comparer))
                .Sum(x => x[x.Length/2]);
        }
        int Part2(string[] lines)
        {
            var (comparer, pages) = Parse(lines);

            return pages
                .Where(x => !SortedCorrectly(x, comparer))
                .Sum(x => x.Order(comparer).ElementAt(x.Length/2));
        }

        static bool SortedCorrectly(int[] page, ComparePages comparer)
            => page.SequenceEqual(page.Order(comparer));

        static (ComparePages comparer, List<int[]> pages) Parse(string[] lines)
        {
            HashSet<(int, int)> rules = [];
            List<int[]> pages = [];
            ComparePages compare = new(rules);

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

            return (compare, pages);
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