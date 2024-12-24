static partial class Aoc2024
{
    public static void Day24()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                x00: 1
                x01: 1
                x02: 1
                y00: 0
                y01: 1
                y02: 0

                x00 AND y00 -> z00
                x01 XOR y01 -> z01
                x02 OR y02 -> z02
                """.ToLines();
            Part1(input).Should().Be(4);
            input = 
                """
                x00: 1
                x01: 0
                x02: 1
                x03: 1
                x04: 0
                y00: 1
                y01: 1
                y02: 1
                y03: 1
                y04: 1

                ntg XOR fgs -> mjb
                y02 OR x01 -> tnw
                kwq OR kpj -> z05
                x00 OR x03 -> fst
                tgd XOR rvg -> z01
                vdt OR tnw -> bfw
                bfw AND frj -> z10
                ffh OR nrd -> bqk
                y00 AND y03 -> djm
                y03 OR y00 -> psh
                bqk OR frj -> z08
                tnw OR fst -> frj
                gnj AND tgd -> z11
                bfw XOR mjb -> z00
                x03 OR x00 -> vdt
                gnj AND wpb -> z02
                x04 AND y00 -> kjc
                djm OR pbm -> qhw
                nrd AND vdt -> hwm
                kjc AND fst -> rvg
                y04 OR y02 -> fgs
                y01 AND x02 -> pbm
                ntg OR kjc -> kwq
                psh XOR fgs -> tgd
                qhw XOR tgd -> z09
                pbm OR djm -> kpj
                x03 XOR y03 -> ffh
                x00 XOR y04 -> ntg
                bfw OR bqk -> z06
                nrd XOR fgs -> wpb
                frj XOR qhw -> z04
                bqk OR frj -> z07
                y03 OR x01 -> nrd
                hwm AND bqk -> z03
                tgd XOR rvg -> z12
                tnw OR pbm -> gnj
                """.ToLines();
            Part1(input).Should().Be(2024);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(0);
            Part2(input).Should().Be(0);
        }

        long Part1(string[] lines) => RunSimulation(Parse(lines));
        int Part2(string[] lines) => 0;

        long RunSimulation((Dictionary<string, bool?> gates, (string, Op, string, string)[] wires) input)
        {
            var dependencies = new Dictionary<string, (string, Op, string)>();

            foreach (var (src1, op, src2, dst) in input.wires)
                dependencies[dst] = (src1, op, src2);

            var result = 0L; var p = 0;
            foreach (var gate in input.gates.Keys.Where(k => k.StartsWith("z")).Order())
                result += ((bool)GetValue(gate)! ? 1L : 0L) << p++;

            return result;

            bool? GetValue(string src) 
            {
                var value = input.gates[src];
                if (value != null) return value;

                if (!dependencies.TryGetValue(src, out var dep)) return null;

                var (src1, op, src2) = dep;
                var v1 = GetValue(src1);
                var v2 = GetValue(src2);
                if (v1 == null || v2 == null) return null;

                input.gates[src] = value = op switch
                {
                    Op.AND => v1 & v2,
                    Op.OR => v1 | v2,
                    Op.XOR => v1 ^ v2,
                    _ => throw new Exception("Unknown op")
                };

                return value;
            }
        }

        (Dictionary<string, bool?> gates, (string, Op, string, string)[] wires) Parse(string[] lines)
        {
            var gates = new Dictionary<string, bool?>();
            var wires = new List<(string, Op, string, string)>();
            var i = 0;
            for ( ; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line == "") break;
                var parts = line.Split(": ");
                gates[parts[0]] = parts[1] == "1";
            }
            i++;

            for ( ; i < lines.Length; i++)
            {
                var line = lines[i];
                var parts = line.Split(" ");
                var op = parts[1] switch
                {
                    "AND" => Op.AND,
                    "OR" => Op.OR,
                    "XOR" => Op.XOR,
                    _ => throw new Exception("Unknown op")
                };
                wires.Add((parts[0], op, parts[2], parts[4]));
                if (!gates.ContainsKey(parts[0])) gates.Add(parts[0], null);
                if (!gates.ContainsKey(parts[2])) gates.Add(parts[2], null);
                if (!gates.ContainsKey(parts[4])) gates.Add(parts[4], null);
            }

            return (gates, wires.ToArray());
        }
    }
    enum Op { AND, OR, XOR }
}