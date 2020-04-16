using BeatSaberDrinkWater.UniGif;
using BeatSaberDrinkWater.UniGif.Utility;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using BS_Utils.Utilities;
using DrinkWater.FlowCoordinators;
using DrinkWater.Settings;
using TMPro;
using UnityEngine.UI;

namespace DrinkWater.Controllers
{
    internal class DrinkWaterPanelController : BSMLResourceViewController
    {
        public override string ResourceName => "DrinkWater.Views.drinkwater-warn.bsml";

        public DrinkWaterFlowCoordinator flowCoordinatorOwner;

        [UIComponent("gif-image")] public RawImage rawImageGif;
        [UIComponent("text-content")] public TextMeshProUGUI textContent;
        [UIComponent("continue-btn")] public Button continueButton;

        private float _prefWidth = 30f;
        [UIValue("pref-width-val")]
        public float prefWidth {
            get { return _prefWidth; }
            set
            {
                _prefWidth = value;
                NotifyPropertyChanged();
            }
        }

        private float _prefHeight = 30f;
        [UIValue("pref-height-val")]
        public float prefHeight {
            get { return _prefHeight; }
            set
            {
                _prefHeight = value;
                NotifyPropertyChanged();
            }
        }

        public UniGifImage UniGif { get; private set; }

        

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            base.DidActivate(firstActivation, type);
            if (firstActivation && PluginConfig.Instance.EnablePlugin && PluginConfig.Instance.ShowGIFs)
            {
                UniGif = rawImageGif.gameObject.AddComponent<UniGifImage>();
                var ugiac = UniGif.gameObject.AddComponent<UniGifImageAspectController>();
                UniGif.SetPrivateField("m_imgAspectCtrl", ugiac);
            }
        }

        [UIAction("continue-btn-click")]
        public void ContinueButtonClicked()
        {
            flowCoordinatorOwner.oldCoordinator.DismissFlowCoordinator(flowCoordinatorOwner);
        }
    }
}
