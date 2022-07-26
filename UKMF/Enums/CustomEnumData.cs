using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UKMF.Saving;

namespace UKMF.Enums {
	/// <summary>
	/// Stores data for custom enums.
	/// </summary>
	public class CustomEnumData {
		private Dictionary<Type, List<CustomEnumEntry>> customEnums;
		internal int slot;

		public CustomEnumData() {
			slot = Save.CurrentSlot;
		}

		public CustomEnumData(int slot) {
			this.slot = slot;
		}

		/// <summary>
		/// Binds a custom enum value. Note that this method is not super efficient and you should cache its result for better performance.
		/// </summary>
		public int GetValue<T>(string guid, string name) where T : Enum {
			return GetValue(typeof(T), guid, name);
		}

		/// <summary>
		/// Binds a custom enum value. Note that this method is not super efficient and you should cache its result for better performance.
		/// </summary>
		public int GetValue(Type enumType, string guid, string name) {
			if(customEnums == null) customEnums = new Dictionary<Type, List<CustomEnumEntry>>();
			if(!customEnums.ContainsKey(enumType)) customEnums.Add(enumType, new List<CustomEnumEntry>());

			//TODO: Use hashing instead of comparing strings
			if(customEnums[enumType].Any((x) => x.guid == guid && x.name == name)) {
				return customEnums[enumType].Find((x) => x.guid == guid && x.name == name).id;
			}

			MainPlugin.logger.LogDebug($"Creating new Enum {guid}, {name}, {enumType}");

			int ID;
			int min = Enum.GetValues(enumType).Length;
			Random rand = new Random();
			//TODO: Enums allow for negative values, we should allow them in ID generation
			for(ID = rand.Next(min, int.MaxValue); customEnums[enumType].Any((x) => x.id == ID); ID = rand.Next(min, int.MaxValue)) ;

			MainPlugin.logger.LogDebug($"ID is {ID}");

			customEnums[enumType].Add(new CustomEnumEntry(enumType, guid, name, ID));

			MainPlugin.logger.LogInfo($"{guid} added new Enum: {name} (extends {enumType.FullName}, id: {ID})");

			CustomEnums.SaveSlotData(slot);

			return ID;
		}

		/// <summary>
		/// Clears all custom enum data. Only use this if you know what you are doing!
		/// </summary>
		public void Clear() {
			customEnums.Clear();
			CustomEnums.SaveSlotData(slot);
		}

		// Keep in mind that serialization lists entries in sequential order without any mapping; this is to make things easier to work with and it doesn't really impact performance that much (i think) since the data is going to be converted to a dictionary anyway

		/// <summary>
		/// Writes to the specified <see cref="BinaryWriter"/> with data about this object.
		/// </summary>
		public void Serialize(BinaryWriter writer) {
			int length = 0;
			foreach(var entries in customEnums.Values) {
				foreach(var entry in entries) {
					length++;
				}
			}
			writer.Write(length);
			foreach(var entries in customEnums.Values) {
				foreach(var entry in entries) {
					entry.Serialize(writer);
				}
			}
		}

		/// <summary>
		/// Reads data from the specified <see cref="BinaryReader"/> and applies it to this object. Note that data is not automatically cleared when deserializing; you may want to call <see cref="Clear()"/> before this.
		/// </summary>
		public void Deserialize(BinaryReader reader) {
			int entriesCount = reader.ReadInt32();
			for(int i = 0; i < entriesCount; i++) {
				CustomEnumEntry entry = new CustomEnumEntry();
				entry.Deserialize(reader);
				if(customEnums == null) customEnums = new Dictionary<Type, List<CustomEnumEntry>>();
				if(!customEnums.ContainsKey(entry.enumType)) customEnums.Add(entry.enumType, new List<CustomEnumEntry>());
				customEnums[entry.enumType].Add(entry);
			}
		}
	}
}
