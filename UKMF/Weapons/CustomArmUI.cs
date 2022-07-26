using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace UKMF.Weapons {
	internal static class CustomArmUI {
		//TODO: Implement a new UI for enabling/disabling arms
		/*
		[HarmonyPatch(typeof(ShopGearChecker))]
		[HarmonyPatch(nameof(ShopGearChecker.OnEnable))]
		private static class ReplaceUIPatch {
			static void Prefix(ShopGearChecker __instance) {
				Transform armWindow = __instance.transform;
				for(int i=0; i < __instance.transform.childCount; i++) {
					if(__instance.transform.GetChild(i).name == "ArmWindow") {
						armWindow = __instance.transform.GetChild(i);
						break;
					}
				}
				if(armWindow == __instance.transform) return;

				for(int i = 0; i < armWindow.childCount; i++) {
					GameObject.Destroy(armWindow.GetChild(i).gameObject);
				}
			}
		}
		*/
	}
}
