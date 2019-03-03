using BeatSaberDrinkWater.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeatSaberDrinkWater
{
    public class TimeSpentClock : MonoBehaviour
    {
        public static TimeSpentClock Instance;

        private DateTime _StartTime;
        private TimeSpan _TimeSpent;
        private Coroutine _CUpdateTimeSpentClock;

        public static void OnLoad()
        {
            if (Instance != null) return;
            //Plugin.Log("Creating TimeSpentClock.", Plugin.LogLevel.DebugOnly);
            //new GameObject("TimeSpentClock").AddComponent<TimeSpentClock>().transform.parent = parent;
            new GameObject("TimeSpentClock").AddComponent<TimeSpentClock>();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                //Plugin.Log("TimeSpentClock awake.", Plugin.LogLevel.DebugOnly);
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
                _StartTime = DateTime.Now;
            }
            else
                Destroy(this);
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            //Plugin.Log("TimeSpentClock SceneManagerOnActiveSceneChanged: " + arg0.name + " " + scene.name, Plugin.LogLevel.DebugOnly);
            if (SceneUtils.IsMenuScene(scene))
            {
                if (_CUpdateTimeSpentClock == null)
                    _CUpdateTimeSpentClock = StartCoroutine(UpdateTimeSpentClock());
            }
        }

        public IEnumerator UpdateTimeSpentClock()
        {
            //Plugin.Log("TimeSpentClock UpdateTimeSpentClock function called.", Plugin.LogLevel.DebugOnly);
            while (_TimeSpent != null)
            {
                _TimeSpent = DateTime.Now - _StartTime;
                Console.WriteLine("Current TimeSpent is: " + _TimeSpent);

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
