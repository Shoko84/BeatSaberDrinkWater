using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CustomUI.Utilities;
using CustomUI.MenuButton;
using CustomUI.Settings;

namespace BeatSaberDrinkWater.Settings
{
    class SettingsUI : MonoBehaviour
    {
        public bool Initialized = false;

        private static SettingsUI _Instance = null;
        public static SettingsUI Instance
        {
            get
            {
                if (!_Instance)
                {
                    _Instance = new GameObject("[BeatSaberDrinkWater] SettingsUI").AddComponent<SettingsUI>();
                    DontDestroyOnLoad(_Instance.gameObject);
                }
                return _Instance;
            }
            private set
            {
                _Instance = value;
            }
        }

        public void OnLoad()
        {
            Initialized = false;
            SetupUI();
        }

        private void SetupUI()
        {
            if (Initialized) return;

            var customExitSubMenu = CustomUI.Settings.SettingsUI.CreateSubMenu("Drink Water");

            var enablePlugin = customExitSubMenu.AddBool("Enable", "Enable or not the plugin");
            enablePlugin.GetValue += delegate { return PluginConfig.EnablePlugin; };
            enablePlugin.SetValue += delegate (bool value) { PluginConfig.EnablePlugin = value; PluginConfig.SaveConfig(); };
            var showGIFs = customExitSubMenu.AddBool("Show GIFs", "Display or not GIFs on the warning panel");
            showGIFs.GetValue += delegate { return PluginConfig.ShowGIFs; };
            showGIFs.SetValue += delegate (bool value) { PluginConfig.ShowGIFs = value; PluginConfig.SaveConfig(); };
            var waitDuration = customExitSubMenu.AddInt("Wait duration", "Waiting time in seconds before skipping the warning panel", 0, 30, 1);
            waitDuration.GetValue += delegate { return PluginConfig.WaitDuration; };
            waitDuration.SetValue += delegate (int value) { PluginConfig.WaitDuration = value; PluginConfig.SaveConfig(); };
            var enableByPlaytime = customExitSubMenu.AddBool("Enable by playtime", "Enable the plugin depending of the playtime");
            enableByPlaytime.GetValue += delegate { return PluginConfig.EnableByPlaytime; };
            enableByPlaytime.SetValue += delegate (bool value) { PluginConfig.EnableByPlaytime = value; PluginConfig.SaveConfig(); };
            var enableByPlaycount = customExitSubMenu.AddBool("Enable by play count", "Enable the plugin depending of the play count");
            enableByPlaycount.GetValue += delegate { return PluginConfig.EnableByPlaycount; };
            enableByPlaycount.SetValue += delegate (bool value) { PluginConfig.EnableByPlaycount = value; PluginConfig.SaveConfig(); };
            var playtimeBeforeWarning = customExitSubMenu.AddInt("Playtime warning", "How much playtime before being warned in minutes ('Enable by playtime' must be true)", 1, 30, 1);
            playtimeBeforeWarning.GetValue += delegate { return PluginConfig.PlaytimeBeforeWarning; };
            playtimeBeforeWarning.SetValue += delegate (int value) { PluginConfig.PlaytimeBeforeWarning = value; PluginConfig.SaveConfig(); };
            var playcountBeforeWarning = customExitSubMenu.AddInt("Playcount warning", "How much play count before being warned ('Enable by play count' must be true)", 1, 5, 1);
            playcountBeforeWarning.GetValue += delegate { return PluginConfig.PlaycountBeforeWarning; };
            playcountBeforeWarning.SetValue += delegate (int value) { PluginConfig.PlaycountBeforeWarning = value; PluginConfig.SaveConfig(); };

            Initialized = true;
        }
    }
}
