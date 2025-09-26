* Download [BepInEx](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4), unpack in your game directory, remove non-core files if you wish. <br>
<p align="center"><img width="631" height="295" alt="image" src="https://github.com/user-attachments/assets/cdf910ef-0384-49bf-bd2c-3343fad210a7" /></p>

* **Run the game once.** BepInEx needs to initialise some folders.

* Optional but Recommended: Enable detailed logging so you can see what the mod is doing:

1. Open:
```

CasualtiesUnknown\BepInEx\config\BepInEx.cfg

````
2. Find the section `[Logging.Console]`
3. Set the following values:
```ini
Enabled = true
LogLevels = All
````

<p align="center"><img width="989" height="634" alt="image" src="https://github.com/user-attachments/assets/c0301c68-6c64-4101-bd53-fbebe1dfd7bf" /></p>

* Drag-and-drop the downloaded DLL file into:

```
CasualtiesUnknown\BepInEx\plugins
```

like so: <br>

<p align="center"><img width="720" height="182" alt="image" src="https://github.com/user-attachments/assets/b2adc010-92ee-4ec5-bca2-7e8b3d5b4bb3" /></p>

* Afterwards, go to:

```
C:\Users\[YOUR NAME HERE]\AppData\LocalLow\Orsoniks\CasualtiesUnknown
```

and create a file called `seed.txt`. Not `SEED.txt` or anything else. <br>

<p align="center"><img width="665" height="287" alt="image" src="https://github.com/user-attachments/assets/69b0e683-c383-437e-9d02-1112b816f617" /></p>

* Just toss whatever you want into there. Or don't, and let the mod create a custom seed for you. <br>

<p align="center"><img width="298" height="141" alt="image" src="https://github.com/user-attachments/assets/52ec4a50-c1e0-41d5-809a-1a19748f7738" /></p>

And that's about it. Congrats. If anything goes wrong, I've inserted multiple info/warning/error/debug lines, so do the "optional but recommended", read the terminal properly, and compare logging consoles to see if anything differs between instances.<br>
I'll probably make a video on me reading this markdown file and doing it word for word since some people will still struggle knowing my luck, but that's for the future.
