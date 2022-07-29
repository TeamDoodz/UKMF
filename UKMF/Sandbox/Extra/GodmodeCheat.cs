using HarmonyLib;
using UnityEngine;

namespace UKMF.Sandbox.Extra {
	internal class GodmodeCheat : CustomCheatInfo {
		private bool isActive = false;

		public override string DisplayName => "God Mode";

		public override string Identifier => IdentifierUtil.CreateIdentifier(MainPlugin.GUID, "Godmode");

		public override Sprite Icon {
			get {
				var tex = MainPlugin.assets.ReadImage("cheaticon_godmode.png", FilterMode.Point);
				return Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
			}
		}

		public override bool IsActive => isActive;

		public override StatePersistenceMode PersistenceMode => StatePersistenceMode.Persistent;

		public override void Enable() {
			isActive = true;
		}

		public override void Disable() {
			isActive = false;
		}

		[HarmonyPatch(typeof(NewMovement))]
		[HarmonyPatch(nameof(NewMovement.GetHurt))]
		private static class GodmodePatch {
			static bool Prefix() {
				return !CustomCheats.GetCheat(MainPlugin.GUID, "Godmode").IsActive;
			}
		}
	}
}
