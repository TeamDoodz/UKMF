using System.IO;

namespace UKMF.Saving {
	public class ModdedSaveFile {
		public ModdedSaveData Data;
		internal string location;
		internal string name;

		/// <summary>
		/// Saves data from this file to disk.
		/// </summary>
		public void Save() {
			if(!Data.Dirty) return;
			Data.Dirty = false;

			if(!Directory.Exists(location)) Directory.CreateDirectory(location);
			using(var stream = File.Open(Path.Combine(location, name + "." + Saving.Save.MOD_DATA_EXTENSION), FileMode.Create)) {
				using(var writer = new BinaryWriter(stream)) {
					Data.Serialize(writer);
				}
			}
		}
	}
}
