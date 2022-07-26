using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace UKMF {
	/// <summary>
	/// Allows mods to add their own tools to the spawn menu.
	/// </summary>
	public static class CustomSandboxTools {
		private static List<CheatAssetObject.KeyIcon> customIcons = new List<CheatAssetObject.KeyIcon>();
		/// <summary>
		/// Adds a new sandbox icon. Make sure the name of the icon is unique; you can do this by adding your mod GUID as a prefix to the name.
		/// </summary>
		public static void AddNewIcon(CheatAssetObject.KeyIcon icon) {
			if(customIcons.Any((x) => x.key == icon.key)) {
				MainPlugin.logger.LogWarning($"Found other modded sandbox icons with the same name as \"{icon.key}\"; they will be overwritten. If this was intentional, you can ignore this warning.");
				customIcons.RemoveAll((x) => x.key == icon.key);
			}
			customIcons.Add(icon);
		}
		/// <summary>
		/// Adds a new sandbox icon. Make sure the name of the icon is unique; you can do this by adding your mod GUID as a prefix to the name.
		/// </summary>
		public static void AddNewIcon(string iconName, Sprite icon) {
			AddNewIcon(new CheatAssetObject.KeyIcon() { key = iconName, sprite = icon });
		}

		private static List<SpawnableObject> customObjs = new List<SpawnableObject>();
		/// <summary>
		/// Adds a new sandbox tool. To define an icon you can use <see cref="SpawnableObject.gridIcon"/> or <see cref="AddNewIcon(string, Sprite)"/> with <see cref="SpawnableObject.iconKey"/>.
		/// </summary>
		public static void AddNewTool(SpawnableObject obj) {
			customObjs.Add(obj);
		}
		 
		[HarmonyPatch(typeof(IconManager))]
		[HarmonyPatch(nameof(IconManager.CurrentIcons), MethodType.Getter)]
		private static class AddCustomIconsPatch {
			static bool printedIconNames = false;
			static void Postfix(ref CheatAssetObject __result) {
				if(!printedIconNames) {
					printedIconNames = true;
					MainPlugin.logger.LogDebug("Base Game Sandbox Icons:");
					foreach(var icon in __result.sandboxToolIcons) {
						MainPlugin.logger.LogDebug(icon.key);
					}
				}

				List<CheatAssetObject.KeyIcon> newIcons = new List<CheatAssetObject.KeyIcon>(__result.sandboxToolIcons);
				newIcons.AddRange(customIcons);

				__result.sandboxToolIcons = newIcons.ToArray();
			}
		}

		[HarmonyPatch(typeof(SpawnMenu))]
		[HarmonyPatch(nameof(SpawnMenu.RebuildMenu))]
		private static class AddCustomObjsPatch {
			static bool Prefix(SpawnMenu __instance) {
				__instance.ResetMenu();
				//if(MapInfoBase.InstanceAnyType.sandboxTools) {
					__instance.CreateButtons(__instance.objects.sandboxTools, "SANDBOX TOOLS :^)");
					__instance.CreateButtons(__instance.objects.sandboxObjects, "SANDBOX");
				//}
				__instance.CreateButtons(__instance.objects.enemies, "ENEMIES");
				__instance.CreateButtons(__instance.objects.objects, "ITEMS");
				__instance.CreateButtons(customObjs.ToArray(), "MODDED CONTENT");
				__instance.sectionReference.gameObject.SetActive(false);
				return false;
			}
		}
	}
}
