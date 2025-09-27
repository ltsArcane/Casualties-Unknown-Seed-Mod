using System.Threading;

public static class SeedState
{
    public static int SeedHash;
    private static int counter = 0;

    public static void Init(int seedHash)
    {
        SeedHash = seedHash;
        Main.LoggerInstance.LogDebug($"SeedState: SeedState initialised with hashed seed \"{seedHash}\".");
        Interlocked.Exchange(ref counter, 0);
        Main.LoggerInstance.LogDebug($"SeedState: Counter initialised.");
    }

    public static void ResetCounter()
    {
        Main.LoggerInstance.LogDebug($"SeedState: counter of value \"{counter}\" successfully reset.");
        Interlocked.Exchange(ref counter, 0);
    }
    public static int CurrentSeed()
    {
        return unchecked(SeedHash);
    }

    public static int NextSeed()
    {
        Main.LoggerInstance.LogDebug($"SeedState: Counter incremented to \"{counter + 1}\".");
        return unchecked(SeedHash + Interlocked.Increment(ref counter));
    }
}