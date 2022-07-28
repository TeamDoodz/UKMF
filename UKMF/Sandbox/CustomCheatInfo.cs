using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace UKMF.Sandbox {
	// The developers were shitfaced when designing the cheat system and for some godless reason decided to abstract the code for a cheat to an interface.
	// A goddamn interface.
	// This class aims to remedy this, basically acting as an alternative (and far superiour) abstraction for cheats.
	public abstract class CustomCheatInfo {
		/// <summary>
		/// The display name of this cheat.
		/// </summary>
		public abstract string DisplayName { get; }

		/// <summary>
		/// The internal name of this cheat. Make sure to prefix this with your mod GUID to prevent duplicate names.
		/// </summary>
		public abstract string Identifier { get; }

		/// <summary>
		/// The text that displays on this cheat's button when it is enabled. Leave null for the default value.
		/// </summary>
		public virtual string EnabledText { get => null; }

		/// <summary>
		/// The text that displays on this cheat's button when it is disabled. Leave null for the default value.
		/// </summary>
		public virtual string DisabledText { get => null; }

		/// <summary>
		/// The icon for this cheat.
		/// </summary>
		public abstract Sprite Icon { get; }

		/// <summary>
		/// Returns true if this instance is currently active. For cheats that perfrom an action once on enable you can just return false.
		/// </summary>
		public abstract bool IsActive { get; }

		/// <summary>
		/// If this is true, <see cref="Enable"/> will be called when this cheat is registered.
		/// </summary>
		public virtual bool DefaultState { get => false; }

		/// <summary>
		/// How the state of this instance persists across game sessions. If this is <see cref="StatePersistenceMode.Persistent"/>, <see cref="Enable"/> will be called when this cheat is registered.
		/// </summary>
		public abstract StatePersistenceMode PersistenceMode { get; }

		/// <summary>
		/// Called whenever this cheat is enabled or is loaded in with <see cref="DefaultState"/> returning true or <see cref="PersistenceMode"/> returning <see cref="StatePersistenceMode.Persistent"/> and this cheat is saved as enabled.
		/// </summary>
		public abstract void Enable();

		/// <summary>
		/// Called whenever this cheat is disabled.
		/// </summary>
		public abstract void Disable();

		/// <summary>
		/// Called every frame that this cheat is enabled.
		/// </summary>
		public virtual void Update() { }
	}
}
