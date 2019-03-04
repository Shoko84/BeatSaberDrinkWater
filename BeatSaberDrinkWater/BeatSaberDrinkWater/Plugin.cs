using BeatSaberDrinkWater.Settings;
using BeatSaberDrinkWater.Utilities;
using Harmony;
using IllusionPlugin;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace BeatSaberDrinkWater
{
    public class Plugin : IPlugin
    {
        public string Name => "BeatSaberDrinkWater";
        public string Version => "0.0.1";
        public void OnApplicationStart()
        {
            TimeSpentClock.OnLoad();
            IngameInformationsCounter.OnLoad();
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            PluginConfig.LoadOrCreateConfig();
            HarmonyInstance harmony = HarmonyInstance.Create("com.Shoko84.beatsaber.BeatSaberDrinkWater");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            PluginConfig.SaveConfig();
        }

        private void SceneManagerOnActiveSceneChanged(Scene from, Scene to)
        {
            if (from.name == "EmptyTransition" && SceneUtils.IsMenuScene(to))
            {
                try
                {
                    SettingsUI.Instance.OnLoad();
                    if (PluginConfig.EnablePlugin)
                        DrinkWaterPanel.Instance.OnLoad();
                    else if (DrinkWaterPanel.Instance != null)
                    {
                        //Clean gameobjects created by the plugin
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception on scene change: " + e);
                }
            }

            if (SceneUtils.IsGameScene(from) && SceneUtils.IsMenuScene(to))
            {
                try
                {
                    if (PluginConfig.EnablePlugin)
                        DrinkWaterPanel.Instance.OnMapFinished();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception on scene change: " + e);
                }
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
