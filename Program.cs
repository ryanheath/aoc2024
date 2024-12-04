try
{
    Console.WriteLine("Start");
    var totalMs = 0d;
    var totalMem = 0L;
    
    typeof(Aoc2024).GetMethods(BindingFlags.Static | BindingFlags.Public)
        .Where(m => m.Name.StartsWith("Day"))
        .Where(m => m.Name != "DayTemplate")
        .OrderByDescending(m => m.Name.Length)
        .ThenByDescending(m => m.Name)
        .ToList()
        .ForEach(m => 
        {
            Console.Write(m.Name.PadLeft(5));
            var memBefore = GC.GetTotalAllocatedBytes();
            var start = Stopwatch.GetTimestamp();

            m.Invoke(null, null);

            var elapsedTicks = Stopwatch.GetTimestamp() - start;
            var elapsedMs = elapsedTicks * 1000.0 / Stopwatch.Frequency;
            var memAfter = GC.GetTotalAllocatedBytes();

            var mem = memAfter - memBefore;
            Console.WriteLine($" {elapsedMs:#,0.0} ms {mem/1024.0/1024:#,0.0} MB");
            totalMs += elapsedMs;
            totalMem += mem;
        });
    
    Console.WriteLine($"Total: {totalMs:#,0.0} ms {totalMem/1024.0/1024:#,0.0} MB");
    Console.WriteLine("Done!");
}
catch (Exception e)
{
    while (e.InnerException is not null)
    {
        e = e.InnerException;
    }

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.Message);
    Console.ResetColor();
    if (e is not FluentAssertions.Execution.AssertionFailedException)
    {
        Console.WriteLine(e.StackTrace);
    }
}