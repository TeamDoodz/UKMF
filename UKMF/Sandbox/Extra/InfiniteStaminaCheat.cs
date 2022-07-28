using HarmonyLib;
using UnityEngine;

namespace UKMF.Sandbox.Extra {
	internal class InfiniteStaminaCheat : CustomCheatInfo {
		private bool isActive = false;

		public override string DisplayName => "Infinite Stamina";

		public override string Identifier => IdentifierUtil.CreateIdentifier(MainPlugin.GUID, "InfiniteStamina");

		public override Sprite Icon => null;

		public override bool IsActive => isActive;

		public override StatePersistenceMode PersistenceMode => StatePersistenceMode.Persistent;

		public override void Enable() {
			isActive = true;
		}

		public override void Disable() {
			isActive = false;
		}

		public override void Update() {
			NewMovement.Instance.FullStamina();
		}
	}
}
