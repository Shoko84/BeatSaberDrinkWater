using Harmony;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberDrinkWater.Patches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator), "HandleResultsViewControllerContinueButtonPressed",
    new Type[] { typeof(VRUIViewController) })]
    class HandleResultsViewControllerContinueButtonPressedPatch
    {
        public static bool Prefix(VRUIViewController viewController)
        {
            if (DrinkWaterPanel.Instance.DisplayPanelNeeded)
            {
                DrinkWaterPanel.Instance.ShowDrinkWaterPanel(DrinkWaterPanel.DrinkWaterPanelMode.CONTINUE);
                return false;
            }
            return true;
        }
    }
}