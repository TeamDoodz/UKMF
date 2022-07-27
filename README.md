# UKMF

The Ultrakill Modding Framework (UKMF) is a [BepInEx](https://github.com/BepInEx/BepInEx) plugin that contains various APIs and utilities to aid in creating custom content for the early access title [ULTRAKILL](https://store.steampowered.com/app/1229490/ULTRAKILL/).

## Functionality

UKMF currently has the following features:

* Ability to reserve custom enum values without fear of mod conflict
* API for defining custom arm variations
* Easy methods for mods to store their own custom save data
* A simple interface for mods to read static files on disk
* Interface for adding custom content to the spawn menu
* Helper classes for handling save data and weapon configuration

## Difference from ULTRAKIT

[ULTRAKIT](https://github.com/Dazegambler/UltraKit) is another project with a very similar goal as UKMF. However, there are quite a few differences between this project and ULTRAKIT, which are:

| ULTRAKIT                                                                           | UKMF                                                                                  |
|------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------|
| Plugins written in [Lua](https://en.wikipedia.org/wiki/Lua_(programming_language)) | Plugins written in [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) |
| Can access wrappers for some engine/base game APIs                                 | Can access any part of the game directly                                              |
| Unity is required to make plugins                                                  | Unity is only required for custom assets                                              |
| Easy learning curve                                                                | Intermediate C# knowledge required                                                    |

## Compatability

There are no plans to make UKMF compatible with any other modding framework, including ad-hoc ones like [Ultrakill-Custom-Arms-Proof-Of-Concept](https://github.com/Temperz87/Ultrakill-Custom-Arms-Proof-Of-Concept).

## Feature Showcase

### Custom Enums

Using the `CustomEnums` static class you can reserve IDs for existing enumerations in the game:

```cs
BlockType Lava = (BlockType)CustomEnums.CurrentSlotData.GetValue<BlockType>("your.guid.here", "Lava");
```

When that code is executed, a new ID is generated and saved as belonging to your mod and being identified by whatever name you gave it; in this case, `Lava`.

### Custom Arms

To create a custom arm, first you will need to define how it is created:

```cs
public class MyArmFactory : AbstractPrefabFactory {
	// The return value of this method is instantiated and used as the object for this arm.
	// This example just returns the object for the Knuckleblaster; you can return any GameObject you want through this method.
	public override GameObject CreateObject() {
		return FistControl.Instance.redArm;
	}
}
```

Then, all you have to do is define your arm in the API:

```cs
CustomArms.AddArm(new CustomArmInfo() { 
	ID = (FistType)CustomEnums.CurrentSlotData.GetValue<FistType>("your.guid.here", "MyArm"),
	DisplayName = "My Arm",
	PrefabFactory = new MyArmFactory(),
	IconColor = Color.magenta,
	Holdable = true,
});
```

### Custom Save Data

Saving your own custom data is easy with the `ModdedSaveFile` class:

```cs
// CreateSaveFile is an extension method of BaseUnityPlugin, and requires a reference to your main plugin instance to use
// This particular snippet should be placed somewhere in the main plugin class, if you want to use it in other contexts you should use the Singleton pattern
ModdedSaveFile MyFile = this.CreateSaveFile("MyData");
```

You can modify the stored data through the `Data` property:

```cs
MyFile.Data.SetString("message", "Hello, World!");
MyFile.Save(); // Custom save files do not automatically write to disk when modifying them
```

Later, you can then retrieve that data:

```cs
string message = MyFile.Data.GetString("message");
Logger.LogMessage(message);
```

You can also store arbitrary binary data using `GetBytes` and `SetBytes`:

```cs
MyFile.SetBytes("someData", new byte[] { 0x0C, 0x25 });

foreach(var b in MyFile.GetBytes("someData")) {
	Logger.LogMessage(b.ToString());
}
```

### Asset Manager

The `AssetManager` class aids in reading files from disk. To create one, simply use the `AssetManager.Create` method:

```cs
// In this context, "Info" is the PluginInfo object associated with your main plugin class.
var assets = AssetManager.Create(Info);
```

Then you can load text, JSON, image, and binary files:

```cs
// The path passed to these methods is relative to (Plugin Path)/assets
string text = assets.ReadText("myText.txt");
Vector3 position = assets.ReadJSON<Vector3>("position.json");
Texture2D art = assets.ReadImage("art.jpg");
byte[] rawData = assets.ReadBytes("data.bytes");
```

You can also load custom Unity assets through asset bundles:

```cs
// For the best compatibility, asset bundles should be built using Unity v2019.4.
AssetBundle bundle = assets.ReadBundle("bundle");
foreach(string name in bundle.GetAllAssetNames()) {
	Logger.LogMessage(name);
}
```

### Custom Sandbox Tools

To create a custom tool, first you will need to define an icon:

```cs
Texture2D tex = AssetManager.Create(Info).ReadImage("icon_box.png");
Sprite sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);

CustomSandboxTools.AddNewIcon("your.guid.here:box", sprite);
```

Then, you can define the tool itself by creating a `SpawnableObject`:

```cs
CustomSandboxTools.AddNewTool(new SpawnableObject() { 
	identifier = "your.guid.here:box",
	objectName = "box",
	gameObject = myObj, // "myObj" should be an object created elsewhere, usually loaded from an asset bundle
	iconKey = "your.guid.here:box"
});
```