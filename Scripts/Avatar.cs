using MelonLoader;
using UnityEngine;
using System;

namespace BricksVR.Scripts
{
    [RegisterTypeInIl2Cpp]
    class Avatar : MonoBehaviour
    {
        public Avatar(IntPtr ptr) : base(ptr) { }
        public Transform root;
        public Transform leftHand;
        public Transform rightHand;
        public Transform head;

        public Transform customLeft;
        public Transform customRight;
        public Transform customHead;

        public void Start()
        {
            root = GameObject.Find("Player Controllers/VR Rig").transform;
            leftHand = GameObject.Find("Player Controllers/VR Rig/Camera Offset/LeftHand/Controller").transform;
            rightHand = GameObject.Find("Player Controllers/VR Rig/Camera Offset/RightHand/Controller").transform;
            head = GameObject.Find("Player Controllers/VR Rig/Camera Offset/Head").transform;

            customLeft = transform.FindChild("Left Hand");
            customRight = transform.FindChild("Right Hand");
            customHead = transform.FindChild("Head");

            customHead.Find("Bottom/4x2 - Placed/CombinedModel").GetComponent<MeshRenderer>().enabled = false;
            customHead.Find("Bottom/4x2 - Placed (1)/CombinedModel").GetComponent<MeshRenderer>().enabled = false;
            customHead.Find("Top/4x2 - Placed/CombinedModel").GetComponent<MeshRenderer>().enabled = false;
            customHead.Find("Top/4x2 - Placed (1)/CombinedModel").GetComponent<MeshRenderer>().enabled = false;

            GameObject.Find("Player Controllers/VR Rig/Camera Offset/LeftHand/MenuHand").SetActive(false);
            GameObject.Find("Player Controllers/VR Rig/Camera Offset/RightHand/MenuHand").SetActive(false);
        }

        public void Update()
        {
            transform.position = root.position;
            transform.rotation = root.rotation;
            transform.localScale = root.localScale;

            customHead.position = head.position;
            customHead.rotation = head.rotation;

            customLeft.position = leftHand.position;
            customLeft.rotation = leftHand.rotation;

            customRight.position = rightHand.position;
            customRight.rotation = rightHand.rotation;
        }
    }
}
