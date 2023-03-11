using UnityEngine;
using HarmonyLib;
using System;
using UnityEngine.XR.Interaction.Toolkit;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(NormalSessionManager), nameof(NormalSessionManager.WarmOtherCaches))]
    class OtherCacheWarmerPatch
    {
        public static bool Prefix(NormalSessionManager __instance)
        {
            GameObject brick4x2 = GameObject.Instantiate(Resources.Load("4x2") as GameObject, new Vector3(0, -10, 0), Quaternion.identity);
            GameObject brick2x2 = GameObject.Instantiate(Resources.Load("2x2") as GameObject, new Vector3(0, -20, 0), Quaternion.identity);
            GameObject brick1x4 = GameObject.Instantiate(Resources.Load("1x4") as GameObject, new Vector3(0, -30, 0), Quaternion.identity);
            GameObject brick4x2Placed = GameObject.Instantiate(Resources.Load("4x2 - Placed") as GameObject, new Vector3(0, -40, 0), Quaternion.identity);
            GameObject brick2x2Placed = GameObject.Instantiate(Resources.Load("2x2 - Placed") as GameObject, new Vector3(0, -50, 0), Quaternion.identity);
            GameObject brick1x4Placed = GameObject.Instantiate(Resources.Load("1x4 - Placed") as GameObject, new Vector3(0, -60, 0), Quaternion.identity);

            brick4x2.GetComponent<ShowSnappableBrickPositions>().enabled = true; // Warm ShowSnappableBrickPositions update logic, mainly brick attach detection code

            BrickDestroyer destroyer = BrickDestroyer.GetInstance();

            destroyer.DelayedDestroy(brick2x2);
            destroyer.DelayedDestroy(brick1x4);
            destroyer.DelayedDestroy(brick4x2Placed);
            destroyer.DelayedDestroy(brick2x2Placed);
            destroyer.DelayedDestroy(brick1x4Placed);

            // To warm the XRInteractionManager update loop (which is SLOW the first time), force select an object, wait 3 frames to allow Update() to be called
            // then destroy it.
            __instance.xrInteractionManager.ForceSelect(__instance.playerControllers.GetComponentInChildren<XRDirectInteractor>(), brick4x2.GetComponent<XRGrabInteractable>());

            Action destroyBrick = new Action(() =>
            {
                __instance._brickStore.Delete(brick4x2.GetComponent<BrickAttach>().GetUuid());
                destroyer.DelayedDestroy(brick4x2);
            });

            Wait.ForFrames(3, destroyBrick);
            return false;
        }
    }
}
