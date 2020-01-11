using Harmony;
using System;

namespace DrinkWater.Patches
{
    [HarmonyPatch(typeof(ResultsViewController))]
    [HarmonyPatch("ContinueButtonPressed")]
    [HarmonyPatch(new Type[] { })]
    class ResultsViewControllerContinueButtonPressedPatch
    {
        public static bool Prefix()
        {
            Logger.log.Debug(">>>>>>>> patched?");
            Logger.log.Debug(">>>>>>>> should display?: " + DrinkWaterPanel.Instance.DisplayPanelNeeded);
            if (Plugin.config.Value.EnablePlugin && DrinkWaterPanel.Instance.DisplayPanelNeeded)
            {
                DrinkWaterPanel.Instance.ShowDrinkWaterPanel(DrinkWaterPanel.DrinkWaterPanelMode.Continue);
                return false;
            }
            return true;
        }
    }
}