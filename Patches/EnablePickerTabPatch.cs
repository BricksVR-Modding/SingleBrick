using HarmonyLib;

namespace BricksVR.Patches
{
    [HarmonyPatch(typeof(BrickPickerManager), nameof(BrickPickerManager.EnableTab))]
    class EnablePickerTabPatch
    {
        public static bool Prefix(string tabName, BrickPickerManager __instance)
        {
            BrickPickerManager.MenuTab menuTab = __instance._tabs[tabName];

            menuTab.Gameobject.SetActive(true);
            if (!menuTab.Initialized)
            {
                __instance.InitializeTab(tabName);
                menuTab.Initialized = true;
                __instance._tabs[tabName] = menuTab;
            }
            return false;
        }
    }
}
