using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UKMF.Weapons;
using UKMF.Saving;

namespace UKMF {
	/// <summary>
	/// The main class for UKMF, responsible for initializing all of its APIs.
	/// </summary>
	[BepInPlugin(GUID,Name,Version)]
	public class MainPlugin : BaseUnityPlugin {

		internal static ManualLogSource logger;

		public const string GUID = "io.github.TeamDoodz.UKMF";
		public const string Name = "UKMF";
		/// <summary>
		/// The current version of UKMF.
		/// </summary>
		public const string Version = "0.1.0";

		private void Awake() {
			logger = Logger;
			new Harmony(GUID).PatchAll();

			CustomArms.AddBaseGameArms();

			logger.LogMessage($"{Name} v{Version} Loaded!");

			ModdedSaveFile MyFile = this.CreateSaveFile("MyData");
			MyFile.Data.SetString("message", "Hello, World!");
			MyFile.Save();
		}
	}
}
