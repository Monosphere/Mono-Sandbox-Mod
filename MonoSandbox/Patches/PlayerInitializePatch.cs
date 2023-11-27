using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoSandbox.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    public class PlayerInitializePatch
    {
        public static void Postfix(GorillaTagger __instance)
        {
            RefCache.LHand = __instance.offlineVRRig.leftHandTransform.parent.Find("palm.01.L").gameObject;
            RefCache.RHand = __instance.offlineVRRig.rightHandTransform.parent.Find("palm.01.R").gameObject;
        }
    }
}
