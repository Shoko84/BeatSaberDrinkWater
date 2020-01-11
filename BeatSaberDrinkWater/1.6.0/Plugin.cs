using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Utilities;
using DrinkWater.Controllers;
using DrinkWater.Models;
using Harmony;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using Config = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace DrinkWater
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static Ref<PluginConfig> config;
        internal static IConfigProvider configProvider;

        public SceneState CurrentSceneState { get; private set; } = SceneState.Menu;

        public string Name    => "Drink Water";
        public string Version => "0.11.1";

        public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider cfgProvider)
        {
            Logger.log = logger;
            BSEvents.menuSceneLoadedFresh += MenuLoadFresh;
            configProvider = cfgProvider;

            config = cfgProvider.MakeLink<PluginConfig>((p, v) =>
            {
                if (v.Value == null || v.Value.RegenerateConfig)
                    p.Store(v.Value = new PluginConfig { RegenerateConfig = false });
                config = v;
            });
        }

        public void OnApplicationStart()
        {
            IngameInformationsCounter.OnLoad();
            HarmonyInstance harmony = HarmonyInstance.Create("com.Shoko84.beatsaber.BeatSaberDrinkWater");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            BSEvents.menuSceneActive += OnMenuSceneActive;
            BSEvents.gameSceneActive += OnGameSceneActive;
        }

        public void OnApplicationQuit()
        {

        }

        public void OnFixedUpdate()
        {

        }

        public void OnUpdate()
        {

        }

        public void OnMenuSceneActive()
        {
            if (CurrentSceneState == SceneState.Menu) return;
            Logger.log.Debug(">>>> Plugin|OnMenuSceneActive doing stuff");
            CurrentSceneState = SceneState.Menu;
            if (config.Value.EnablePlugin)
                DrinkWaterPanel.Instance.OnMapFinished();
        }

        public void OnGameSceneActive()
        {
            if (CurrentSceneState == SceneState.Game) return;
            Logger.log.Debug(">>>> Plugin|OnGameSceneActive doing stuff");
            CurrentSceneState = SceneState.Game;
        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        public void MenuLoadFresh()
        {
            BSMLSettings.instance.AddSettingsMenu("Drink Water", "DrinkWater.Views.settings.bsml", SettingsController.instance);
            if (config.Value.EnablePlugin)
                DrinkWaterPanel.Instance.OnLoad();
        }
    }
}
