using System;
using System.Collections.Generic;
using System.Text;

namespace UKMF.Sandbox.Extra {
	internal static class ExtraCheats {
		public static void DoStuff() {
			// Renames
			CustomCheats.RenameCategory("weapons", "combat");

			// Cheats
			CustomCheats.AddCheat(new GodmodeCheat(), "combat");
			CustomCheats.AddCheat(new InfiniteStaminaCheat(), "movement");
		}
	}
}
