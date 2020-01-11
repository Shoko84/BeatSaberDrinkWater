namespace DrinkWater
{
    internal class PluginConfig
    {
        public bool RegenerateConfig = true;
        public bool EnablePlugin = true;
        public bool ShowGIFs = true;
        public int  WaitDuration = 5;
        public bool EnableByPlaytime = true;
        public bool EnableByPlaycount = false;
        public int  PlaytimeBeforeWarning = 5;
        public int  PlaycountBeforeWarning = 2;
    }
}