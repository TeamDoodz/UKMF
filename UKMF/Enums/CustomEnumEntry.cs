using System;
using System.IO;
using System.Linq;

namespace UKMF.Enums {
	/// <summary>
	/// Defines a reserved enum ID, its name, and the mod that it belongs to.
	/// </summary>
	public struct CustomEnumEntry {
		public Type enumType;
		public string guid;
		public string name;
		public int id;

		/// <summary>
		/// Writes to the specified <see cref="BinaryWriter"/> with data about this object.
		/// </summary>
		public void Serialize(BinaryWriter writer) {
			writer.Write(enumType.FullName);
			writer.Write(guid);
			writer.Write(name);
			writer.Write(id);
		}

		/// <summary>
		/// Reads data from the specified <see cref="BinaryReader"/> and applies it to this object.
		/// </summary>
		public void Deserialize(BinaryReader reader) {
			string typeName = reader.ReadString();
			MainPlugin.logger.LogDebug($"type name: {typeName}");
			enumType = (from ass in AppDomain.CurrentDomain.GetAssemblies()
						from type in ass.GetTypes()
						where type.FullName == typeName
						select type).FirstOrDefault();

			guid = reader.ReadString();
			name = reader.ReadString();
			id = reader.ReadInt32();
		}

		public CustomEnumEntry(Type enumType, string guid, string name, int id) {
			this.enumType = enumType;
			this.guid = guid;
			this.name = name;
			this.id = id;
		}

		public override int GetHashCode() {
			return id;
		}

		public override bool Equals(object obj) {
			if(!(obj is CustomEnumEntry)) return false;
			return Equals((CustomEnumEntry)obj);
		}

		public bool Equals(CustomEnumEntry other) {
			return GetHashCode() == other.GetHashCode();
		}

		public static bool operator ==(CustomEnumEntry a, CustomEnumEntry b) {
			return a.Equals(b);
		}
		public static bool operator !=(CustomEnumEntry a, CustomEnumEntry b) {
			return !a.Equals(b);
		}
	}
}
