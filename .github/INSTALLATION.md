# Before Getting Started
This is the install guide, not the main [README.md](README.md). Both contain a description on how to install the mod, but this goes into depth (with images!) on what to download, how to install, where to place stuff, and what to create, before the mod actually works.
<hr>

# [STEP ONE] Get BepInEx. How to install, and how to setup BepInEx.
* Download [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4), unpack in your game directory, remove non-core files if you wish. <br>
<p align="center"><img height="300" alt="image" src="https://github.com/user-attachments/assets/cdf910ef-0384-49bf-bd2c-3343fad210a7" /></p>

* **Run the game once.** BepInEx needs to initialise some folders.
<hr>

# [STEP TWO] Download the mod and place it in the right spot.
* Check the [releases](https://github.com/ltsArcane/Casualties-Unknown-Seed-Mod/releases) page, and either download the latest, or most stable, version of the mod. There's a reason why sometimes the latest is NOT the most stable, so pick your poison wisely. Read the changelogs for each release for more info. I recommend the latest over the most stable 6/10 times, but if the mod doesn't work, swap to the latest stable version instead.
* Download the thing.
<p align="center"><img height="300" alt="vid" src="https://i.gyazo.com/b63c6941002ce9b9198a2da1a6985b49.gif" /></p>

* Drag-and-drop the downloaded DLL file into:

```
CasualtiesUnknown\BepInEx\plugins
```

like so:<br>

<p align="center"><img height="300" alt="image" src="https://github.com/user-attachments/assets/b2adc010-92ee-4ec5-bca2-7e8b3d5b4bb3" /></p>
<hr>

# [STEP THREE] Creating (and optionally initialising) seed.txt
* Go to:

```
C:\Users\[YOUR NAME HERE]\AppData\LocalLow\Orsoniks\CasualtiesUnknown
```

and create a file called `seed.txt`. Not `SEED.txt` or anything else. <br>

<p align="center"><img height="300" alt="image" src="https://github.com/user-attachments/assets/69b0e683-c383-437e-9d02-1112b816f617" /></p>

* Just toss whatever you want into there. Or don't, and let the mod create a custom seed for you. <br>

<p align="center"><img height="300" alt="image" src="https://github.com/user-attachments/assets/52ec4a50-c1e0-41d5-809a-1a19748f7738" /></p>

<hr>

# Optional(s)

* Enable detailed logging so you can see what the mod is doing.<br>
Open
```
CasualtiesUnknown\BepInEx\config\BepInEx.cfg
````
Then find the section `[Logging.Console]`, and set the following values as such:
```ini
Enabled = true
LogLevels = All
````
Like this:
<p align="center"><img alt="image" src="https://github.com/user-attachments/assets/c0301c68-6c64-4101-bd53-fbebe1dfd7bf" /></p>
<hr>

And that's about it. Congrats. If anything goes wrong, I've inserted multiple info/warning/error/debug lines, so do the "optional but recommended", read the terminal properly, and compare logging consoles to see if anything differs between instances.<br>
I'll probably make a video on me reading this markdown file and doing it word for word since some people will still struggle knowing my luck, but that's for the future.
