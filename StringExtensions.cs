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
    static public (T i0, T i1) To2Numerics<T>(this string input, string splitter, Func<string, T> parse)
        => input.ToNumerics(splitter, parse) switch
        {
            [T i0, T i1, ..] => (i0, i1),
            _ => throw new InvalidOperationException()
        };
    static public (T i0, T i1, T i2) To3Numerics<T>(this string input, string splitter, Func<string, T> parse)
        => input.ToNumerics(splitter, parse) switch
        {
            [T i0, T i1, T i2, ..] => (i0, i1, i2),
            _ => throw new InvalidOperationException()
        };
    static public (T i0, T i1, T i3, T i4) To4Numerics<T>(this string input, string splitter, Func<string, T> parse)
        => input.ToNumerics(splitter, parse) switch
        {
            [T i0, T i1, T i2, T i3, ..] => (i0, i1, i2, i3),
            _ => throw new InvalidOperationException()
        };

    static public int[] ToInts(this string input, string splitter) => input.ToNumerics(splitter, int.Parse);
    static public long[] ToLongs(this string input, string splitter) => input.ToNumerics(splitter, long.Parse);
    static public ulong[] ToULongs(this string input, string splitter) => input.ToNumerics(splitter, ulong.Parse);

    static public (int i0, int i1) To2Ints(this string input, string splitter) => input.To2Numerics(splitter, int.Parse);
    static public (int i0, int i1, int i2) To3Ints(this string input, string splitter) => input.To3Numerics(splitter, int.Parse);
    static public (int i0, int i1, int i2, int i3) To4Ints(this string input, string splitter) => input.To4Numerics(splitter, int.Parse);

    static public IEnumerable<int[]> ToInts(this string[] lines, string splitter) => lines.Select(line => line.ToInts(splitter));

    static public int[] ToInts(this string[] input) => [.. input.Select(int.Parse)];

    static public string Sort(this string input) => string.Concat(input.OrderBy(c => c));

    static public int ToInt(this string input) => int.Parse(input);
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
}
