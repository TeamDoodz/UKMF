using UnityEngine;

namespace UKMF.Weapons {
	/// <summary>
	/// Defines information about a custom arm.
	/// </summary>
	public class CustomArmInfo {
		/// <summary>
		/// The ID of this arm. Use <see cref="Enums.CustomEnumData.GetValue{FistType}(string, string)"/> to generate an ID.
		/// </summary>
		public FistType ID;
		/// <summary>
		/// The display name of this arm used in the equip terminal.
		/// </summary>
		public string DisplayName;
		/// <summary>
		/// The factory that will be used to generate the arm object.
		/// </summary>
		public AbstractPrefabFactory PrefabFactory;
		/// <summary>
		/// The color of this arm's icon.
		/// </summary>
		public Color IconColor;
		/// <summary>
		/// Whether or not this arm can be equipped for melee.
		/// </summary>
		public bool Holdable;
		/// <summary>
		/// Whether or not this arm is a part of the base game.
		/// </summary>
		public bool IsBaseGame { get; internal set; }
		private GameObject prefab;
		/// <summary>
		/// Creates a copy of this arm's prefab.
		/// </summary>
		public GameObject InstantiatePrefab(Transform parent = null) {
			if(prefab == null) { 
				prefab = PrefabFactory.CreateObject();
				prefab.SetActive(false);
			}
			var outp = GameObject.Instantiate(prefab, parent);
			outp.SetActive(true);
			return outp;
		}
	}
}
