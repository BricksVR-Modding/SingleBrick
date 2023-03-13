using UnityEngine.UI;
using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(NormalSessionManager), nameof(NormalSessionManager.Update))]
    class SessionUpdatePatch
    {
        public static bool Prefix(NormalSessionManager __instance)
        {
            if (__instance.mainEnvironment.active && OVRInput.GetUp(OVRInput.Button.Start, OVRInput.Controller.Touch))
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

                    Object.FindObjectOfType<Scripts.Avatar>()?.customLeft?.gameObject?.SetActive(false);
                    Object.FindObjectOfType<Scripts.Avatar>()?.customRight?.gameObject?.SetActive(false);

                    GameObject.Find("MenuBoard/InGame Main/RoomOptionsButton").GetComponent<Button>().interactable = false;
                    GameObject.Find("MenuBoard/InGame Main/PlayersMenu").GetComponent<Button>().interactable = false;
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

                    GameObject.FindObjectOfType<Scripts.Avatar>()?.customLeft?.gameObject?.SetActive(true);
                    GameObject.FindObjectOfType<Scripts.Avatar>()?.customRight?.gameObject?.SetActive(true);
                }
            }

            return false;
        }
    }
}
