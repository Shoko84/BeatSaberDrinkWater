using BeatSaberMarkupLanguage;
using DrinkWater.Controllers;
using HMUI;

namespace DrinkWater.FlowCoordinators
{
    internal class DrinkWaterFlowCoordinator : FlowCoordinator
    {
        public FlowCoordinator oldCoordinator;
        public DrinkWaterPanelController panelController;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (!addedToHierarchy)
                return;
            panelController = BeatSaberUI.CreateViewController<DrinkWaterPanelController>();
            panelController.flowCoordinatorOwner = this;
            ProvideInitialViewControllers(panelController);
        }
    }
}
