using System.Collections.Generic;
using System.IO;
using System.Text;
using UKMF.Saving;

namespace UKMF.Enums {
	/// <summary>
	/// Allows mods to reserve enum IDs without fear of conflicting with other mods. Use <see cref="GetSlotData(int)"/> or <see cref="CurrentSlotData"/> to access extensions, and use <see cref="CustomEnumData.GetValue{T}(string, string)"/> to bind a custom ID.
	/// </summary>
	public static class CustomEnums {

		public const string DATA_FILE_NAME = "CustomEnumData.ukmf";

		private static Dictionary<int, CustomEnumData> slotData = new Dictionary<int, CustomEnumData>();

		/// <summary>
		/// Returns custom enum data for the specified save slot.
		/// </summary>
		public static CustomEnumData GetSlotData(int slot) {
			if(slotData.ContainsKey(slot)) return slotData[slot];

			string location = Save.GetDataFolder(slot);
			MainPlugin.logger.LogDebug($"Attempting to read enum data from {Path.Combine(location, DATA_FILE_NAME)}");

			if(!Directory.Exists(location) || !File.Exists(Path.Combine(location,DATA_FILE_NAME))) {
				MainPlugin.logger.LogDebug("File does not exist");
				slotData[slot] = new CustomEnumData(slot);
				return slotData[slot];
			}

			using(var stream = File.Open(Path.Combine(location, DATA_FILE_NAME), FileMode.Open)) {
				using(var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: false)) {
					slotData[slot] = new CustomEnumData(slot);
					slotData[slot].Deserialize(reader);
				}
			}
			return slotData[slot];
		}

		/// <summary>
		/// Returns custom enum data for the current save slot.
		/// </summary>
		public static CustomEnumData CurrentSlotData => GetSlotData(Save.CurrentSlot);

		internal static void SaveSlotData(int slot) {
			string location = Save.GetDataFolder(slot);
			MainPlugin.logger.LogDebug($"Attempting to write enum data to {Path.Combine(location, DATA_FILE_NAME)}");

			if(!Directory.Exists(location)) Directory.CreateDirectory(location);

			using(var stream = File.Open(Path.Combine(location, DATA_FILE_NAME), FileMode.Create)) {
				using(var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: false)) {
					if(slotData[slot] == null) slotData[slot] = new CustomEnumData(slot);
					slotData[slot].Serialize(writer);
				}
			}
		}

		internal static void SaveCurrentSlotData() => SaveSlotData(Save.CurrentSlot);

	}
}
