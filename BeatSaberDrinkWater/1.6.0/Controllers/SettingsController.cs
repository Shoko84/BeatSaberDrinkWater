using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;

namespace DrinkWater.Controllers
{
    public class SettingsController : PersistentSingleton<SettingsController>
    {
        [UIParams]
        private BSMLParserParams parserParams;

        [UIValue("enabled-bool")]
        public bool enabledValue
        {
            get => Plugin.config.Value.EnablePlugin;
            set => Plugin.config.Value.EnablePlugin = value;
        }

        [UIValue("show-gif-bool")]
        public bool showGifValue
        {
            get => Plugin.config.Value.ShowGIFs;
            set => Plugin.config.Value.ShowGIFs = value;
        }

        [UIValue("wait-duration-int")]
        public int waitDurationValue
        {
            get => Plugin.config.Value.WaitDuration;
            set => Plugin.config.Value.WaitDuration = value;
        }

        [UIValue("enable-playtime-bool")]
        public bool enableByPlaytimeValue
        {
            get => Plugin.config.Value.EnableByPlaytime;
            set => Plugin.config.Value.EnableByPlaytime = value;
        }

        [UIValue("enable-playtime-count-bool")]
        public bool enableByPlaytimeCount
        {
            get => Plugin.config.Value.EnableByPlaycount;
            set => Plugin.config.Value.EnableByPlaycount = value;
        }

        [UIValue("playtime-warning-int")]
        public int playtimeBeforeWarningValue
        {
            get => Plugin.config.Value.PlaytimeBeforeWarning;
            set => Plugin.config.Value.PlaytimeBeforeWarning = value;
        }

        [UIValue("playcount-warning-int")]
        public int playcountBeforeWarningValue
        {
            get => Plugin.config.Value.PlaycountBeforeWarning;
            set => Plugin.config.Value.PlaycountBeforeWarning = value;
        }

        [UIAction("#apply")]
        public void OnApply()
        {
            Plugin.config.Value.EnablePlugin = enabledValue;
            Plugin.config.Value.ShowGIFs = showGifValue;
            Plugin.config.Value.WaitDuration = waitDurationValue;
            Plugin.config.Value.EnableByPlaytime = enableByPlaytimeValue;
            Plugin.config.Value.EnableByPlaycount = enableByPlaytimeCount;
            Plugin.config.Value.PlaytimeBeforeWarning = playtimeBeforeWarningValue;
            Plugin.config.Value.PlaycountBeforeWarning = playcountBeforeWarningValue;
            Plugin.configProvider.Store(Plugin.config.Value);
        }
    }
}
