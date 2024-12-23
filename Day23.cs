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
            NumberOfSetsOf(Parse(input), startT: false).Should().Be(12);
            Part1(input).Should().Be(7);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(893);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => NumberOfSetsOf(Parse(lines));
        int Part2(string[] lines) => 0;

        static int NumberOfSetsOf(Dictionary<string, List<string>> lan, bool startT = true)
        {
            var sets = 0;

            foreach (var (c1, links) in lan.OrderBy(x => x.Key)) // handle in order to avoid double counting
            {
                foreach (var c2 in links)
                {
                    if (string.CompareOrdinal(c1, c2) > 0) continue; // avoid double counting
                    
                    foreach (var c3 in lan[c2])
                    {
                        if (string.CompareOrdinal(c2, c3) > 0) continue; // avoid double counting

                        if (!links.Contains(c3)) continue; // not connected

                        if (startT && c1[0] != 't' && c2[0] != 't' && c3[0] != 't') continue;

                        sets++;
                    }
                }
            }

            return sets;
        }

        static Dictionary<string, List<string>> Parse(string[] lines)
        {
            var lan = new Dictionary<string, List<string>>();
            foreach (var line in lines)
            {
                var links = line.Split('-');
                if (!lan.ContainsKey(links[0]))
                    lan[links[0]] = new List<string>();
                lan[links[0]].Add(links[1]);
                if (!lan.ContainsKey(links[1]))
                    lan[links[1]] = new List<string>();
                lan[links[1]].Add(links[0]);
            }
            return lan;
        }
    }
}