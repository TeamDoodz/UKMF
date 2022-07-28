using HarmonyLib;
using UnityEngine;

namespace UKMF.Sandbox.Extra {
	internal class GodmodeCheat : CustomCheatInfo {
		private bool isActive = false;

		public override string DisplayName => "God Mode";

		public override string Identifier => IdentifierUtil.CreateIdentifier(MainPlugin.GUID, "Godmode");

		public override Sprite Icon => null;

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
