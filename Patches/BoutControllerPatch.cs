using HarmonyLib;
using UnityEngine;
using TotF;

namespace RefMod.Patches
{
    [HarmonyPatch(typeof(BoutController))]
    public static class BoutControllerPatch
    {
        [HarmonyPatch(nameof(BoutController.Awake))]
        [HarmonyPrefix]
        public static void Awake_Prefix(BoutController __instance)
        {
            //Set identifier if the fighter is the ref, for later use
            if (BoutRules.boxerResourceName == Main.referee.boxerResourceName)
            {
                BoutRules.boxerResourceName = null;
                Main.fighterIsRef = true;
                Main.logger.LogInfo("Referee fight started");

                //Set the prefab of the fighter to be melky just for it to work
                //Sort of as a ground base of which we will modify later on
                __instance.enemyPrefab = Resources.Load<GameObject>("TotF/Alexei Petrov");
            }
            else Main.fighterIsRef = false;
        }
    }
}
