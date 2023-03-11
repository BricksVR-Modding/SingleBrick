using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(BrickPickerManager), nameof(BrickPickerManager.LateUpdate))]
    class PickerUpdatePatch
    {
        public static bool Prefix(BrickPickerManager __instance)
        {
            if (!__instance.normalSessionManager.mainEnvironment.active || __instance.normalSessionManager.InGameMenuUp()) return false;

            if (__instance._holdingMenu)
                __instance.RepositionMenu(__instance._holdingMenuWithLeftHand);

            if (__instance._waitingToReleaseLeftButton && (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch) || Input.GetKey(KeyCode.B))) return false;
            if (__instance._waitingToReleaseRightButton && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch)) return false;

            __instance._waitingToReleaseLeftButton = false;
            __instance._waitingToReleaseRightButton = false;

            bool leftJoystickDown;

            // Not currently holding the menu
            if (!__instance._holdingMenu && (
                    (leftJoystickDown = (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch)) || Input.GetKey(KeyCode.B)) ||
                    OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch)
            ))
            {
                __instance.ToggleMenu(leftJoystickDown);
                if (__instance.IsMenuOpen && !__instance._menuClosing)
                {
                    __instance._holdingMenuWithLeftHand = leftJoystickDown;
                    __instance._holdingMenu = true;
                }

                switch (leftJoystickDown)
                {
                    case true:
                        __instance._waitingToReleaseLeftButton = true;
                        break;
                    case false:
                        __instance._waitingToReleaseRightButton = true;
                        break;
                }
            }
            else if (__instance._holdingMenu && (
              (__instance._holdingMenuWithLeftHand && (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.Touch) || !Input.GetKey(KeyCode.B))) ||
              (!__instance._holdingMenuWithLeftHand && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger, OVRInput.Controller.Touch))))
            {
                __instance._holdingMenu = false;
            }

            __instance.ProcessHoveredTiles();
            return false;
        }
    }
}
