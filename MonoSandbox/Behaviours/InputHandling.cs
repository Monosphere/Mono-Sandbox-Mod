using UnityEngine;

namespace MonoSandbox.Behaviours
{
    public class InputHandling : MonoBehaviour
    {
        public static float LeftTrigger, RightTrigger, LeftGrip, RightGrip;
        public static bool LeftPrimary, RightPrimary, LeftSecondary, RightSecondary;

        public void Update()
        {
            LeftTrigger = ControllerInputPoller.instance.leftControllerIndexFloat;
            LeftGrip = ControllerInputPoller.instance.leftControllerGripFloat;
            RightTrigger = ControllerInputPoller.instance.rightControllerIndexFloat;
            RightGrip = ControllerInputPoller.instance.rightControllerGripFloat;
            LeftPrimary = ControllerInputPoller.instance.leftControllerPrimaryButton;
            LeftSecondary = ControllerInputPoller.instance.leftControllerSecondaryButton;
            RightPrimary = ControllerInputPoller.instance.rightControllerPrimaryButton;
            RightSecondary = ControllerInputPoller.instance.rightControllerSecondaryButton;
        }
    }
}
