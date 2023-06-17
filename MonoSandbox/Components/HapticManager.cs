using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MonoSandbox.Components
{
    public class HapticManager
    {
        public static void PlaceBlockHaptic()
        {
            GorillaTagger.Instance.StartVibration(MonoSandbox.PluginInfo.usedHand, GorillaTagger.Instance.tapHapticStrength * 0.4f, GorillaTagger.Instance.tapHapticDuration);
        }

        public static void ShootWeaponHaptic()
        {
            GorillaTagger.Instance.StartVibration(MonoSandbox.PluginInfo.usedHand, GorillaTagger.Instance.tapHapticStrength * 1.1f, GorillaTagger.Instance.tapHapticDuration * 0.5f);
        }

        public static void LaserGunHaptic()
        {
            GorillaTagger.Instance.StartVibration(MonoSandbox.PluginInfo.usedHand, GorillaTagger.Instance.tapHapticStrength * 0.8f, Time.deltaTime);
        }
    }
}
