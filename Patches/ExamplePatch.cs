using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoSandbox.Patches
{
    /// <summary>
    /// This is an example patch, made to demonstrate how to use Harmony. You should remove it if it is not used.
    /// </summary>
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class ExamplePatch
    {
        private static void Postfix(GorillaLocomotion.Player __instance)
        {
            Console.WriteLine(__instance.maxJumpSpeed);
        }
    }
}
