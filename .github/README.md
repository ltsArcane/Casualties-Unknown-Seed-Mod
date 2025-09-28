# Forenote
I am, by no means, a professional game developer or modder. Expect this to be poorly made, most likely poorly optimised, and buggy.  
Emphasis on the buggy part. Never touched Visual Studio, let alone built a Class Library, before. Never touched C#.

This mod is made for the 4.0.1 version of Casualties: Unknown, which is currently the latest version on the game's [Itch page](https://orsonik.itch.io/scav-prototype). Make sure this is the version of the game you have before continuing.<br>
Downloadable [here](https://orsonik.itch.io/scav-prototype/download/eyJpZCI6MzIxNDQzOSwiZXhwaXJlcyI6MTc1ODgyMzQ4MX0%3d.bWj9Lv6KdmVaMJsIHZOA5Bqq0%2bg%3d) for free. Highly recommend you support the developer, or buy the game on Steam once it releases at the very least.

For debugging purposes, I highly recommend you set 2 of BepInEx's configs to be as such (both fall under the `[Logging.Console]`) section
```ini
Enabled = true
LogLevels = All
```
This is re-iterated with a picture in the installation guide file [here](INSTALL.md), so just double-check there if you aren't 100% sure.

# What This Mod Does
This mod forces *Casualties: Unknown* to use a consistent seed for world generation, allowing maps and entity placement to be repeatable across runs.

* It reads a `seed.txt` file from the game’s save/config folder.
* If the file is empty, the mod generates a seed automatically and writes it to the file. No file = no mod.
* All world generation systems (map layout, noise functions, entity distribution, etcetera) are patched to use this seed, ensuring deterministic results.
* Sub-seeds are created during generation steps so the game behaves consistently, while still having variation between different processes (otherwise you'd see recurrent patterns).

**In short:** with this mod, you can control or lock world randomness. Using the same seed will always generate the same world, letting you replay identical scenarios or share seeds with others.

# How It Works (Technical Overview)

*This section is the optional "stats for nerds." Here for those curious about implementation details. Assumes you've already read "What This Mod Does".*

The mod uses [Harmony](https://github.com/pardeike/Harmony) patches to hook into *Casualties: Unknown*’s world generation methods and force them to use a deterministic seed.

### Core Logic
* A `SeedHash` is computed based on `seed.txt` file's contents, or `DateTime.Now.Ticks` if said file is empty. Latter is written back to the file.
* A `SeedState` class to manage the active seed and an incrementing counter for generating sub-seeds.

### Patches
* **`WorldGeneration.Awake()`**: Initializes the Unity random state with the main seed and resets the counter.
* **`WorldGeneration.GenerateWorld()`**: Ensures each new world uses the same deterministic seed sequence.
* **`WorldGeneration.DistributeEntities()`**: Increments the counter to generate sub-seeds for entity placement.
* **`FastNoiseLite(int constructor)`**: Increments the counter to generate sub-seeds for noise generation.

### Why This Matters
By re-seeding Unity’s `Random` consistently across these generation steps, the game world becomes **deterministic**.
* Running the game with the same `seed.txt` will always produce the same map and entity layout.
* Sub-seeding keeps different generation processes independent but still tied to the master seed.

This setup allows repeatable scenarios, easier debugging, and the ability to share seeds with others while preserving consistency.

# Installation
For more in-depth installation instructions with images, see [INSTALL.md](INSTALL.md).
- Download [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4), unpack in your game directory, remove non-core files if you wish.
- **Run the game once.** BepInEx needs to initialise some folders.
- Drag-and-drop the downloaded DLL file into `CasualtiesUnknown\BepInEx\plugins`.
- Go to `C:\Users\[YOUR NAME HERE]\AppData\LocalLow\Orsoniks\CasualtiesUnknown`, and create a file called `seed.txt`. Give it something random, or leave it blank. Just make sure to create it.

# Bug #1 (IMPORTANT)

<p align="center"><b>THERE IS A VERY HIGH CHANCE THAT NEWLY OPENED INSTANCES INITIALISE WEIRDLY WITH JANKY INIT STATES, MESSING WITH THE MOD IN SUCH A WAY THAT CAN'T BE RELIABLY REPLICATED OR SHOWN VIA LOGS.</b></p>

If this happens, try hitting <img alt="Start run button" src="https://github.com/user-attachments/assets/22b85ab9-baf9-41cc-b9f1-2377ddcfdc4f" height="11" style="vertical-align:centre;"/> again.

If not, restart the game. One restart should fix it, as the instances opened thereafter should mend properly with the mod.

# Bug #2 <i>(not as important)</i>

I'm not too sure why, but every so now and then, the randomness generated from a seed seems to just decide to set entirely new randomness, as if the seed is something completely different.

One day you're playing the same map over and over, the next day, it's completely different. Sometimes, it'll seemingly reset itself back to an old state that you may have played on before. 

I don't know why this happens, and it honestly has so little impact on the main use case of this mod, that I will not be worrying about it for the timebeing, if at all.

# Footnote

This is probably the most I'm going to do, don't expect much else.
