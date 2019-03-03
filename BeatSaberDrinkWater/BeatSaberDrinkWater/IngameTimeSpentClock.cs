using BeatSaberDrinkWater.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeatSaberDrinkWater
{
    public class IngameTimeSpentClock : MonoBehaviour
    {
        public static IngameTimeSpentClock Instance;
        
        private TimeSpan _TimeSpent;
        private Coroutine _CUpdateIngameTimeSpentClock;

        public static void OnLoad()
        {
            if (Instance != null) return;
            //Plugin.Log("Creating IngameTimeSpentClock.", Plugin.LogLevel.DebugOnly);
            //new GameObject("IngameTimeSpentClock").AddComponent<IngameTimeSpentClock>().transform.parent = parent;
            new GameObject("IngameTimeSpentClock").AddComponent<IngameTimeSpentClock>();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                //Plugin.Log("IngameTimeSpentClock awake.", Plugin.LogLevel.DebugOnly);
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
                _TimeSpent = new TimeSpan(0);
            }
            else
                Destroy(this);
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (SceneUtils.IsGameScene(scene))
            {
                if (_CUpdateIngameTimeSpentClock != null)
                    StopCoroutine(_CUpdateIngameTimeSpentClock);
                _CUpdateIngameTimeSpentClock = StartCoroutine(UpdateIngameTimeSpentClock());
            }
            else if (SceneUtils.IsMenuScene(scene))
            {
                if (_CUpdateIngameTimeSpentClock != null)
                {
                    StopCoroutine(_CUpdateIngameTimeSpentClock);
                    _CUpdateIngameTimeSpentClock = null;
                }
            }
        }

        public IEnumerator UpdateIngameTimeSpentClock()
        {
            //Plugin.Log("IngameTimeSpentClock UpdateIngameTimeSpentClock function called.", Plugin.LogLevel.DebugOnly);
            while (_TimeSpent != null)
            {
                _TimeSpent = _TimeSpent.Add(new TimeSpan(0, 0, 1));
                Console.WriteLine("Current IngameTimeSpent is: " + _TimeSpent);
                
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
