using System;
using System.Collections.Generic;
using System.Text;

namespace UKMF {
	public static class IdentifierUtil {
		/// <summary>
		/// Creates a unique identifier by salting a name with your GUID.
		/// </summary>
		public static string CreateIdentifier(string guid, string name) {
			return guid + ":" + name;
		}
	}
}
