using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;
using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(BrickPickerBrick), nameof(BrickPickerBrick.Interact))]
    class BrickPickerInteractPatch
    {
        public static bool Prefix(QuickInteractor interactor, BrickPickerBrick __instance)
        {
            if (interactor.GetComponent<XRDirectInteractor>().selectTarget != null)
                return false;
            
            if (!__instance._manager.IsMenuFullyOpen)
                return false;
            
            GameObject brick = Object.Instantiate(
                Resources.Load<GameObject>(__instance._brickData.PrefabName),
                interactor.transform.position,
                __instance._brickMeshRenderer.transform.rotation
            );

            Object.DestroyImmediate(brick.GetComponent<RealtimeTransform>());
            Object.DestroyImmediate(brick.GetComponent<BuildingBrickSync>());
            Object.DestroyImmediate(brick.GetComponent<RealtimeView>());

            brick.GetComponent<Rigidbody>().isKinematic = false;
            
            brick.GetComponent<BrickAttach>().Color = __instance._color;
            string newUUID = BrickId.FetchNewBrickID();
            
            BrickStore brickStore = BrickStore.GetInstance();
            
            GameObject existingBrick = brickStore.Get(newUUID);
            if (existingBrick) existingBrick.GetComponent<BrickAttach>().DelayedDestroy();
            
            brickStore.Put(newUUID, __instance.gameObject);
            
            brick.GetComponent<BrickUuid>().uuid = newUUID;
            
            FadeObjectScaleOnSpawn fadeComponent = brick.AddComponent<FadeObjectScaleOnSpawn>();
            fadeComponent.objectToScale = brick.transform.Find("CombinedModel").gameObject;

            return false;
        }
    }
}
