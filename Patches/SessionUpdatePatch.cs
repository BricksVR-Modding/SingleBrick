using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(NormalSessionManager), nameof(NormalSessionManager.Update))]
    class SessionUpdatePatch
    {
        public static bool Prefix(NormalSessionManager __instance)
        {
            if (__instance.mainEnvironment.active && (OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Touch) || Input.GetKeyUp(KeyCode.M)))
            {
                if (!__instance._inGameMenuUp)
                {
                    __instance._inGameMenuUp = true;

                    __instance.DisableAllMenus();
                    __instance.inGameMain.SetActive(true);

                    __instance.joystickLocomotion.enabled = false;
                    __instance.buttonInput.EnableMenuControls();
                    __instance.menuBoard.SetActive(true);
                    __instance.MoveMenuToFrontOfUser();

                    __instance.menuLeftHand.SetActive(false);
                    __instance.menuRightHand.SetActive(true);

                    __instance.teleporterLeftHand.SetActive(false);
                    __instance.teleporterRightHand.SetActive(false);

                    __instance._avatarManager.localAvatar.leftHand.gameObject.SetActive(false);
                    __instance._avatarManager.localAvatar.rightHand.gameObject.SetActive(false);
                }
                else
                {
                    __instance._inGameMenuUp = false;
                    __instance.adjustPlayerScale.ChangePlayerScale();

                    __instance.joystickLocomotion.enabled = true;
                    __instance.buttonInput.DisableMenuControls();
                    __instance.menuBoard.SetActive(false);

                    __instance.menuLeftHand.SetActive(false);
                    __instance.menuRightHand.SetActive(false);

                    __instance._avatarManager.localAvatar.leftHand.gameObject.SetActive(true);
                    __instance._avatarManager.localAvatar.rightHand.gameObject.SetActive(true);
                }
            }

            return false;
        }
    }
}
