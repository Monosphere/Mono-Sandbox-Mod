using UnityEngine;

namespace MonoSandbox
{
    /// <summary>
    /// This class is used to provide information about your mod to BepInEx.
    /// </summary>
    internal class PluginInfo
    {
        public const string GUID = "com.monosphere.gorillatag.monosandbox";
        public const string Name = "MonoSandbox";
        public const string Version = "1.0.4";

        public static GameObject leftHand;
        public static GameObject rightHand;
        public static RaycastHit raycastHit;
        // These variables are set in the Plugin.

        public const bool usedHand = false;
        // true for Left hand, false for Right hand.
    }
}
