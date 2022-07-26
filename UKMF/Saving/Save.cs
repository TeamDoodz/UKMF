using System.IO;
using BepInEx;

namespace UKMF.Saving {
	/// <summary>
	/// Helper class for handling save data. Use <see cref="CreateSaveFile(BaseUnityPlugin, string)"/> to create a reference to a save file.
	/// </summary>
	public static class Save {
		/// <summary>
		/// The file extension used for modded data files.
		/// </summary>
		public const string MOD_DATA_EXTENSION = "ukmf";

		/// <summary>
		/// The name of the folder that modded save data will be stored in.
		/// </summary>
		public const string MOD_DATA_FOLDER_NAME = "ModData";

		/// <summary>
		/// Returns the path to the specified save slot's mod data folder.
		/// </summary>
		public static string GetDataFolder(int slot) => Path.Combine(GameProgressSaver.BaseSavePath, string.Format("Slot{0}", slot + 1), MOD_DATA_FOLDER_NAME);
		/// <summary>
		/// Returns the path to this save slot's mod data folder.
		/// </summary>
		public static string GetCurrentDataFolder() => GetDataFolder(CurrentSlot);

		/// <summary>
		/// The save slot index that the player is currently on.
		/// </summary>
		public static int CurrentSlot => MonoSingleton<PrefsManager>.Instance.GetInt("selectedSaveSlot", 0);

		/// <summary>
		/// Creates a reference to a save file of the specified name.
		/// </summary>
		/// <param name="path">The path of the save file, relative to <c>(Save Folder)/ModData/(Mod GUID)</c></param>
		public static ModdedSaveFile CreateSaveFile(this BaseUnityPlugin plugin, string path) {
			var outp = new ModdedSaveFile {
				location = Path.Combine(GetCurrentDataFolder(), plugin.Info.Metadata.GUID),
				name = path
			};

			outp.Data = new ModdedSaveData();

			if(File.Exists(Path.Combine(outp.location, path + "." + MOD_DATA_EXTENSION))) {
				using(var stream = File.OpenRead(Path.Combine(outp.location, path + "." + MOD_DATA_EXTENSION))) {
					using(var reader = new BinaryReader(stream)) {
						outp.Data.Deserialize(reader);
					}
				}
			}

			return outp;
		}
	}
}
