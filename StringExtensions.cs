static class StringExtensions
{
    static public IEnumerable<List<string>> GroupLines(this string [] lines)
    {
        var group = new List<string>();

        foreach(var line in lines)
        {
            if (line == "")
            {
                yield return group;
                group = [];
            }
            else
            {
                group.Add(line);
            }
        }

        yield return group;
    }

    static public string[] ToLines(this string input) => input.Split("\r\n");

    static public int[] ToInts(this string input) => input.ToInts("\r\n");

    static public T[] ToNumerics<T>(this string input, string splitter, Func<string, T> parse) 
        => [.. input.Split(splitter, StringSplitOptions.RemoveEmptyEntries).Select(parse)];

    static public (T i0, T i1) To2Numerics<T>(this ReadOnlySpan<char> input, string splitter, Func<ReadOnlySpan<char>, T> parse)
    {
        var enumerator = input.Split(splitter);
        T i0 = parse(input[MoveNext(ref enumerator)]);
        T i1 = parse(input[MoveNext(ref enumerator)]);

        return (i0, i1);

        static Range MoveNext(ref MemoryExtensions.SpanSplitEnumerator<char> enumerator)
        {
            if (!enumerator.MoveNext()) throw new InvalidOperationException();
            var r = enumerator.Current;
            while (r.Start.Equals(r.End))
            {
                if (!enumerator.MoveNext()) throw new InvalidOperationException();
                r = enumerator.Current;
            }
            return r;
        }
    }

    static public int[] ToInts(this string input, string splitter) => input.ToNumerics(splitter, int.Parse);
    static public long[] ToLongs(this string input, string splitter) => input.ToNumerics(splitter, long.Parse);
    static public ulong[] ToULongs(this string input, string splitter) => input.ToNumerics(splitter, ulong.Parse);

    static public (int i0, int i1) To2Ints(this ReadOnlySpan<char> input, string splitter) => input.To2Numerics(splitter, (ReadOnlySpan<char> args) => int.Parse(args));
    static public (long i0, long i1) To2Longs(this ReadOnlySpan<char> input, string splitter) => input.To2Numerics(splitter, (ReadOnlySpan<char> args) => long.Parse(args));

    static public IEnumerable<int[]> ToInts(this string[] lines, string splitter) => lines.Select(line => line.ToInts(splitter));

    static public int[] ToInts(this string[] input) => [.. input.Select(int.Parse)];

    static public string Sort(this string input) => string.Concat(input.OrderBy(c => c));

    static public int ToInt(this string input) => int.Parse(input);
    static public int ToInt(this char input) => input - '0';
    static public int ToIntFromHex(this string input) => int.Parse(input, System.Globalization.NumberStyles.HexNumber);
    static public int? ToNullableInt(this string input) => string.IsNullOrEmpty(input) ? null : int.Parse(input);

    static public long ToLong(this string input) => long.Parse(input);

    static public string ToBits(this int input) => Convert.ToString(input, 2);

    static public string ToBits(this byte input) => Convert.ToString(input, 2);

    static public int IntFromBits(this string input) => input.AsSpan().IntFromBits();

    static public int IntFromBits(this ReadOnlySpan<char> input)
    {
        var v = 0;
        foreach (var c in input)
        {
            v <<= 1;
            if (c == '1')
            {
                v |= 1;
            }
        }

        return v;
    }

    public static string ReverseString(this string s) => new([..s.Reverse()]);

    public static IEnumerable<char> TraverseHorizontal(this string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                yield return lines[y][x];
            }

            // yield a separator between lines
            yield return '\n';
        }
    }

    public static IEnumerable<char> TraverseVertical(this string[] lines)
    {
        for (var x = 0; x < lines[0].Length; x++)
        {
            for (var y = 0; y < lines.Length; y++)
            {
                yield return lines[y][x];
            }

            // yield a separator between lines
            yield return '\n';
        }
    }

    public static IEnumerable<char> TraverseDiagonal(this string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            foreach (var c in TraverseLine(y, 0))
            {
                yield return c;
            }
        }

        for (var x = 1; x < lines[0].Length; x++)
        {
            foreach (var c in TraverseLine(0, x))
            {
                yield return c;
            }
        }

        IEnumerable<char> TraverseLine(int y, int x)
        {
            while (y < lines.Length && x < lines[y].Length)
            {
                yield return lines[y][x];

                y++;
                x++;
            }

            // yield a separator between lines
            yield return '\n';
        }
    }

    public static IEnumerable<char> TraverseDiagonalBackwards(this string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            foreach (var c in TraverseLine(y, lines[y].Length - 1))
            {
                yield return c;
            }
        }

        for (var x = lines[0].Length - 2; x >= 0; x--)
        {
            foreach (var c in TraverseLine(0, x))
            {
                yield return c;
            }
        }

        IEnumerable<char> TraverseLine(int y, int x)
        {
            while (y < lines.Length && x >= 0)
            {
                yield return lines[y][x];

                y++;
                x--;
            }

            // yield a separator between lines
            yield return '\n';
        }
    }
}
