using UnityEngine;

namespace MonoSandbox
{
    public static class RefCache
    {
        public static GameObject LHand, RHand, SandboxContainer;

        public static bool HitExists;
        public static RaycastHit Hit;

        public static Material Default, Selection;
        public static AudioClip PageSelection, ItemSelection;
    }
}
