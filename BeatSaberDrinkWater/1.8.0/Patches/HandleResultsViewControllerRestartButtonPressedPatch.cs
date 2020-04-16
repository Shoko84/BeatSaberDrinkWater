using System;
using DrinkWater.Settings;
using HarmonyLib;

namespace DrinkWater.Patches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("RestartButtonPressed")]
    [HarmonyPatch(new Type[] { })]
    class HandleResultsViewControllerRestartButtonPressedPatch
    {
        public static bool Prefix()
        {
            if (PluginConfig.Instance.EnablePlugin && DrinkWaterPanel.Instance.DisplayPanelNeeded)
            {
                DrinkWaterPanel.Instance.ShowDrinkWaterPanel(DrinkWaterPanel.DrinkWaterPanelMode.Restart);
                return false;
            }
            return true;
        }
    }
}