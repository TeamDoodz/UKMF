using System;
using System.Collections.Generic;
using HarmonyLib;
using ULTRAKILL.Cheats;

namespace UKMF.Sandbox {
	public static class CustomCheats {

		private static List<(ICheat,string)> customCheats = new List<(ICheat,string)>();

		/// <summary>
		/// Adds a new cheat into the game. <b>Not recommended</b>, use <see cref="AddCheat(CustomCheatInfo, string)"/> instead.
		/// </summary>
		public static void AddCheat(ICheat cheat, string category = "modded") {
			customCheats.Add((cheat, category));
			MainPlugin.logger.LogInfo($"New cheat added: {cheat.Identifier}");
			OnCheatAdded?.Invoke((cheat, category));
		}
		/// <summary>
		/// Adds a new cheat into the game.
		/// </summary>
		public static void AddCheat(CustomCheatInfo cheat, string category = "modded") {
			AddCheat(new CustomCheatInfoToICheatAdapter(cheat), category);
		}

		public static event Action<(ICheat, string)> OnCheatAdded;

		[HarmonyPatch(typeof(CheatsManager))]
		[HarmonyPatch(nameof(CheatsManager.Start))]
		private static class AddCustomCheatsPatch {
			static bool Prefix(CheatsManager __instance) {
				__instance.RebuildIcons();

				// META
				__instance.RegisterCheat(new KeepEnabled(), "meta");

				// SANDBOX
				if(MapInfoBase.InstanceAnyType.sandboxTools) {
					// quicksaving
					__instance.RegisterCheat(new QuickSave(), "sandbox");
					__instance.RegisterCheat(new QuickLoad(), "sandbox");
					__instance.RegisterCheat(new ManageSaves(), "sandbox");
					__instance.RegisterCheat(new ClearMap(), "sandbox");
					// navmesh
					if(MonoSingleton<SandboxNavmesh>.Instance) {
						__instance.RegisterCheat(new RebuildNavmesh(), "sandbox");
					}
				}
				// grid building
				__instance.RegisterCheats(new ICheat[] {
					new Snapping(),
					new Physics()
				}, "sandbox");

				// GENERAL
				__instance.RegisterCheats(new ICheat[] {
					new SpawnSpawnerArm(),
					new TeleportMenu(),
					new FullBright()
				}, "general");

				// MOVEMENT
				__instance.RegisterCheats(new ICheat[] {
					new Noclip(),
					new Flight(),
					new InfiniteWallJumps()
				}, "movement");

				// WEAPONS
				__instance.RegisterCheats(new ICheat[] {
					new NoWeaponCooldown(),
					new InfinitePowerUps()
				}, "weapons");

				// ENEMIES
				__instance.RegisterCheats(new ICheat[] {
					new BlindEnemies(),
					new DisableEnemySpawns(),
					new InvincibleEnemies(),
					new KillAllEnemies()
				}, "enemies");

				// SPECIAL
				if(GameProgressSaver.GetClashModeUnlocked()) {
					__instance.RegisterCheat(new CrashMode(), "special");
				}

				// MODDED
				foreach(var cheat in customCheats) {
					__instance.RegisterCheat(cheat.Item1, cheat.Item2);
				}

				MonoSingleton<CheatBinds>.Instance.RestoreBinds(__instance.allRegisteredCheats);
				__instance.RebuildMenu();
				return false;
			}
		}

	}
}
