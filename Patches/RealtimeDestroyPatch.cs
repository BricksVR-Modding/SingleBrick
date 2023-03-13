using Normal.Realtime;
using UnityEngine;
using HarmonyLib;
using System;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(Realtime), nameof(Realtime.Destroy), new Type[] { typeof(GameObject) })]
    class RealtimeDestroyPatch
    {
        public static bool Prefix(GameObject gameObject)
        {
            GameObject.Destroy(gameObject);
            return false;
        }
    }
}
