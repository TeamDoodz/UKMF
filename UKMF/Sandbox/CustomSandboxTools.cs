using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using System.Linq;

namespace UKMF.Sandbox {
	/// <summary>
	/// Allows mods to add their own tools to the spawn menu.
	/// </summary>
	public static class CustomSandboxTools {

		private static List<SpawnableObject> customObjs = new List<SpawnableObject>();
		/// <summary>
		/// Adds a new sandbox tool.
		/// </summary>
		public static void AddNewTool(SpawnableObject obj) {
			customObjs.Add(obj);
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
				if(customObjs.Count > 0) __instance.CreateButtons(customObjs.ToArray(), "MODDED CONTENT");
				__instance.sectionReference.gameObject.SetActive(false);
				return false;
			}
		}
	}
}
