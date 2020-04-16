using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using BS_Utils.Utilities;
using DrinkWater.Controllers;
using DrinkWater.Models;
using DrinkWater.Settings;
using HarmonyLib;
using IPA;
using IPA.Config.Stores;
using Config = IPA.Config.Config;
using IPALogger = IPA.Logging.Logger;

namespace DrinkWater
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        #region Properties

        public static IPALogger Log { get; private set; }
        public SceneState CurrentSceneState { get; private set; } = SceneState.Menu;

        #endregion

        #region BSIPA Events

        [Init]
        public void Init(IPALogger logger, Config conf)
        {
            Log = logger;
            PluginConfig.Instance = conf.Generated<PluginConfig>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            BSEvents.menuSceneLoadedFresh += MenuLoadFresh;
            IngameInformationsCounter.OnLoad();
            var harmony = new Harmony("com.Shoko84.beatsaber.DrinkWater");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            BSEvents.menuSceneActive += OnMenuSceneActive;
            BSEvents.gameSceneActive += OnGameSceneActive;
        }

        #endregion

        #region Events

        public void OnMenuSceneActive()
        {
            if (CurrentSceneState == SceneState.Menu) return;
            CurrentSceneState = SceneState.Menu;
            if (PluginConfig.Instance.EnablePlugin)
                DrinkWaterPanel.Instance.OnMapFinished();
        }

        public void OnGameSceneActive()
        {
            if (CurrentSceneState == SceneState.Game) return;
            CurrentSceneState = SceneState.Game;
        }

        public void MenuLoadFresh()
        {
            BSMLSettings.instance.AddSettingsMenu("Drink Water", "DrinkWater.Views.settings.bsml", SettingsController.instance);
            if (PluginConfig.Instance.EnablePlugin)
                DrinkWaterPanel.Instance.OnLoad();
        }

        #endregion
    }
}
