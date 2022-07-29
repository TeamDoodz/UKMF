using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UKMF.Weapons;
using UKMF.Sandbox.Extra;
using UKMF.Saving;
using BepInEx.Configuration;

namespace UKMF {
	/// <summary>
	/// The main class for UKMF, responsible for initializing all of its APIs.
	/// </summary>
	[BepInPlugin(GUID,Name,Version)]
	public class MainPlugin : BaseUnityPlugin {

		internal static ManualLogSource logger;
		internal static ConfigFile cfg;
		internal static AssetManager assets;

		public const string GUID = "io.github.TeamDoodz.UKMF";
		public const string Name = "UKMF";
		/// <summary>
		/// The current version of UKMF.
		/// </summary>
		public const string Version = "0.1.0";

		private void Awake() {
			logger = Logger;
			cfg = Config;
			assets = AssetManager.Create(Info);
			new Harmony(GUID).PatchAll();

			CustomArms.AddBaseGameArms();

			ExtraCheats.DoStuff();

			logger.LogMessage($"{Name} v{Version} Loaded!");
		}
	}
}
