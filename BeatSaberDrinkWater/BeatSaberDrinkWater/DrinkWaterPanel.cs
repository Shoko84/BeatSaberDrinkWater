using BeatSaberDrinkWater.Settings;
using BeatSaberDrinkWater.Utilities;
using CustomUI.BeatSaber;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberDrinkWater
{
    class DrinkWaterPanel : MonoBehaviour
    {
        public enum DrinkWaterPanelMode : int
        {
            UNKNOWN = -1,
            CONTINUE = 0,
            RESTART = 1
        }

        private CustomMenu _CustomMenu = null;
        private CustomViewController _CustomViewController = null;
        private SoloFreePlayFlowCoordinator _Sfpfc;
        private ResultsViewController _ResultsViewController;

        private DrinkWaterPanelMode _CurrentPanelMode;
        private TextMeshProUGUI _TextContent;
        private Button _ContinueButton;

        public bool Initialized = false;
        public bool DisplayPanelNeeded = false;

        private static DrinkWaterPanel _instance = null;
        public static DrinkWaterPanel Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject("[BeatSaverDrinkWater] DrinkWaterPanel").AddComponent<DrinkWaterPanel>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public void OnLoad()
        {
            Initialized = false;
            _Sfpfc = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().FirstOrDefault();
            if (_Sfpfc != null)
                _ResultsViewController = ReflectionUtil.GetPrivateField<ResultsViewController>(_Sfpfc, "_resultsViewController");
            _SetupUI();
        }

        public void OnMapFinished()
        {
            //TODO: Remove
            //DisplayPanelNeeded = true;
            IngameInformationsCounter.Instance.PlayerHasFinishedMap();
            if (PluginConfig.EnableByPlaytime && IngameInformationsCounter.Instance.IngameTimeSpent.TotalMinutes >= PluginConfig.PlaytimeBeforeWarning)
            {
                IngameInformationsCounter.Instance.ResetTimeSpent();
                DisplayPanelNeeded = true;
            }
            else if (PluginConfig.EnableByPlaycount && IngameInformationsCounter.Instance.CurrentPlaycount >= PluginConfig.PlaycountBeforeWarning)
            {
                IngameInformationsCounter.Instance.ResetPlaycount();
                DisplayPanelNeeded = true;
            }
        }

        private void _SetupDrinkWaterPanel()
        {
            if (_CustomMenu != null && _CustomViewController != null)
                return;

            _CustomMenu = BeatSaberUI.CreateCustomMenu<CustomMenu>("Drink some water!");
            _CustomViewController = BeatSaberUI.CreateViewController<CustomViewController>();

            if (_CustomMenu != null && _CustomViewController != null)
            {
                _CustomMenu.SetMainViewController(_CustomViewController, false, (firstActivation, type) =>
                {
                    if (firstActivation && type == VRUI.VRUIViewController.ActivationType.AddedToHierarchy)
                    {
                        _TextContent = _CustomViewController.CreateText("", new Vector2(0, 20f));
                        _TextContent.alignment = TextAlignmentOptions.Center;
                        _TextContent.fontSize = 5;
                        _TextContent.enableWordWrapping = false;
                        _ContinueButton = _CustomViewController.CreateUIButton("CreditsButton", new Vector2(0, -20f), new Vector2(37f, 10f),
                                                                               null, "I understand!");
                        _ContinueButton.ToggleWordWrapping(false);
                        _ContinueButton.SetButtonTextSize(4);
                        _ContinueButton.onClick.AddListener(delegate () { _CustomMenu.Dismiss(); });

                        _RefreshTextContent(_CurrentPanelMode);

                        //switch (_CurrentPanelMode)
                        //{
                        //    case DrinkWaterPanelMode.CONTINUE:
                        //        _TextContent.text = "Before browsing some new songs, drink some water, that's important for your body!";
                        //        //var a = ReflectionUtil.GetPrivateField<Action<ResultsViewController>>(_ResultsViewController, "restartButtonPressedEvent");
                        //        //_ContinueButton.onClick.AddListener(delegate () { _ResultsViewController.ContinueButtonPressed(); });
                        //        //_ContinueButton.onClick.AddListener(delegate () {
                        //        //    ReflectionUtil.GetPrivateField<Action<ResultsViewController>>(_ResultsViewController, "continueButtonPressedEvent")?.Invoke(_ResultsViewController);
                        //        //});
                        //        break;
                        //    case DrinkWaterPanelMode.RESTART:
                        //        _TextContent.text = "Before restarting this song, drink some water, that's important for your body!";
                        //        //_ContinueButton.onClick.AddListener(delegate () { _ResultsViewController.RestartButtonPressed(); });
                        //        //_ContinueButton.onClick.AddListener(delegate () {
                        //        //    ReflectionUtil.GetPrivateField<Action<ResultsViewController>>(_ResultsViewController, "restartButtonPressedEvent")?.Invoke(_ResultsViewController);
                        //        //});
                        //        break;
                        //    default:
                        //        break;
                        //}
                    }
                });
            }
            else
                Console.WriteLine("[BeatSaberCustomExit.MainMenuUIOverload]: '_CustomMenu' or '_CustomViewController' was null.");
        }

        public void ShowDrinkWaterPanel(DrinkWaterPanelMode mode)
        {
            if (_CustomMenu != null && _ResultsViewController != null)
            {
                _CurrentPanelMode = mode;
                _CustomMenu.Present();
                _RefreshTextContent(mode);
                StartCoroutine(MakeButtonInteractableDelay(_ContinueButton, PluginConfig.WaitDuration, 0.1f, "0.0"));
                DisplayPanelNeeded = false;
            }
        }

        public IEnumerator MakeButtonInteractableDelay(Button button, float duration, float delayStep = 1f, string format = "0", bool showInButton = true)
        {
            string buttonTextContent = button.GetComponentInChildren<TextMeshProUGUI>().text;
            if (showInButton)
                button.SetButtonText(buttonTextContent + " (" + duration.ToString(format) + ")");
            button.interactable = false;
            while (duration > 0)
            {
                yield return new WaitForSeconds(delayStep);
                duration -= delayStep;
                if (duration < 0) duration = 0;
                if (showInButton)
                    button.SetButtonText(buttonTextContent + ((duration > 0) ? (" (" + duration.ToString(format) + ")") : ("")));
            }
            button.interactable = true;
        }

        private void _RefreshTextContent(DrinkWaterPanelMode mode)
        {
            if (_TextContent != null)
                _TextContent.text = ((_CurrentPanelMode == DrinkWaterPanelMode.RESTART) ? ("Before restarting this song") : ("Before browsing some new songs")) + ", drink some water, that's important for your body!";
        }

        private void _SetupUI()
        {
            if (Initialized) return;

            _SetupDrinkWaterPanel();
            Initialized = true;
        }
    }
}
