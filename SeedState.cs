using System.Threading;

public static class SeedState
{
    public static string Seed = "";
    private static int counter = 0;

    public static void Init(string seed)
    {
        Seed = seed;
        Main.LoggerInstance.LogDebug($"SeedState: Initialised with seed \"{Seed}\".");
        Main.LoggerInstance.LogDebug($"SeedState: Hashed seed value set to \"{GetHashedSeed()}\".");
        Main.LoggerInstance.LogDebug($"SeedState: Initialising counter...");
        ResetCounter();
    }

    public static void ResetCounter()
    {
        Main.LoggerInstance.LogDebug($"SeedState: counter of value \"{counter}\" successfully reset.");
        Interlocked.Exchange(ref counter, 0);
    }

    public static void IncrementCounter()
    {
        Main.LoggerInstance.LogDebug($"SeedState: Counter incremented to \"{counter + 1}\".");
        Interlocked.Increment(ref counter);
    }

    public static string GetSeed() => Seed;
    public static int GetHashedSeed() => unchecked(Seed.GetHashCode());
    public static int GetIncrementedSeedHash() => unchecked(GetHashedSeed() + counter);
    
}