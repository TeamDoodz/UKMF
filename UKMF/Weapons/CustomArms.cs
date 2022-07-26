using System;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace UKMF.Weapons {
	/// <summary>
	/// Allows mods to define their own custom arms to be used in-game. Use <see cref="AddArm(CustomArmInfo)"/> to define an arm.
	/// </summary>
	public static class CustomArms {
		private static List<CustomArmInfo> AllData = new List<CustomArmInfo>();

		public static CustomArmInfo GetInfo(FistType ID) => AllData.Find(x => x.ID == ID);
		public static CustomArmInfo GetCurrentInfo() => GetInfo(Weapon.CurrentArm);
		public static void AddArm(CustomArmInfo arm) {
			if(AllData.Any((x) => x.ID == arm.ID)) {
				throw new InvalidOperationException("An arm with that ID already exists.");
			}
			AllData.Add(arm);
			MainPlugin.logger.LogInfo($"New arm registered: {arm.DisplayName} {(arm.IsBaseGame? "(Base game)" : "")} {(arm.Holdable? "" : "(Not holdable)")}");
			OnArmAdded?.Invoke(arm);
		}

		internal static void AddBaseGameArms() {
			// Feedbacker
			AddArm(new CustomArmInfo() { 
				ID = FistType.Standard,
				DisplayName = "Feedbacker",
				PrefabFactory = new BaseGameArmFactory(FistType.Standard),
				IconColor = Weapon.GetVariationColor(0),
				Holdable = true,
				IsBaseGame = true,
			});
			// Knuckleblaster
			AddArm(new CustomArmInfo() {
				ID = FistType.Heavy,
				DisplayName = "Knuckleblaster",
				PrefabFactory = new BaseGameArmFactory(FistType.Heavy),
				IconColor = Weapon.GetVariationColor(2),
				Holdable = true,
				IsBaseGame = true,
			});
			// Whiplash
			AddArm(new CustomArmInfo() {
				ID = FistType.Spear,
				DisplayName = "Whiplash",
				PrefabFactory = new BaseGameArmFactory(FistType.Spear),
				IconColor = Weapon.GetVariationColor(1),
				Holdable = false,
				IsBaseGame = true,
			});
		}

		internal static void UpdateBaseGameIconColors() {

		}

		public static event Action<CustomArmInfo> OnArmAdded;

		[HarmonyPatch(typeof(FistControl))]
		[HarmonyPatch(nameof(FistControl.UpdateFistIcon))]
		private static class ReplaceUpdateFistIconPatch {
			static bool Prefix(FistControl __instance) {
				try {
					__instance.fistIcon.color = GetCurrentInfo().IconColor;
				} catch(Exception e) {
					MainPlugin.logger.LogWarning($"There was an error updating the icon color:\n {e}");
				}
				return false;
			}
		}

		[HarmonyPatch(typeof(FistControl))]
		[HarmonyPatch(nameof(FistControl.ResetFists))]
		private static class ReplaceResetFistsPatch {
			static void TryEquipArm(CustomArmInfo info, FistControl parent) {
				if(!Weapon.IsArmEquipped(info.ID)) return;

				MainPlugin.logger.LogDebug($"Initializing arm {info.DisplayName}");

				var go = info.PrefabFactory.CreateObject();
				if(go != null) {
					var inst = GameObject.Instantiate(go, parent.transform);
					inst.name = info.DisplayName;
					parent.spawnedArms.Add(inst);
					parent.spawnedArmNums.Add((int)info.ID);
				}

				// If arm is whiplash, equip it
				if(info.ID == FistType.Spear) HookArm.Instance.equipped = true;
			}
			static bool Prefix(FistControl __instance) {
				// Destroy all arms and unequip whiplash
				if(__instance.spawnedArms.Count > 0) {
					for(int i = 0; i < __instance.spawnedArms.Count; i++) {
						UnityEngine.Object.Destroy(__instance.spawnedArms[i]);
					}
					__instance.spawnedArms.Clear();
					__instance.spawnedArmNums.Clear();
				}
				HookArm.Instance.equipped = false;

				// Spawn equipped arms
				foreach(var arm in AllData) {
					TryEquipArm(arm, __instance);
				}

				// ???
				if(__instance.spawnedArms.Count <= 1 || !MonoSingleton<PrefsManager>.Instance.GetBool("armIcons", false)) {
					GameObject[] array = __instance.fistPanels;
					for(int j = 0; j < array.Length; j++) {
						array[j].SetActive(false);
					}
				} else {
					GameObject[] array = __instance.fistPanels;
					for(int j = 0; j < array.Length; j++) {
						array[j].SetActive(true);
					}
				}

				// Profit
				__instance.ForceArm((int)Weapon.CurrentArm, false);
				__instance.UpdateFistIcon();
				return false;
			}
		}
	}
}
