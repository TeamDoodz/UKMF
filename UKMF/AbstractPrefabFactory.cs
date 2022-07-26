using UnityEngine;

namespace UKMF {
	/// <summary>
	/// Abstraction for creating prefabs.
	/// </summary>
	public abstract class AbstractPrefabFactory {
		/// <summary>
		/// Creates and returns a <see cref="GameObject"/>. If you return <c>null</c>, no spawning will occur.
		/// </summary>
		public abstract GameObject CreateObject();
	}
}
