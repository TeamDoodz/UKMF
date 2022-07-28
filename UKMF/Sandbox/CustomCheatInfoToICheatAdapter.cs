using System;
using System.Collections.Generic;
using System.Text;

namespace UKMF.Sandbox {
	/// <summary>
	/// Adapter class for making instances of <see cref="CustomCheatInfo"/> work like they implement the (terribly designed) <see cref="ICheat"/> interface.
	/// </summary>
	internal class CustomCheatInfoToICheatAdapter : ICheat {
		public readonly CustomCheatInfo Info;

		public string LongName => Info.DisplayName;

		public string Identifier => Info.Identifier;

		public string ButtonEnabledOverride => Info.EnabledText;

		public string ButtonDisabledOverride => Info.DisabledText;

		//TODO: Custom cheat icons
		public string Icon => null;

		public bool IsActive => Info.IsActive;

		public bool DefaultState => Info.DefaultState;

		public StatePersistenceMode PersistenceMode => Info.PersistenceMode;

		public void Disable() {
			Info.Disable();
		}

		public void Enable() {
			Info.Enable();
		}

		public void Update() {
			Info.Update();
		}

		public CustomCheatInfoToICheatAdapter(CustomCheatInfo Info) {
			this.Info = Info;
		}
	}
}
