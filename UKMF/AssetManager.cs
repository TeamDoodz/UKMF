using System.IO;
using BepInEx;
using UnityEngine;

namespace UKMF {
	/// <summary>
	/// Access your mod's data files through a simplified interface.
	/// </summary>
	public class AssetManager {
		public string MainFolder;
		public AssetManager(string MainFolder) {
			this.MainFolder = MainFolder;
		}

		public string GetPath(string path) => Path.Combine(MainFolder, path);

		/// <summary>
		/// Reads from the specified file and returns its contents.
		/// </summary>
		public byte[] ReadBytes(string path) {
			string fullPath = GetPath(path);
			return File.ReadAllBytes(fullPath);
		}
		/// <summary>
		/// Reads from the specified file and returns its contents as text.
		/// </summary>
		public string ReadText(string path) {
			string fullPath = GetPath(path);
			return File.ReadAllText(fullPath);
		}
		/// <summary>
		/// Reads from the specified file, parses it using <see cref="JsonUtility"/>, and returns the deserialized output.
		/// </summary>
		public T ReadJSON<T>(string path) {
			string json = ReadText(path);
			return JsonUtility.FromJson<T>(json);
		}
		/// <summary>
		/// Reads from the specified PNG/JPG file and converts it into a <see cref="Texture2D"/>.
		/// </summary>
		public Texture2D ReadImage(string path, FilterMode filterMode = FilterMode.Bilinear) {
			byte[] data = ReadBytes(path);
			Texture2D texture = new Texture2D(2, 2);
			texture.LoadImage(data);
			texture.filterMode = filterMode;
			return texture;
		}
		/// <summary>
		/// Deserializes assets from the specified file.
		/// </summary>
		public AssetBundle ReadAssetBundle(string path) {
			string fullPath = GetPath(path);
			return AssetBundle.LoadFromFile(fullPath);
		}
		/// <summary>
		/// Opens the specified file.
		/// </summary>
		public FileStream Open(string path, FileMode mode) {
			string fullPath = GetPath(path);
			return File.Open(fullPath, mode);
		}

		public static AssetManager Create(PluginInfo plugin) {
			string mainFolder = Path.Combine(Path.GetDirectoryName(plugin.Location), "assets");
			MainPlugin.logger.LogDebug($"Creating Asset Manager for {mainFolder}");
			return new AssetManager(mainFolder);
		}
	}
}
