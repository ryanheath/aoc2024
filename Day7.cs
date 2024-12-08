static partial class Aoc2024
{
    public static void Day7()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input =
                """
                190: 10 19
                3267: 81 40 27
                83: 17 5
                156: 15 6
                7290: 6 8 6 15
                161011: 16 10 13
                192: 17 8 14
                21037: 9 7 18 13
                292: 11 6 16 20
                """.ToLines();
            Part1(input).Should().Be(3749);
            Part2(input).Should().Be(11387);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(4364915411363);
            Part2(input).Should().Be(38322057216320);
        }

        long Part1(string[] lines) => TestLines(lines, Div, Sub);
        long Part2(string[] lines) => TestLines(lines, Div, Sub, Spl);

        static long TestLines(string[] lines, params Func<long, long, long>[] operations)
            => Parse(lines)
            .Where(x => IsValid((x.testValue, x.values), operations))
            .Sum(x => x.testValue);

        static long Div(long a, long b)
        {
            var (div, mod) = Math.DivRem(a, b);
            return mod == 0 ? div : -1;
        }
        static long Sub(long a, long b) => a - b;
        static long Spl(long a, long b) => Div(a - b, PowLogApprox(b));
        static long PowLogApprox(long b) => b switch
        {
            < 10 => 10,
            < 100 => 100,
            < 1000 => 1000,
            _ => 10000
        };

        static bool IsValid((long testValue, long[] values) line, params ReadOnlySpan<Func<long, long, long>> operations)
        {
            // process backwards to reduce the search space and speed up runtime
            // see explanation at https://www.reddit.com/r/adventofcode/comments/1h8l3z5/comment/m0yo4ba

            var resultValues = new Stack<(long result, int nextIndex)>();
            var lastIndex = line.values.Length - 1;

            resultValues.Push((line.testValue, line.values.Length - 1));

            while (resultValues.Count > 0)
            {
                var (result, nextIndex) = resultValues.Pop();
                var nextValue = line.values[nextIndex];

                foreach (var operation in operations)
                {
                    var nextResult = operation(result, nextValue);

                    if (nextIndex == 0 && nextResult == 0)
                    {
                        return true;
                    }
                    else if (nextResult > 0 && nextIndex > 0)
                    {
                        resultValues.Push((nextResult, nextIndex - 1));
                    }
                }
            }

            return false;
        }

        static List<(long testValue, long[] values)> Parse(string[] lines)
        {
            var result = new List<(long testValue, long[] values)>();
            foreach (var line in lines)
            {
                var parts = line.Split(": ");
                result.Add((parts[0].ToLong(), parts[1].ToLongs(" ")));
            }
            return result;
        }
    }
}