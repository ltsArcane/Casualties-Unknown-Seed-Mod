I am, by no means, a professional game developer or modder. Expect this to be poorly made, most likely poorly optimised, and buggy.
Emphasis on the buggy part. Never touched Visual Studio, let alone built a Class Library, before. Never touched C#.

Download [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4), unpack in your game directory, remove non-core files if you wish.
<img width="631" height="295" alt="image" src="https://github.com/user-attachments/assets/cdf910ef-0384-49bf-bd2c-3343fad210a7" />

Run the game once so that BepInEx can create it's necessary components (ie config/core/patches/plugins).

Optional but recommended: Go to "CasualtiesUnknown\BepInEx\config\BepInEx.cfg", and make sure [Logging.Console].Enabled = true and [Logging.Console].LogLevels = All.

Drap-and-drop the downloaded DLL file into "CasualtiesUnknown\BepInEx\plugins" like so:
<img width="720" height="182" alt="image" src="https://github.com/user-attachments/assets/b2adc010-92ee-4ec5-bca2-7e8b3d5b4bb3" />

Afterwards, go to "C:\Users\[YOUR NAME HERE]\AppData\LocalLow\Orsoniks\CasualtiesUnknown", and create a text file called "seed". Caps-sensisitve.
<img width="665" height="287" alt="image" src="https://github.com/user-attachments/assets/69b0e683-c383-437e-9d02-1112b816f617" />

Just toss whatever you want into there. Or don't, and let the mod create a custom seed for you.
<img width="298" height="141" alt="image" src="https://github.com/user-attachments/assets/52ec4a50-c1e0-41d5-809a-1a19748f7738" />

And that's about it. Congrats. If anything goes wrong, I've inserted like info/warning/error/debug lines, so read the terminal properly, and compare code.

# THERE IS A SMALL, BUT DEFINITE, CHANCE THAT INSTANCES MAY NOT LOOK THE SAME DESPITE USING THE SAME SEED. RE-START THE RUN, AND IT SHOULD FIX ITSELF.
<img width="185" height="50" alt="image" src="https://github.com/user-attachments/assets/9d982dce-3bcb-4c16-9aa3-13894560d073" />

I'm not too sure why, but every so now and then, the randomness generated from a seed seems to just decide to re-generate itself, as if the seed is something completely different.
One day you're playing the same map over and over, the next day, it's completely different. I do not know why this happens, and I'm too lazy to debug every single UnityEngine.Random call to figure out why.
This is probably the most I'm going to do, don't expect much else.
