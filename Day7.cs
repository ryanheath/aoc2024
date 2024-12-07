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

        long Part1(string[] lines) => TestLines(lines, Multiply, Add);
        long Part2(string[] lines) => TestLines(lines, Multiply, Add, Concat);

        static long TestLines(string[] lines, params Func<long, long, long>[] operations)
            => Parse(lines)
            .Where(x => IsValid((x.testValue, x.values), operations))
            .Sum(x => x.testValue);

        static long Multiply(long a, long b) => a * b;
        static long Add(long a, long b) => a + b;
        static long Concat(long a, long b) => a * (long)Math.Pow(10, Math.Floor(Math.Log10(b) + 1)) + b;

        static bool IsValid((long testValue, long[] values) line, params ReadOnlySpan<Func<long, long, long>> operations)
        {
            var resultValues = new Stack<(long result, int nextIndex)>();
            var lastIndex = line.values.Length - 1;

            resultValues.Push((line.values[0], 1));

            while (resultValues.Count > 0)
            {
                var (result, nextIndex) = resultValues.Pop();
                var nextValue = line.values[nextIndex];

                foreach (var operation in operations)
                {
                    if (TestOperation(operation(result, nextValue)))
                    {
                        return true;
                    }
                }

                bool TestOperation(long nextResult)
                {
                    if (nextIndex == lastIndex && nextResult == line.testValue)
                    {
                        return true;
                    }
                    else if (nextResult <= line.testValue && nextIndex < lastIndex)
                    {
                        resultValues.Push((nextResult, nextIndex + 1));
                    }
                    return false;
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