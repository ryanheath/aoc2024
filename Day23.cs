static partial class Aoc2024
{
    public static void Day23()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                kh-tc
                qp-kh
                de-cg
                ka-co
                yn-aq
                qp-ub
                cg-tb
                vc-aq
                tb-ka
                wh-tc
                yn-cg
                kh-ub
                ta-co
                de-co
                tc-td
                tb-wq
                wh-td
                ta-ka
                td-qp
                aq-cg
                wq-ub
                ub-vc
                de-ta
                wq-aq
                wq-vc
                wh-yn
                ka-de
                kh-ta
                co-tc
                wh-qp
                tb-vc
                td-yn
                """.ToLines();
            SetsOf3(Parse(input), startT: false).Count.Should().Be(12);
            Part1(input).Should().Be(7);
            Part2(input).Should().Be("co,de,ka,ta");
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(893);
            Part2(input).Should().Be("cw,dy,ef,iw,ji,jv,ka,ob,qv,ry,ua,wt,xz");
        }

        int Part1(string[] lines) => SetsOf3(Parse(lines), startT: true).Count;
        string Part2(string[] lines) => LargestSet(Parse(lines));

        static string LargestSet(Dictionary<int, List<int>> lan)
        {
            var sets = SetsOf3(lan);

            foreach (var (c, links) in lan)
            foreach (var set in sets)
            {
                if (!set.Contains(c)) continue;
                foreach (var link in links)
                {
                    var linked = lan[link];
                    foreach (var c2 in set)
                        if (!linked.Contains(c2)) goto next;
                    set.Add(link);
                }
                next: ;
            }

            return string.Join(',', sets.MaxBy(x => x.Count).Order().Select(Unhash));
        }

        static List<HashSet<int>> SetsOf3(Dictionary<int, List<int>> lan, bool startT = false)
        {
            List<HashSet<int>> sets = new();

            foreach (var (c1, links) in lan.OrderBy(x => x.Key)) // handle in order to avoid double counting
            foreach (var c2 in links)
            {
                if (c1 > c2) continue; // avoid double counting
                
                foreach (var c3 in lan[c2])
                {
                    if (c2 > c3) continue; // avoid double counting

                    if (!links.Contains(c3)) continue; // not connected

                    if (startT && UnhashFirst(c1) != 't' && UnhashFirst(c2) != 't' && UnhashFirst(c3) != 't') continue;

                    sets.Add([c1, c2, c3]);
                }
            }

            return sets;
        }

        static Dictionary<int, List<int>> Parse(string[] lines)
        {
            var lan = new Dictionary<int, List<int>>();
            foreach (var line in lines)
            {
                var links = line.Split('-');
                var c1 = Hash(links[0]);
                var c2 = Hash(links[1]);
                if (!lan.ContainsKey(c1))
                    lan[c1] = new List<int>();
                lan[c1].Add(c2);
                if (!lan.ContainsKey(c2))
                    lan[c2] = new List<int>();
                lan[c2].Add(c1);
            }
            return lan;
        }

        static int Hash(string s) => (s[0] - 'a') * 26 + (s[1] - 'a');
        static char UnhashFirst(int i) => (char)('a' + i / 26);
        static string Unhash(int i) => $"{UnhashFirst(i)}{(char)('a' + i % 26)}";
    }
}