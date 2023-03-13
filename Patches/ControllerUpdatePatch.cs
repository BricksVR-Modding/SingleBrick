using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(ControllerButtonInput), nameof(ControllerButtonInput.Update))]
    class ControllerUpdatePatch
    {
        public static bool Prefix(ControllerButtonInput __instance)
        {
            if ((!Object.FindObjectOfType<NormalSessionManager>()?.mainEnvironment?.active).Value || __instance.inMenu)
                __instance.MenuLogic();
            else if (!__instance._reset)
                __instance.ResetMenuState();
            return false;
        }
    }
}
