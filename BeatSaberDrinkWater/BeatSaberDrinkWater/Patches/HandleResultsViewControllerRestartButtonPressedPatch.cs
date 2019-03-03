using Harmony;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberDrinkWater.Patches
{
    [HarmonyPatch(typeof(SoloFreePlayFlowCoordinator), "HandleResultsViewControllerRestartButtonPressed",
    new Type[] { typeof(VRUIViewController) })]
    class HandleResultsViewControllerRestartButtonPressedPatch
    {
        public static bool Prefix(VRUIViewController viewController)
        {
            if (DrinkWaterPanel.Instance.DisplayPanelNeeded)
            {
                DrinkWaterPanel.Instance.ShowDrinkWaterPanel(DrinkWaterPanel.DrinkWaterPanelMode.RESTART);
                return false;
            }
            return true;
        }
    }
}