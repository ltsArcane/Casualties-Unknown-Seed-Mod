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
        
        int seedHash = seed.GetHashCode();
        
        LoggerInstance.LogInfo($"Main: Seed hash is \"{seedHash}\" This is what will be used for making generation deterministic.\n");

        LoggerInstance.LogInfo($"Main: Initialising and setting seed hash as patch variables...");
        LoggerInstance.LogDebug($"Main: > WorldGenerationerationAwakePrefix...");
        WorldGenerationerationAwakePrefix.SeedHash = seedHash;
        LoggerInstance.LogDebug($"Main: > WorldGenerationerationGenerateWorldPrefix...");
        WorldGenerationerationGenerateWorldPrefix.SeedHash = seedHash;
        LoggerInstance.LogDebug($"Main: > SeedState...");
        SeedState.Init(seedHash);
        LoggerInstance.LogInfo($"Main: Initialisation successful.\n");

        LoggerInstance.LogInfo($"Main: Creating Harmony patches and patching necessary methods...");
        LoggerInstance.LogDebug($"Main: > WorldGenerationerationAwakePrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationerationAwakePrefix)); 
        
        LoggerInstance.LogDebug($"Main: > WorldGenerationerationGenerateWorldPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationerationGenerateWorldPrefix));
        
        LoggerInstance.LogDebug($"Main: > FastNoiseLiteConstructorPrefix...");
        Harmony.CreateAndPatchAll(typeof(FastNoiseLiteConstructorPrefix));

        LoggerInstance.LogDebug($"Main: > FastNoiseLiteConstructorPrefix...");
        Harmony.CreateAndPatchAll(typeof(WorldGenerationDistributeEntitiesPrefix));

        LoggerInstance.LogInfo($"Main: Harmony patches successful. Mod successfully initialised.");
    }
}
    
// Patches WorldGenerationeration Awake() method. Also resets SeedState, so that each time WorldGenerationeration is ran, it uses the same sequence.
[HarmonyPatch(typeof(WorldGeneration), "Awake")]
public class WorldGenerationerationAwakePrefix
{
    public static int SeedHash; // Always ensure Seed is set before patching.

    static void Prefix()
    {
        Main.LoggerInstance.LogDebug($"WorldGenerationAwakePatch: Setting UnityEngine Random InitState to {SeedHash!}.");
        UnityEngine.Random.InitState(SeedHash);

        Main.LoggerInstance.LogDebug($"WorldGenerationAwakePatch: Requesting counter reset...");
        SeedState.ResetCounter();
    }
}

// Patches WorldGenerationeration.cs's GenerateWorld() method. Also resets SeedState, so that each time the world is generated, it uses the same sequence.
[HarmonyPatch(typeof(WorldGeneration), "GenerateWorld")]
public class WorldGenerationerationGenerateWorldPrefix
{
    public static int SeedHash; // Always ensure Seed is set before patching.

    static void Prefix()
    {
        Main.LoggerInstance.LogDebug($"WorldGenerationGenerateWorldPatch: Setting UnityEngine Random InitState to {SeedHash!}.");
        UnityEngine.Random.InitState(SeedHash);

        Main.LoggerInstance.LogDebug($"WorldGenerationGenerateWorldPatch: Requesting counter reset...");
        SeedState.ResetCounter();
    }
}

[HarmonyPatch(typeof(WorldGeneration), nameof(WorldGeneration.DistributeEntities))]
public class WorldGenerationDistributeEntitiesPrefix
{
    static void Prefix()
    {
        int subSeed = SeedState.NextSeed();
        Main.LoggerInstance.LogDebug($"WorldGenerationDistributeEntitiesPatch: Setting UnityEngine Random InitState to hashed sub seed \"{subSeed}\"...");
        UnityEngine.Random.InitState(subSeed);
    }
}

// Replace whatever seed the game would have passed (Unity RNG) with BaseSeed + counter.
[HarmonyPatch(typeof(FastNoiseLite), MethodType.Constructor, new Type[] { typeof(int) })]
public class FastNoiseLiteConstructorPrefix
{
    static void Prefix(ref int seed)
    {
        Main.LoggerInstance.LogDebug($"FastNoiseLite: Requesting counter increment...");
        int subSeed = SeedState.NextSeed();
        Main.LoggerInstance.LogDebug($"FastNoiseLite: Replacing random value \"{seed}\" with hashed sub seed \"{subSeed}\"...");
        seed = subSeed;
    }
}