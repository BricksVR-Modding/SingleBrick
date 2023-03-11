using System.Collections;
using Normal.Realtime;
using UnityEngine.UI;
using MelonLoader;
using UnityEngine;
using System;
using TMPro;

namespace BricksVR
{
    public class HandleMenu
    {
        public static void HandleButtons(SingleBrick instance)
        {
            GameObject playButton = instance.playButton;
            GameObject joinButton = instance.joinButton;
            GameObject settingsButton = instance.settingsButton;
            Button playButtonComponent = playButton.GetComponent<Button>();

            joinButton.SetActive(false);

            Vector3 templatePosition = settingsButton.transform.localPosition;
            playButton.transform.localPosition = new Vector3(templatePosition.x, playButton.transform.localPosition.y, templatePosition.z);
            playButton.transform.localScale = settingsButton.transform.localScale;

            TextMeshProUGUI text = playButton.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Play";

            Action action = () =>
                MelonCoroutines.Start(HandlePlay());

            playButtonComponent.onClick.RemoveAllListeners();
            playButtonComponent.onClick.AddListener(action);

        }

        public static IEnumerator HandlePlay()
        {
            NormalSessionManager sessionManager = GameObject.Find("MetaObjects/NormalSessionManager").GetComponent<NormalSessionManager>();
            if (sessionManager != null)
            {
                sessionManager._brickStore.ClearAndRemoveFromWorld();


                sessionManager.WarmOtherCaches();
                BrickPrefabCache.GetInstance().GenerateCache();
                yield return sessionManager.brickPickerMenu.WarmMenu();
                BrickColorMap.WarmColorDictionary();
                

                sessionManager.buttonInput.DisableMenuControls();

                sessionManager.mainEnvironment.SetActive(true);
                sessionManager.menuEnvironment.SetActive(false);
                sessionManager.playerControllers.transform.position = sessionManager.gameSpawnPoint.position;
                sessionManager.playerControllers.transform.rotation = sessionManager.gameSpawnPoint.rotation;

                sessionManager.menuBoard.SetActive(false);

                yield return new WaitForSeconds(0.25f);

                sessionManager.joystickLocomotion.enabled = true;

                sessionManager._didConnectToRoom = true;
                GameObject localHead = sessionManager.realtimeGameobject.GetComponent<RealtimeAvatarManager>().localAvatar.head.gameObject;
                Renderer[] localHeadRenderers = localHead.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in localHeadRenderers)
                {
                    r.enabled = false;
                }
            }
        }
    }
}
