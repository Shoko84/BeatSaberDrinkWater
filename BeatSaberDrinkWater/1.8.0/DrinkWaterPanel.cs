using System;
using System.Collections;
using System.IO;
using System.Linq;
using BeatSaberMarkupLanguage;
using DrinkWater.FlowCoordinators;
using DrinkWater.Settings;
using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DrinkWater
{
    class DrinkWaterPanel : MonoBehaviour
    {
        public enum DrinkWaterPanelMode : int
        {
            Continue = 0,
            Restart  = 1
        }

        private readonly string[] _defaultGifs = {
            "https://media1.tenor.com/images/013d560bab2b0fc56a2bc43b8262b4ed/tenor.gif", "https://i.giphy.com/zWOnltJgKVlsc.gif",
            "https://i.giphy.com/3ohhwF34cGDoFFhRfy.gif", "https://i.giphy.com/eRBa4tzlbNwE8.gif"
        };

        private DrinkWaterFlowCoordinator _flowCoordinator;
        private string[]                  _gifRotation;

        private DrinkWaterPanelMode _currentPanelMode;

        public bool Initialized        { get; private set; }
        public bool DisplayPanelNeeded { get; private set; }

        private static DrinkWaterPanel _Instance;

        public static DrinkWaterPanel Instance {
            get {
                if (!_Instance)
                {
                    _Instance = new GameObject("[BeatSaverDrinkWater] DrinkWaterPanel").AddComponent<DrinkWaterPanel>();
                    DontDestroyOnLoad(_Instance.gameObject);
                }
                return _Instance;
            }
        }

        private void Awake()
        {
            Random.InitState((int)DateTime.Now.Ticks);
        }

        public void OnLoad()
        {
            Initialized = false;
            var pathConfigFolder = Path.Combine(Environment.CurrentDirectory, "UserData");
            var pathConfigFile = Path.Combine(pathConfigFolder, "DrinkWaterGifRotation.cfg");
            if (!Directory.Exists(pathConfigFolder))
                Directory.CreateDirectory(pathConfigFolder);
            if (!File.Exists(pathConfigFile))
            {
                using (var sw = File.CreateText(pathConfigFile))
                {
                    foreach (var defaultGif in _defaultGifs)
                        sw.WriteLine(defaultGif);
                }
                _gifRotation = _defaultGifs;
            }
            else
                _gifRotation = File.ReadAllLines(pathConfigFile);
            SetupUI();
        }

        public void OnMapFinished()
        {
            IngameInformationsCounter.Instance.PlayerHasFinishedMap();
            if (PluginConfig.Instance.EnablePlugin && PluginConfig.Instance.EnableByPlaytime && IngameInformationsCounter.Instance.IngameTimeSpent.TotalMinutes >= PluginConfig.Instance.PlaytimeBeforeWarning)
            {
                IngameInformationsCounter.Instance.ResetTimeSpent();
                DisplayPanelNeeded = true;
            }
            else if (PluginConfig.Instance.EnablePlugin && PluginConfig.Instance.EnableByPlaycount && IngameInformationsCounter.Instance.CurrentPlaycount >= PluginConfig.Instance.PlaycountBeforeWarning)
            {
                IngameInformationsCounter.Instance.ResetPlaycount();
                DisplayPanelNeeded = true;
            }
        }

        private void SetupDrinkWaterPanel()
        {
            _flowCoordinator = BeatSaberUI.CreateFlowCoordinator<DrinkWaterFlowCoordinator>();
        }

        public void ShowDrinkWaterPanel(DrinkWaterPanelMode mode)
        {
            if (PluginConfig.Instance.EnablePlugin)
            {
                var currentFlow = Resources.FindObjectsOfTypeAll<FlowCoordinator>().FirstOrDefault(f => f.isActivated);
                _flowCoordinator.oldCoordinator = currentFlow;
                currentFlow.PresentFlowCoordinator(_flowCoordinator);
                _currentPanelMode = mode;
                if (PluginConfig.Instance.ShowGIFs)
                    StartCoroutine(DisplayGifFromRotation());
                RefreshTextContent();
                StartCoroutine(MakeButtonInteractableDelay(_flowCoordinator.panelController.continueButton, PluginConfig.Instance.WaitDuration, 0.1f, "0.0"));
                DisplayPanelNeeded = false;
            }
        }

        public IEnumerator MakeButtonInteractableDelay(Button button, float duration, float delayStep = 1f, string format = "0", bool showInButton = true)
        {
            var buttonTextContent = button.GetComponentInChildren<TextMeshProUGUI>().text;
            if (showInButton)
                button.SetButtonText(buttonTextContent + ((duration > 0) ? (" (" + duration.ToString(format) + ")") : ("")));
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

        private IEnumerator DisplayGifFromRotation()
        {
            if (_flowCoordinator != null)
            {
                Coroutine gifCoroutine = null;
                _flowCoordinator.panelController.rawImageGif.enabled = false;
                try
                {
                    var gifUrl = _gifRotation[Random.Range(0, _gifRotation.Length)];
                    gifCoroutine = StartCoroutine(_flowCoordinator.panelController.UniGif.SetGifFromUrlCoroutine(gifUrl));
                }
                catch (Exception e)
                {
                    Plugin.Log.Error("Exception catched on displaying gif: " + e);
                }
                yield return gifCoroutine;
                var ratio = new Vector2(_flowCoordinator.panelController.UniGif.width  / 120f,
                                        _flowCoordinator.panelController.UniGif.height / 30f);
                if (ratio.x > 1 || ratio.y > 1)
                {
                    var ratioMax = Math.Max(ratio.x, ratio.y);
                    _flowCoordinator.panelController.prefWidth = (int)(_flowCoordinator.panelController.UniGif.width   / ratioMax);
                    _flowCoordinator.panelController.prefHeight = (int)(_flowCoordinator.panelController.UniGif.height / ratioMax);
                    Plugin.Log.Debug(">>>>> width: "  + _flowCoordinator.panelController.prefWidth);
                    Plugin.Log.Debug(">>>>> height: " + _flowCoordinator.panelController.prefHeight);
                }
                _flowCoordinator.panelController.rawImageGif.enabled = true;
            }
        }

        private void RefreshTextContent()
        {
            if (_flowCoordinator.panelController.textContent != null)
                _flowCoordinator.panelController.textContent.text = (_currentPanelMode == DrinkWaterPanelMode.Restart ? "Before restarting this song" : "Before browsing some new songs") +
                                                                    ", drink some water, that's important for your body!";
        }

        private void SetupUI()
        {
            if (Initialized) return;

            SetupDrinkWaterPanel();
            Initialized = true;
        }
    }
}