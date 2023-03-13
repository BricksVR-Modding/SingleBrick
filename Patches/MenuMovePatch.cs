using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(NormalSessionManager), nameof(NormalSessionManager.MoveMenuToFrontOfUser))]
    class MenuMovePatch
    {
        public static bool Prefix(NormalSessionManager __instance)
        {
            float verticalOffset = __instance.mainEnvironment.active ? 0.2f : 1.4f;
            Vector3 gazeDirection = __instance.head.transform.forward;
            gazeDirection.y = 0f;
            Vector3 headPosition = __instance.head.transform.position;
            __instance.menuBoard.transform.position = headPosition + (gazeDirection.normalized * (__instance.mainEnvironment.active ? 2.8f : 10f));
            __instance.menuBoard.transform.rotation = Quaternion.LookRotation(__instance.menuBoard.transform.position - headPosition);
            __instance.menuBoard.transform.position += new Vector3(0, verticalOffset, 0);

            return false;
        }
    }
}
