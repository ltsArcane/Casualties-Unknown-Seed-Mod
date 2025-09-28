using System;
using System.Threading;

public static class SeedState
{
    private static PrefixedLogger LoggerInstance = null!;


    private static string Seed = null!; // Don't really need the seed anywhere else other than within SeedState.
    public static int SeedHash => unchecked(Seed.GetHashCode());

    private static int counter = 0;
    public static void InitLogger() => LoggerInstance = new PrefixedLogger(Main.BaseLogger, typeof(SeedState));

    public static void Init(string seed)
    {
        Seed = seed;
        LoggerInstance.LogDebug($"Initialised with seed \"{Seed}\".");
        LoggerInstance.LogDebug($"Hashed seed value set to \"{SeedHash}\".");
        LoggerInstance.LogDebug($"Initialising counter...");
        ResetCounter();
    }

    public static void ResetCounter()
    {
        if (counter == 0) return;
        LoggerInstance.LogDebug($"counter of value \"{counter}\" successfully reset.");
        Interlocked.Exchange(ref counter, 0);
    }

    public static void IncrementCounter()
    {
        LoggerInstance.LogDebug($"Counter incremented to \"{counter + 1}\".");
        Interlocked.Increment(ref counter);
    }
    public static int GetIncrementedSeedHash() => unchecked(SeedHash + counter);
}