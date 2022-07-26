using System;
using UnityEngine;

namespace UKMF.Weapons {
	internal class BaseGameArmFactory : AbstractPrefabFactory {
		private FistType type;

		public override GameObject CreateObject() {
			switch(type) {
				case FistType.Standard: return FistControl.Instance.blueArm;
				case FistType.Heavy: return FistControl.Instance.redArm;
				case FistType.Spear: return null;
				case (FistType)3: return FistControl.Instance.goldArm;
			}
			throw new NotImplementedException();
		}

		public BaseGameArmFactory(FistType type) {
			this.type = type;
		}
	}
}
