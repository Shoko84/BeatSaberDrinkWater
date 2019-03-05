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
        private UniGifImage _UniGifImage = null;
        private RawImage _RawImage = null;
        private string[] _GifRotation;

        private DrinkWaterPanelMode _CurrentPanelMode;
        private TextMeshProUGUI _TextContent;
        private Button _ContinueButton;

        public bool Initialized = false;
        public bool DisplayPanelNeeded = false;

        private static DrinkWaterPanel _Instance = null;
        public static DrinkWaterPanel Instance
        {
            get
            {
                if (!_Instance)
                {
                    _Instance = new GameObject("[BeatSaverDrinkWater] DrinkWaterPanel").AddComponent<DrinkWaterPanel>();
                    DontDestroyOnLoad(_Instance.gameObject);
                }
                return _Instance;
            }
            private set
            {
                _Instance = value;
            }
        }

        public void OnLoad()
        {
            Initialized = false;
            _GifRotation = new string[] { "https://media1.tenor.com/images/013d560bab2b0fc56a2bc43b8262b4ed/tenor.gif", "https://i.giphy.com/zWOnltJgKVlsc.gif",
                                          "https://i.giphy.com/3ohhwF34cGDoFFhRfy.gif", "https://i.giphy.com/eRBa4tzlbNwE8.gif" };
            _SetupUI();
        }

        public void OnMapFinished()
        {
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
                        _TextContent = _CustomViewController.CreateText("", new Vector2(0, 28f));
                        _TextContent.alignment = TextAlignmentOptions.Center;
                        _TextContent.fontSize = 5;
                        _TextContent.enableWordWrapping = false;
                        _ContinueButton = _CustomViewController.CreateUIButton("CreditsButton", new Vector2(0, -28f), new Vector2(37f, 10f),
                                                                               null, "I understand!");
                        _ContinueButton.ToggleWordWrapping(false);
                        _ContinueButton.SetButtonTextSize(4);
                        _ContinueButton.onClick.AddListener(delegate () { _CustomMenu.Dismiss(); Destroy(_CustomViewController.gameObject, 1); _CustomMenu = null; _CustomViewController = null; });

                        _RefreshTextContent(_CurrentPanelMode);

                        GameObject go = new GameObject("[BeatSaberDrinkWater] PreviewGif");
                        _RawImage = go.AddComponent<RawImage>();
                        _RawImage.material = Instantiate(Resources.FindObjectsOfTypeAll<Material>().Where(m => m.name == "UINoGlow").FirstOrDefault());
                        go.transform.SetParent(_CustomViewController.transform, false);
                        go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                        go.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
                        _UniGifImage = go.AddComponent<UniGifImage>();
                        UniGifImageAspectController ugiac = go.AddComponent<UniGifImageAspectController>();
                        _UniGifImage.SetPrivateField("m_imgAspectCtrl", ugiac);

                        StartCoroutine(_DisplayGifFromRotation());

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
            if (_CustomMenu != null && _CustomViewController != null)
            {
                _CurrentPanelMode = mode;
                StartCoroutine(_DisplayGifFromRotation());
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

        private IEnumerator _DisplayGifFromRotation()
        {
            if (_UniGifImage != null)
            {
                Coroutine gifCoroutine = null;
                _RawImage.enabled = false;
                try
                {
                    gifCoroutine = StartCoroutine(_UniGifImage.SetGifFromUrlCoroutine(_GifRotation[UnityEngine.Random.Range(0, _GifRotation.Length)]));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception catched on displaying gif: " + e);
                }
                yield return gifCoroutine;
                _RawImage.enabled = true;
            }
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
