﻿static partial class Aoc2024
{
    public static void DayTemplate()
    {
        var day = MethodBase.GetCurrentMethod()!.Name;

        ComputeExample(); Compute();

        void ComputeExample()
        {
            var input = 
                """
                abc
                """.ToLines();
            Part1(input).Should().Be(0);
            Part2(input).Should().Be(0);
        }

        void Compute()
        {
            var input = File.ReadAllLines($"{day.ToLowerInvariant()}.txt");
            Part1(input).Should().Be(0);
            Part2(input).Should().Be(0);
        }

        int Part1(string[] lines) => 0;
        int Part2(string[] lines) => 0;
    }
}