using BepInEx;
using System;
using System.IO;
using UnityEngine;
using HarmonyLib;
using BepInEx.Logging;

[BepInPlugin("com.example.seedmod", "Seed Mod", "1.0.0")]
public class Main : BaseUnityPlugin
{
    public static ManualLogSource LoggerInstance;

    public void Start()
    {
        LoggerInstance = Logger;
        string pathToFile = Application.persistentDataPath.Replace("/", "\\");
        string pathOfFile = Path.Combine(pathToFile, "seed.txt");

        LoggerInstance.LogMessage($"Main: For more log info, set LogLevels to All.");
        LoggerInstance.LogMessage($"Main: SeedMod is now active. Starting seed generation...");
        LoggerInstance.LogInfo($"Main: Path to seed text file: {new Uri(pathToFile).AbsoluteUri}.");
        LoggerInstance.LogDebug($"Main: Checking if file exists...");

        if (!File.Exists(pathOfFile))
        {
            LoggerInstance.LogError($"Main: File does not exist, ending mod instance...");
            LoggerInstance.LogError($"Main: Please create a seed text file at {new Uri(pathToFile).AbsoluteUri} to continue.\n");
            return;
        }

        LoggerInstance.LogDebug($"Main: Acquiring seed...");
        string seed = File.ReadAllText(pathOfFile).Trim();

        if (string.IsNullOrEmpty(seed))
        {
            seed = DateTime.Now.Ticks.ToString();
            File.WriteAllText(pathOfFile, seed);

            LoggerInstance.LogError($"Main: Seed text file exists, but is empty...");
            LoggerInstance.LogWarning($"Main: Inserting DateTime.Now as a seed into the text file instead...");
            LoggerInstance.LogWarning($"Main: The mod will run, but with a randomised seed...");
            LoggerInstance.LogWarning($"Main: Note that this will impact future gameplay similar to how a predefined seed would.\n");
        }

        LoggerInstance.LogInfo($"Main: Seed \"{seed}\" successfully acquired.");
                
        LoggerInstance.LogInfo($"Main: Seed hash is \"{seed.GetHashCode()}\" This is what will be used for making generation deterministic.\n");

        LoggerInstance.LogInfo($"Main: State-securing acquired seed...");
        SeedState.Init(seed);
        LoggerInstance.LogInfo($"Main: Secured.\n");

        LoggerInstance.LogInfo($"Main: Creating Harmony patches and patching necessary methods...");

        LoggerInstance.LogDebug($"Main: > WorldGenerationerationAwakePrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationerationAwakePrefix)); 
        
        LoggerInstance.LogDebug($"Main: > WorldGenerationerationGenerateWorldPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationerationGenerateWorldPrefix));
        
        LoggerInstance.LogDebug($"Main: > FastNoiseLiteConstructorPrefix...");
        Harmony.CreateAndPatchAll(typeof(FastNoiseLiteConstructorPrefix));

        LoggerInstance.LogDebug($"Main: > WorldGenerationDistributeEntitiesPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationDistributeEntitiesPrefix));

        LoggerInstance.LogInfo($"Main: Harmony patches successful. Mod successfully initialised.");
    }
}
    
// Patches WorldGenerationeration Awake() method. Also resets SeedState, so that each time WorldGenerationeration is ran, it uses the same sequence.
[HarmonyPatch(typeof(WorldGeneration), "Awake")]
public class WorldGenerationerationAwakePrefix
{
    static void Prefix()
    {
        Main.LoggerInstance.LogDebug($"WorldGenerationAwakePatch: Setting UnityEngine Random InitState to {SeedState.GetHashedSeed()}.");
        UnityEngine.Random.InitState(SeedState.GetHashedSeed());

        Main.LoggerInstance.LogDebug($"WorldGenerationAwakePatch: Requesting counter reset...");
        SeedState.ResetCounter();
    }
}

// Patches WorldGenerationeration.cs's GenerateWorld() method. Also resets SeedState, so that each time the world is generated, it uses the same sequence.
[HarmonyPatch(typeof(WorldGeneration), "GenerateWorld")]
public class WorldGenerationerationGenerateWorldPrefix
{
    static void Prefix()
    {
        Main.LoggerInstance.LogDebug($"WorldGenerationGenerateWorldPatch: Setting UnityEngine Random InitState to {SeedState.GetHashedSeed()}.");
        UnityEngine.Random.InitState(SeedState.GetHashedSeed());

        Main.LoggerInstance.LogDebug($"WorldGenerationGenerateWorldPatch: Requesting counter reset...");
        SeedState.ResetCounter();
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.DistributeEntities))]
public class WorldGenerationDistributeEntitiesPrefix
{
    static void Prefix()
    {
        Main.LoggerInstance.LogDebug($"WorldGenerationDistributeEntitiesPatch: Requesting counter increment...");
        SeedState.IncrementCounter();
        int subSeed = SeedState.GetIncrementedSeedHash();

        Main.LoggerInstance.LogDebug($"WorldGenerationDistributeEntitiesPatch: Setting UnityEngine Random InitState to hashed sub seed \"{subSeed}\"...");
        UnityEngine.Random.InitState(subSeed);
    }
}

[HarmonyPatch(typeof(FastNoiseLite), MethodType.Constructor, new Type[] { typeof(int) })]
public class FastNoiseLiteConstructorPrefix
{
    static void Prefix()
    {
        Main.LoggerInstance.LogDebug($"FastNoiseLiteConstructorPatch: Requesting counter increment...");
        SeedState.IncrementCounter();
        int subSeed = SeedState.GetIncrementedSeedHash();

        Main.LoggerInstance.LogDebug($"FastNoiseLiteConstructorPatch: Setting UnityEngine Random InitState to hashed sub seed \"{subSeed}\"...");
        UnityEngine.Random.InitState(subSeed);
    }
}