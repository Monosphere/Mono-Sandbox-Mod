using UnityEngine;

namespace MonoSandbox.Behaviours
{
    public class HapticManager
    {
        public enum HapticType
        {
            Create, Use, Constant
        }

        public static void Haptic() => Haptic(HapticType.Create);

        public static void Haptic(HapticType hapticType)
        {
            if (hapticType == HapticType.Constant)
            {
                GorillaTagger.Instance.StartVibration(false, GorillaTagger.Instance.tapHapticStrength / 10f, Time.deltaTime);
                return;
            }

            GorillaTagger.Instance.StartVibration(false, hapticType == HapticType.Create ? 0.1f : 0.5f, GorillaTagger.Instance.tapHapticDuration / 1.25f);
        }
    }
}
