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

            GameObject.DestroyImmediate(playButton.GetComponent<Button>());
            playButton.AddComponent<Button>();
            Button playButtonComponent = playButton.GetComponent<Button>();

            joinButton.SetActive(false);

            Vector3 templatePosition = settingsButton.transform.localPosition;
            playButton.transform.localPosition = new Vector3(templatePosition.x, playButton.transform.localPosition.y, templatePosition.z);
            playButton.transform.localScale = settingsButton.transform.localScale;

            TextMeshProUGUI text = playButton.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Play";

            Action action = () =>
                MelonCoroutines.Start(HandlePlay());

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
                sessionManager.brickPickerMenu.WarmMenu();
                BrickColorMap.WarmColorDictionary();
                

                sessionManager.buttonInput.DisableMenuControls();

                GameObject customModel = GameObject.Instantiate(Resources.Load<GameObject>("Custom VR Player"));
                customModel.AddComponent<Scripts.Avatar>();
                GameObject.Destroy(customModel.GetComponent<RealtimeTransform>());
                GameObject.Destroy(customModel.GetComponent<RealtimeAvatar>());
                GameObject.Destroy(customModel.GetComponent<RealtimeView>());

                AvatarNicknameSync avatarSync = customModel.GetComponent<AvatarNicknameSync>();

                avatarSync._isSelf = true;
                avatarSync.nameText.enabled = false;
                avatarSync.face.SetActive(false);

                sessionManager.mainEnvironment.SetActive(true);
                sessionManager.menuEnvironment.SetActive(false);
                sessionManager.playerControllers.transform.position = sessionManager.gameSpawnPoint.position;
                sessionManager.playerControllers.transform.rotation = sessionManager.gameSpawnPoint.rotation;

                sessionManager.menuBoard.SetActive(false);

                yield return new WaitForSeconds(0.25f);

                sessionManager.joystickLocomotion.enabled = true;

                sessionManager._didConnectToRoom = true;
            }
        }
    }
}
