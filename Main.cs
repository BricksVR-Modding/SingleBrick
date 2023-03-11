using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System.Collections;

namespace BricksVR
{
    public static class BuildInfo
    {
        public const string Name = "SingleBrick";
        public const string Description = "A single-player BricksVR Mod!";
        public const string Author = "ATXLtheAxolotl#2134";
        public const string Company = "BricksVR Modding";
        public const string Version = "0.1.0";
        public const string DownloadLink = "https://github.com/BricksVR-Modding/SingleBrick/releases";
    }

    public class SingleBrick : MelonMod
    {
        public GameObject settingsButton;
        public GameObject playButton;
        public GameObject joinButton;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            playButton = GameObject.Find("MenuBoard/Main/CreateButton");
            joinButton = GameObject.Find("MenuBoard/Main/JoinButton");
            settingsButton = GameObject.Find("MenuBoard/Main/SettingsButton");

            HandleMenu.HandleButtons(this);
        }
    }
}