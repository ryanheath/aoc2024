static partial class Aoc2024
{
    public static void Day17()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                Register A: 729
                Register B: 0
                Register C: 0

                Program: 0,1,5,4,3,0
                """.ToLines();
            Part1(input).Should().BeEquivalentTo([4,6,3,5,6,3,5,2,1,0]);
            input = 
                """
                Register A: 2024
                Register B: 0
                Register C: 0

                Program: 0,3,5,4,3,0
                """.ToLines();
            Part2(input, one: true).Should().Be(117440);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().BeEquivalentTo([2,1,3,0,5,2,3,7,1]);
            Part2(input, one: false).Should().Be(107416732707226);
        }

        int[] Part1(string[] lines) => Run(Parse(lines)).ToArray();
        long Part2(string[] lines, bool one) => one ? FindCopy1(Parse(lines)) : FindCopy2(Parse(lines));

        static long FindCopy1((long a, long b, long c, int[] byteCode) input)
        {
            var (_, b, c, byteCode) = input;

            // 32_768 first length of 6
            // 32_768 => 0,0,0,0, 1,0
            // pow 8     0 3 5 4 +2 0

            var a = 32_768 + Pow8(2)*3 + Pow8(3)*5 + Pow8(4)*4 + Pow8(5)*2;
            var output = Run((a, b, c, byteCode)).ToArray();

            if (output.SequenceEqual(byteCode))
            {
                return a;
            }
            
            return 0;

            static long Pow8(int n) => (long)Math.Pow(8, n);
        }

        static long FindCopy2((long a, long b, long c, int[] byteCode) input)
        {
            var (_, b, c, byteCode) = input;

            // pow(15) first length of 16
            // 107416749484442 is too high
            var tooHigh = 107416749484442;
            var a = tooHigh;
            var lowestA = a;
            for (; a > Pow8(15); a -= 2097152) // octal 10_000_000
            {
                var output = Run((a, b, c, byteCode)).ToArray();
                if (a != tooHigh && output.SequenceEqual(byteCode))
                {
                    lowestA = a;
                    break;
                }
            }

            return lowestA;

            static long Pow8(int n) => (long)Math.Pow(8, n);
        }

        static IEnumerable<int> Run((long a, long b, long c, int[] byteCode) input)
        {
            var (a, b, c, byteCode) = input;
            var ip = 0;

            while (ip < byteCode.Length)
            {
                var instr = byteCode[ip++];
                var opr = byteCode[ip++];

                switch (instr)
                {
                    case 0: adv(); break;
                    case 1: bxl(); break;
                    case 2: bst(); break;
                    case 3: jnz(); break;
                    case 4: bxc(); break;
                    case 5: yield return (int)mod8(); break;
                    case 6: bdv(); break;
                    case 7: cdv(); break;
                    default: throw new UnreachableException();
                }

                void adv() => a = dv();
                void bxl() => b = xor(b, opr);
                void bst() => b = mod8();
                void jnz() => ip = a == 0 ? ip : opr;
                void bxc() => b = xor(b, c);
                void bdv() => b = dv();
                void cdv() => c = dv();
                long cmb(int opr) => opr switch
                {
                    0 or 1 or 2 or 3 => opr,
                    4 => a,
                    5 => b,
                    6 => c,
                    _ => throw new UnreachableException()
                };

                long dv() => a / (long)Math.Pow(2, cmb(opr));
                long xor(long x, long y) => x ^ y;
                long mod8() => cmb(opr) & 0b111;
            }
        }

        static (int a, int b, int c, int[] byteCode) Parse(string[] lines)
        {
            var a = lines[0]["Register A: ".Length..].ToInt();
            var b = lines[1]["Register B: ".Length..].ToInt();
            var c = lines[2]["Register C: ".Length..].ToInt();
            var byteCode = lines[4]["Program: ".Length..].ToInts(",");
            return (a, b, c, byteCode);
        }
    }
}