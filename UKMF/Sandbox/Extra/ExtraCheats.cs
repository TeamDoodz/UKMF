using BepInEx.Configuration;

namespace UKMF.Sandbox.Extra {
	internal static class ExtraCheats {
		static bool WeaponsRename = MainPlugin.cfg.Bind(
			new ConfigDefinition("Cheats","WeaponsRename"),
			true,
			new ConfigDescription("Renames the WEAPONS cheat category to COMBAT.")
		).Value;

		static bool DoExtraCheats = MainPlugin.cfg.Bind(
			new ConfigDefinition("Cheats", "DoExtraCheats"),
			true,
			new ConfigDescription("Adds god mode and infinite stamina cheats.")
		).Value;

		public static void DoStuff() {
			// Renames
			if(WeaponsRename) {
				CustomCheats.RenameCategory("weapons", "combat");
			}

			// Cheats
			if(DoExtraCheats) {
				CustomCheats.AddCheat(new GodmodeCheat(), "combat");
				CustomCheats.AddCheat(new InfiniteStaminaCheat(), "movement");
			}
		}
	}
}
