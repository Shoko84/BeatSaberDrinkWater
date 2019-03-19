using BeatSaberDrinkWater.Utilities;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeatSaberDrinkWater
{
    public class IngameInformationsCounter : MonoBehaviour
    {
        public static IngameInformationsCounter Instance;
        
        public TimeSpan IngameTimeSpent;
        public int CurrentPlaycount;
        private Coroutine _CUpdateIngameTimeSpentClock;

        public static void OnLoad()
        {
            if (Instance != null) return;
            new GameObject("IngameTimeSpentClock").AddComponent<IngameInformationsCounter>();
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
                CurrentPlaycount = 0;
                IngameTimeSpent = new TimeSpan(0);
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
            while (IngameTimeSpent != null)
            {
                IngameTimeSpent = IngameTimeSpent.Add(new TimeSpan(0, 0, 1));
                yield return new WaitForSeconds(1f);
            }
        }

        public void PlayerHasFinishedMap()
        {
            CurrentPlaycount += 1;
        }

        public void ResetTimeSpent()
        {
            IngameTimeSpent = new TimeSpan(0);
        }

        public void ResetPlaycount()
        {
            CurrentPlaycount = 0;
        }
    }
}
