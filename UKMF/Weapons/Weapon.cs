using UnityEngine;

namespace UKMF.Weapons {
	/// <summary>
	/// Helper class for handling weapon configuration.
	/// </summary>
	public static class Weapon {
		/// <summary>
		/// The arm that the player is currently holding.
		/// </summary>
		public static FistType CurrentArm {
			get {
				return (FistType)PlayerPrefs.GetInt("CurArm");
			}
			set {
				PlayerPrefs.SetInt("CurArm", (int)value);
			}
		}

		/// <summary>
		/// Returns the color of the specified weapon variation.
		/// </summary>
		/// <param name="index">
		/// The variation in question, where:<br/>
		/// 0 = Blue<br/>
		/// 1 = Green<br/>
		/// 2 = Red<br/>
		/// 3 = Gold<br/>
		/// </param>
		public static Color GetVariationColor(int index) {
			string key = "hudColor.var"+index+".";
			string keyR = key + "r";
			string keyG = key + "g";
			string keyB = key + "b";
			return new Color(PrefsManager.Instance.GetFloat(keyR), PrefsManager.Instance.GetFloat(keyG), PrefsManager.Instance.GetFloat(keyB));
		}

		/// <summary>
		/// Whether or not the specified weapon variation is equipped.
		/// </summary>
		/// <param name="weaponName">
		/// The name of the weapon, where:<br/>
		/// arm = Arm<br/>
		/// rev = Revolver<br/>
		/// sho = Shotgun<br/>
		/// nai = Nailgun<br/>
		/// rai = Railcannon<br/>
		/// </param>
		/// <param name="variation">
		/// The variation in question, where:<br/>
		/// 0 = Blue<br/>
		/// 1 = Green<br/>
		/// 2 = Red<br/>
		/// 3 = Gold<br/>
		/// </param>
		public static WeaponEquipState GetEquipState(string weaponName, int variation) => (WeaponEquipState)PrefsManager.Instance.GetInt("weapon." + weaponName + variation);

		// Make sure to remove the "(int)type > 3" statement after implementing custom arm equipping.
		/// <summary>
		/// Whether or not the specified arm is equipped. Keep in mind that modded arms are always equipped.
		/// </summary>
		public static bool IsArmEquipped(FistType type) => (int)type > 3 || GetEquipState("arm", (int)type) != WeaponEquipState.Unequipped;
	}
}
