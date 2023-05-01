using HarmonyLib;
using TotF;
using UnityEngine;
using System.Collections.Generic;

namespace RefMod.Patches
{
    [HarmonyPatch(typeof(FightMenuManager))]
    public static class FightMenuManagerPatch
    {
        /// <summary>
        /// This is bad, don't do like me.
        /// This method patches the entire method and just adds two lines, the functionality stays the same.
        /// This won't work very well if someone else wants to mod just this method with this mod present.
        /// Instead use a transpiler but who in the actual fuck knows how that works so i didn't.
        /// </summary>
        [HarmonyPatch(nameof(FightMenuManager.Awake))]
        [HarmonyPrefix]
        public static bool FightMenuManager_Awake(FightMenuManager __instance)
        {
            var s_unlockedOpponents = AccessTools.Field(typeof(FightMenuManager), "unlockedOpponents");
            var s_defaultDifficulty = AccessTools.Field(typeof(FightMenuManager), "defaultDifficulty");

            s_unlockedOpponents.SetValue(__instance, new List<FightMenuManager.OpponentDetails>());
            for (int i = 0; i < __instance.opponents.Length; i++)
            {
                List<FightMenuManager.OpponentDetails> list;
                list = s_unlockedOpponents.GetValue(__instance) as List<FightMenuManager.OpponentDetails>;

                if (__instance.opponents[i].IsUnlocked())
                    list.Add(__instance.opponents[i]);

                //Add the referee as the last fighter if the referee is unlocked
                if (i == __instance.opponents.Length - 1 && Main.referee.IsUnlocked())
                    list.Add(Main.referee);

                s_unlockedOpponents.SetValue(__instance, list);
            }

            s_defaultDifficulty.SetValue(__instance, (FightMenuManager.SelectedDifficulty)PlayerPrefs.GetInt("defaultDifficulty", 2));
            if (__instance.customizeMenu != null && !__instance.extraBattle)
            {
                __instance.customizeMenu.fightMenu = __instance;
            }

            return false;
        }
    }
}
