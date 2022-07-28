using System;
using System.Collections.Generic;
using HarmonyLib;
using ULTRAKILL.Cheats;

namespace UKMF.Sandbox {
	public static class CustomCheats {

		private static List<(ICheat,string)> customCheats = new List<(ICheat,string)>();
		private static List<(Predicate<string>, string)> renames = new List<(Predicate<string>, string)>();

		public static ICheat GetCheat(string guid, string name) => CheatsManager.Instance.idToCheat[IdentifierUtil.CreateIdentifier(guid, name)];

		/// <summary>
		/// Adds a new cheat into the game. <b>Not recommended</b>, use <see cref="AddCheat(CustomCheatInfo, string)"/> instead.
		/// </summary>
		public static void AddCheat(ICheat cheat, string category = "general") {
			customCheats.Add((cheat, category));
			MainPlugin.logger.LogInfo($"New cheat added: {cheat.Identifier}");
			OnCheatAdded?.Invoke((cheat, category));
		}
		/// <summary>
		/// Adds a new cheat into the game.
		/// </summary>
		public static void AddCheat(CustomCheatInfo cheat, string category = "general") {
			AddCheat(new CustomCheatInfoToICheatAdapter(cheat), category);
		}

		public static void RenameCategory(Predicate<string> from, string to) {
			renames.Add((from, to));
		}
		public static void RenameCategory(string from, string to) {
			RenameCategory((x) => x == from, to);
		}

		public static string ApplyCategoryRenames(string category) {
			string outp = category;
			foreach(var c in renames) {
				if(c.Item1(outp)) outp = c.Item2;
			}
			return outp;
		}

		public static event Action<(ICheat, string)> OnCheatAdded;

		[HarmonyPatch(typeof(CheatsManager))]
		[HarmonyPatch(nameof(CheatsManager.Start))]
		private static class AddCustomCheatsPatch {
			static bool Prefix(CheatsManager __instance) {
				__instance.RebuildIcons();

				string categoryMeta = ApplyCategoryRenames("meta");
				string categorySandbox = ApplyCategoryRenames("sandbox");
				string categoryGeneral = ApplyCategoryRenames("general");
				string categoryMovement = ApplyCategoryRenames("movement");
				string categoryWeapons = ApplyCategoryRenames("weapons");
				string categoryEnemies = ApplyCategoryRenames("enemies");
				string categorySpecial = ApplyCategoryRenames("special");

				// META
				__instance.RegisterCheat(new KeepEnabled(), categoryMeta);

				// SANDBOX
				if(MapInfoBase.InstanceAnyType.sandboxTools) {
					// quicksaving
					__instance.RegisterCheat(new QuickSave(), categorySandbox);
					__instance.RegisterCheat(new QuickLoad(), categorySandbox);
					__instance.RegisterCheat(new ManageSaves(), categorySandbox);
					__instance.RegisterCheat(new ClearMap(), categorySandbox);
					// navmesh
					if(MonoSingleton<SandboxNavmesh>.Instance) {
						__instance.RegisterCheat(new RebuildNavmesh(), categorySandbox);
					}
				}
				// grid building
				__instance.RegisterCheats(new ICheat[] {
					new Snapping(),
					new Physics()
				}, categorySandbox);

				// GENERAL
				__instance.RegisterCheats(new ICheat[] {
					new SpawnSpawnerArm(),
					new TeleportMenu(),
					new FullBright()
				}, categoryGeneral);

				// MOVEMENT
				__instance.RegisterCheats(new ICheat[] {
					new Noclip(),
					new Flight(),
					new InfiniteWallJumps()
				}, categoryMovement);

				// WEAPONS
				__instance.RegisterCheats(new ICheat[] {
					new NoWeaponCooldown(),
					new InfinitePowerUps()
				}, categoryWeapons);

				// ENEMIES
				__instance.RegisterCheats(new ICheat[] {
					new BlindEnemies(),
					new DisableEnemySpawns(),
					new InvincibleEnemies(),
					new KillAllEnemies()
				}, categoryEnemies);

				// SPECIAL
				if(GameProgressSaver.GetClashModeUnlocked()) {
					__instance.RegisterCheat(new CrashMode(), categorySpecial);
				}

				// MODDED
				foreach(var cheat in customCheats) {
					__instance.RegisterCheat(cheat.Item1, ApplyCategoryRenames(cheat.Item2));
				}

				MonoSingleton<CheatBinds>.Instance.RestoreBinds(__instance.allRegisteredCheats);
				__instance.RebuildMenu();
				return false;
			}
		}

	}
}
