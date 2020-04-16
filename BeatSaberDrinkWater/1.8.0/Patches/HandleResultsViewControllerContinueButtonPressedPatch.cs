using System;
using DrinkWater.Settings;
using HarmonyLib;

namespace DrinkWater.Patches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("ContinueButtonPressed")]
    [HarmonyPatch(new Type[] { })]
    class ResultsViewControllerContinueButtonPressedPatch
    {
        public static bool Prefix()
        {
            Plugin.Log.Debug(">>>>>>>> should display?: " + DrinkWaterPanel.Instance.DisplayPanelNeeded);
            if (PluginConfig.Instance.EnablePlugin && DrinkWaterPanel.Instance.DisplayPanelNeeded)
            {
                DrinkWaterPanel.Instance.ShowDrinkWaterPanel(DrinkWaterPanel.DrinkWaterPanelMode.Continue);
                return false;
            }
            return true;
        }
    }
}