using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UKMF.Sandbox {
	public class SimpleCheat : CustomCheatInfo {
		private string displayName;
		private string identifier;
		private Sprite icon;

		private Action activate;

		public override string DisabledText => "ACTIVATE";

		public override string DisplayName => displayName;

		public override string Identifier => identifier;

		public override Sprite Icon => icon;

		public override bool IsActive => false;

		public override StatePersistenceMode PersistenceMode => StatePersistenceMode.NotPersistent;

		public override void Disable() { }

		public override void Enable() {
			activate?.Invoke();
		}

		public SimpleCheat(string displayName, string guid, string name, Sprite icon = null, Action activate = null) { 
			this.displayName = displayName;
			this.identifier = IdentifierUtil.CreateIdentifier(guid, name);
			this.icon = icon;
			this.activate = activate;
		}
	}
}
