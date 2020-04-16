using BeatSaberMarkupLanguage.Attributes;
using DrinkWater.Settings;

namespace DrinkWater.Controllers
{
    public class SettingsController : PersistentSingleton<SettingsController>
    {
        [UIValue("enabled-bool")]
        public bool enabledValue
        {
            get => PluginConfig.Instance.EnablePlugin;
            set => PluginConfig.Instance.EnablePlugin = value;
        }

        [UIValue("show-gif-bool")]
        public bool showGifValue
        {
            get => PluginConfig.Instance.ShowGIFs;
            set => PluginConfig.Instance.ShowGIFs = value;
        }

        [UIValue("wait-duration-int")]
        public int waitDurationValue
        {
            get => PluginConfig.Instance.WaitDuration;
            set => PluginConfig.Instance.WaitDuration = value;
        }

        [UIValue("enable-playtime-bool")]
        public bool enableByPlaytimeValue
        {
            get => PluginConfig.Instance.EnableByPlaytime;
            set => PluginConfig.Instance.EnableByPlaytime = value;
        }

        [UIValue("enable-playtime-count-bool")]
        public bool enableByPlaytimeCount
        {
            get => PluginConfig.Instance.EnableByPlaycount;
            set => PluginConfig.Instance.EnableByPlaycount = value;
        }

        [UIValue("playtime-warning-int")]
        public int playtimeBeforeWarningValue
        {
            get => PluginConfig.Instance.PlaytimeBeforeWarning;
            set => PluginConfig.Instance.PlaytimeBeforeWarning = value;
        }

        [UIValue("playcount-warning-int")]
        public int playcountBeforeWarningValue
        {
            get => PluginConfig.Instance.PlaycountBeforeWarning;
            set => PluginConfig.Instance.PlaycountBeforeWarning = value;
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            PluginConfig.Instance.EnablePlugin = enabledValue;
            PluginConfig.Instance.ShowGIFs = showGifValue;
            PluginConfig.Instance.WaitDuration = waitDurationValue;
            PluginConfig.Instance.EnableByPlaytime = enableByPlaytimeValue;
            PluginConfig.Instance.EnableByPlaycount = enableByPlaytimeCount;
            PluginConfig.Instance.PlaytimeBeforeWarning = playtimeBeforeWarningValue;
            PluginConfig.Instance.PlaycountBeforeWarning = playcountBeforeWarningValue;
        }
    }
}
