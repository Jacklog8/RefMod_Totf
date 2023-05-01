using HarmonyLib;
using TotF;
using UnityEngine;

namespace RefMod.Patches
{
    [HarmonyPatch(typeof(ParticleManager))]
    public static class ParticleManagerPatch
    {
        [HarmonyPatch(nameof(ParticleManager.PlayDustAtPoint))]
        [HarmonyPrefix]
        public static bool PlayDustAtPoint_Override(Vector3 position, Quaternion rotation, float intensity, Transform parent, float damage = -1f)
        {
            var instance = AccessTools.StaticFieldRefAccess<ParticleManager>(typeof(ParticleManager), "instance");
            if (instance != null)
            {
                if(Main.fighterIsRef == true)
                    instance.GOPlayDustAtPoint(position, rotation, intensity, parent, 0);
                else instance.GOPlayDustAtPoint(position, rotation, intensity, parent, damage);
            }
            return false;
        }
    }
}
