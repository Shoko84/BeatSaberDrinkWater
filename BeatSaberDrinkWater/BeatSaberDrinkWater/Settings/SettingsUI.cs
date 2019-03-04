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

        private static SettingsUI _instance = null;
        public static SettingsUI Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject("[BeatSaberDrinkWater] SettingsUI").AddComponent<SettingsUI>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
            private set
            {
                _instance = value;
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

            Initialized = true;
        }
    }
}
