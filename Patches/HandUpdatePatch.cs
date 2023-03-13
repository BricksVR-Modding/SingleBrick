using UnityEngine.XR.Interaction.Toolkit;
using OVRTouchSample;
using UnityEngine;
using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(Hand), nameof(Hand.Update))]
    class HandUpdatePatch
    {
        public static bool Prefix(Hand __instance)
        {
            if(__instance.mGrabber == null)
            {
                HandReference handRef = GameObject.Find("HandReference").GetComponent<HandReference>();
                __instance.mGrabber = __instance.leftHand
                    ? handRef.leftHand.GetComponent<XRDirectInteractor>()
                    : handRef.rightHand.GetComponent<XRDirectInteractor>();
            }

            __instance.UpdateCapTouchStates();

            __instance._mPointBlend = __instance.InputValueRateChange(__instance._mIsPointing, __instance._mPointBlend);
            __instance._mThumbsUpBlend = __instance.InputValueRateChange(__instance._mIsGivingThumbsUp, __instance._mThumbsUpBlend);

            bool grabbing = __instance.mGrabber.selectTarget != null;
            __instance._changeHandGesture.HandPoseIdDidChange(null, (int)HandPoseId.Default);

            float flex = grabbing ? 1.0f : OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, __instance.mController);
            float pointLockLerpValue = __instance.PointLockLerpValue();

            flex = Mathf.Lerp(flex, 1f, pointLockLerpValue);
            __instance._changeHandGesture.FlexDidChange(null, flex);
            
            bool canPoint = !grabbing;
            float point = canPoint ? __instance._mPointBlend : 0.0f;
            point = Mathf.Lerp(point, grabbing ? 0f : 1f, pointLockLerpValue);
            __instance._changeHandGesture.PointDidChange(null, point);

            // Thumbs up
            bool canThumbsUp = !grabbing;
            float thumbsUp = canThumbsUp ? __instance._mThumbsUpBlend : 0.0f;
            thumbsUp = Mathf.Lerp(thumbsUp, 0f, pointLockLerpValue);
            __instance._changeHandGesture.ThumbsUpDidChange(null, thumbsUp);

            return false;
        }
    }
}
