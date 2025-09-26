# Forenote
I am, by no means, a professional game developer or modder. Expect this to be poorly made, most likely poorly optimised, and buggy.  
Emphasis on the buggy part. Never touched Visual Studio, let alone built a Class Library, before. Never touched C#.

This mod is made for the 4.0.1 version of Casualties: Unknown, which is currently the latest version on the game's [Itch page](https://orsonik.itch.io/scav-prototype). Make sure this is the version of the game you have before continuing.<br>
Downloadable [here](https://orsonik.itch.io/scav-prototype/download/eyJpZCI6MzIxNDQzOSwiZXhwaXJlcyI6MTc1ODgyMzQ4MX0%3d.bWj9Lv6KdmVaMJsIHZOA5Bqq0%2bg%3d) for free. Highly recommend you support the developer, or buy the game on Steam once it releases at the very least.

# Installation
For more in-depth installation instructions with images, see [INSTALL.md](INSTALL.md).
- Download [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4), unpack in your game directory, remove non-core files if you wish.
- **Run the game once.** BepInEx needs to initialise some folders.
- Drag-and-drop the downloaded DLL file into `CasualtiesUnknown\BepInEx\plugins`.
- Go to `C:\Users\[YOUR NAME HERE]\AppData\LocalLow\Orsoniks\CasualtiesUnknown`, and create a file called `seed.txt`. Give it something random, or leave it blank. Just make sure to create it.

# Warning

**THERE IS A SMALL, BUT DEFINITE, CHANCE THAT INSTANCES MAY NOT LOOK THE SAME DESPITE USING THE SAME SEED. RE-START THE RUN, AND IT SHOULD FIX ITSELF.** <br>

<p align="center"><img width="185" height="50" alt="image" src="https://github.com/user-attachments/assets/9d982dce-3bcb-4c16-9aa3-13894560d073" /></p><br>
If it still doesn't fix, try restarting the game. Keep doing so until the very tiles and plant generation of both instances look the same. Shouldn't take too many attempts, usually just 1 restart.

# Endnote

I'm not too sure why, but every so now and then, the randomness generated from a seed seems to just decide to re-generate itself, as if the seed is something completely different.
One day you're playing the same map over and over, the next day, it's completely different. I do not know why this happens, and I'm too lazy to debug every single UnityEngine.Random call to figure out why.
This is probably the most I'm going to do, don't expect much else.
