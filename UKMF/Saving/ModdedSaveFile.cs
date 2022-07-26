using System.IO;

namespace UKMF.Saving {
	public class ModdedSaveFile {
		public ModdedSaveData Data;
		internal string location;
		internal string name;

		public void Save() {
			if(!Directory.Exists(location)) Directory.CreateDirectory(location);
			using(var stream = File.Open(Path.Combine(location, name + "." + Saving.Save.MOD_DATA_EXTENSION), FileMode.Create)) {
				using(var writer = new BinaryWriter(stream)) {
					Data.Serialize(writer);
				}
			}
		}
	}
}
