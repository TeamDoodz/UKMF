using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;
using System.Linq;

namespace UKMF.Sandbox {
	public static class CustomSandboxIcons {
		private static List<CheatAssetObject.KeyIcon> customCheatIcons = new List<CheatAssetObject.KeyIcon>();

		public static void AddCheatIcon(string identifier, Sprite icon) {
			customCheatIcons.RemoveAll((x) => x.key == identifier);
			customCheatIcons.Add(new CheatAssetObject.KeyIcon() { key = identifier, sprite = icon});
		}

		[HarmonyPatch(typeof(IconManager))]
		[HarmonyPatch(nameof(IconManager.CurrentIcons), MethodType.Getter)]
		private static class AddCustomIconsPatch {
			static void Postfix(ref CheatAssetObject __result) {
				List<CheatAssetObject.KeyIcon> icons = __result.cheatIcons.ToList();
				icons.RemoveAll((x) => customCheatIcons.Any((y) => x.key == y.key));
				icons.AddRange(customCheatIcons);
				__result.cheatIcons = icons.ToArray();
			}
		}
	}
}
