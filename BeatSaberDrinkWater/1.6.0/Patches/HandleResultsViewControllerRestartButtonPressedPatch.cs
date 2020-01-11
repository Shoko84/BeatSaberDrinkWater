using Harmony;
using System;

namespace DrinkWater.Patches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("RestartButtonPressed")]
    [HarmonyPatch(new Type[] { })]
    class HandleResultsViewControllerRestartButtonPressedPatch
    {
        public static bool Prefix()
        {
            if (Plugin.config.Value.EnablePlugin && DrinkWaterPanel.Instance.DisplayPanelNeeded)
            {
                DrinkWaterPanel.Instance.ShowDrinkWaterPanel(DrinkWaterPanel.DrinkWaterPanelMode.Restart);
                return false;
            }
            return true;
        }
    }
}