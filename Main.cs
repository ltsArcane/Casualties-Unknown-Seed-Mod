using BepInEx;
using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using BepInEx.Logging;

[BepInPlugin("com.casunknown.seedmod", "Seed Mod", "1.0.0")]
public class Main : BaseUnityPlugin
{
    public static ManualLogSource BaseLogger = null!;
    public static PrefixedLogger LoggerInstance = null!;

    void Awake()
    {
        BaseLogger = Logger;
        LoggerInstance = new PrefixedLogger(BaseLogger, this);
    }

    public void Start()
    {
        string pathToFile = Application.persistentDataPath.Replace("/", "\\");
        string pathOfFile = Path.Combine(pathToFile, "seed.txt");

        LoggerInstance.LogMessage($"For more log info, set LogLevels to All.\n");
        LoggerInstance.LogMessage($"SeedMod is now active.");
        LoggerInstance.LogInfo($"Path to seed text file: {new Uri(pathToFile).AbsoluteUri}.");
        LoggerInstance.LogDebug($"Checking if file exists...");

        if (File.Exists(pathOfFile)) LoggerInstance.LogInfo($"File found.");
        else
        {
            LoggerInstance.LogError($"File does not exist, ending mod instance...");
            LoggerInstance.LogError($"Please create a seed text file at {new Uri(pathToFile).AbsoluteUri} to continue.\n");
            return;
        }
        
        LoggerInstance.LogDebug($"Acquiring seed...");
        string seed = File.ReadAllText(pathOfFile).Trim();

        if (string.IsNullOrEmpty(seed))
        {
            seed = DateTime.Now.Ticks.ToString();
            File.WriteAllText(pathOfFile, seed);

            LoggerInstance.LogError($"Seed text file exists, but is empty...");
            LoggerInstance.LogWarning($"Inserting DateTime.Now.Ticks ({seed}) as a seed into the text file instead...");
            LoggerInstance.LogWarning($"The mod will run, but with a randomised seed...");
            LoggerInstance.LogWarning($"Note that this will impact future gameplay similar to how a predefined seed would.\n");
        }

        LoggerInstance.LogInfo($"Seed \"{seed}\" successfully acquired.");
        LoggerInstance.LogInfo($"Seed hash is \"{seed.GetHashCode()}\". This is what will be used for making generation deterministic.\n");
        
        LoggerInstance.LogDebug($"Initialising SeedState logger..."); // Important to do this before using SeedState, so that the loggers are ready to go.
        SeedState.InitLogger();
        LoggerInstance.LogDebug($"Initialised.\n");

        LoggerInstance.LogInfo($"State-securing acquired seed...");
        SeedState.Init(seed);
        LoggerInstance.LogInfo($"Secured.\n");

        LoggerInstance.LogInfo($"Initialising Harmony loggers..."); // Important to do this before using Harmony, so that the loggers are ready to go.
        LoggerInstance.LogDebug($"> WorldGenerationerationAwakePrefix...");
        WorldGenerationerationAwakePrefix.InitializeLogger();
        LoggerInstance.LogDebug($"> WorldGenerationerationGenerateWorldPrefix...");
        WorldGenerationerationGenerateWorldPrefix.InitializeLogger();
        LoggerInstance.LogDebug($"> FastNoiseLiteConstructorPrefix...");
        FastNoiseLiteConstructorPrefix.InitializeLogger();
        LoggerInstance.LogDebug($"> WorldGenerationDistributeEntitiesPrefix...");
        WorldGenerationDistributeEntitiesPrefix.InitializeLogger();
        LoggerInstance.LogDebug($"> WorldGenerationPlaceCrystalsPrefix...");
        WorldGenerationPlaceCrystalsPrefix.InitializeLogger();
        LoggerInstance.LogInfo($"Harmony logs successfully initialised.\n");

        LoggerInstance.LogInfo($"Creating Harmony patches and patching necessary methods...");
        LoggerInstance.LogDebug($"> WorldGenerationerationAwakePrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationerationAwakePrefix)); 
        LoggerInstance.LogDebug($"> WorldGenerationerationGenerateWorldPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationerationGenerateWorldPrefix));
        LoggerInstance.LogDebug($"> FastNoiseLiteConstructorPrefix...");
        Harmony.CreateAndPatchAll(typeof(FastNoiseLiteConstructorPrefix));
        LoggerInstance.LogDebug($"> WorldGenerationDistributeEntitiesPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationDistributeEntitiesPrefix));
        LoggerInstance.LogDebug($"> WorldGenerationPlaceCrystalsPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationPlaceCrystalsPrefix));
        LoggerInstance.LogInfo($"Harmony patches successful.\n");

        LoggerInstance.LogMessage($"SeedMod successfully initialised.\n");
    }
}
    
// Patches WorldGenerationeration Awake() method. Also resets SeedState, so that each time WorldGenerationeration is ran, it uses the same sequence.
[HarmonyPatch(typeof(WorldGeneration), "Awake")]
public class WorldGenerationerationAwakePrefix
{
    private static PrefixedLogger LoggerInstance = null!;

    public static void InitializeLogger() => LoggerInstance = new PrefixedLogger(Main.BaseLogger, typeof(WorldGenerationerationAwakePrefix));

    static void Prefix()
    {
        LoggerInstance.LogDebug($"Requesting counter reset...");
        SeedState.ResetCounter();

        LoggerInstance.LogDebug($"Setting UnityEngine Random InitState to hashed sub seed {SeedState.SeedHash}...\n");
        UnityEngine.Random.InitState(SeedState.SeedHash);
    }
}

// Patches WorldGenerationeration.cs's GenerateWorld() method. Also resets SeedState, so that each time the world is generated, it uses the same sequence.
[HarmonyPatch(typeof(WorldGeneration), "GenerateWorld")]
public class WorldGenerationerationGenerateWorldPrefix
{
    private static PrefixedLogger LoggerInstance = null!;

    public static void InitializeLogger() => LoggerInstance = new PrefixedLogger(Main.BaseLogger, typeof(WorldGenerationerationGenerateWorldPrefix));
    static void Prefix()
    {
        LoggerInstance.LogDebug($"Requesting counter reset...");
        SeedState.ResetCounter();

        LoggerInstance.LogDebug($"Setting UnityEngine Random InitState to hashed sub seed {SeedState.SeedHash}...\n");
        UnityEngine.Random.InitState(SeedState.SeedHash);

    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.DistributeEntities))]
public class WorldGenerationDistributeEntitiesPrefix
{
    private static PrefixedLogger LoggerInstance = null!;

    public static void InitializeLogger() => LoggerInstance = new PrefixedLogger(Main.BaseLogger, typeof(WorldGenerationDistributeEntitiesPrefix));
    static void Prefix(GameObject basObj)
    {
        LoggerInstance.LogDebug($"Requesting counter increment...");
        SeedState.IncrementCounter();
        int subSeed = SeedState.GetIncrementedSeedHash();

        LoggerInstance.LogDebug($"Setting UnityEngine Random InitState to hashed sub seed \"{subSeed}\"...");
        UnityEngine.Random.InitState(subSeed);

        LoggerInstance.LogDebug($"Distributing GameObject \"{basObj.name}\"...\n");
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.PlaceCrystals))]
public class WorldGenerationPlaceCrystalsPrefix
{
    private static PrefixedLogger LoggerInstance = null!;

    public static void InitializeLogger() => LoggerInstance = new PrefixedLogger(Main.BaseLogger, typeof(WorldGenerationPlaceCrystalsPrefix));
    static void Prefix()
    {
        LoggerInstance.LogDebug($"Requesting counter increment...");
        SeedState.IncrementCounter();
        int subSeed = SeedState.GetIncrementedSeedHash();

        LoggerInstance.LogDebug($"Setting UnityEngine Random InitState to hashed sub seed \"{subSeed}\".\n");
        UnityEngine.Random.InitState(subSeed);
    }
}

[HarmonyPatch(typeof(FastNoiseLite), MethodType.Constructor, new Type[] { typeof(int) })]
public class FastNoiseLiteConstructorPrefix
{
    private static PrefixedLogger LoggerInstance = null!;

    public static void InitializeLogger() => LoggerInstance = new PrefixedLogger(Main.BaseLogger, typeof(FastNoiseLiteConstructorPrefix));
    static void Prefix()
    {
        LoggerInstance.LogDebug($"Requesting counter increment...");
        SeedState.IncrementCounter();
        int subSeed = SeedState.GetIncrementedSeedHash();

        LoggerInstance.LogDebug($"Setting UnityEngine Random InitState to hashed sub seed \"{subSeed}\".\n");
        UnityEngine.Random.InitState(subSeed);
    }
}