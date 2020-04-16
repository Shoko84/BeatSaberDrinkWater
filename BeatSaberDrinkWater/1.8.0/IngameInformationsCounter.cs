using System;
using BS_Utils.Utilities;
using DrinkWater.Models;
using UnityEngine;

namespace DrinkWater
{
    public class IngameInformationsCounter : MonoBehaviour
    {
        public static IngameInformationsCounter Instance;
        
        public TimeSpan IngameTimeSpent { get; private set; }
        public int CurrentPlaycount { get; private set; }
        public SceneState CurrentSceneState { get; private set; } = SceneState.Menu;

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
                BSEvents.menuSceneActive += OnMenuSceneActive;
                BSEvents.gameSceneActive += OnGameSceneActive;
                DontDestroyOnLoad(gameObject);
                CurrentPlaycount = 0;
                IngameTimeSpent = new TimeSpan(0);
            }
            else
                Destroy(this);
        }

        private void OnDestroy()
        {
            BSEvents.menuSceneActive -= OnMenuSceneActive;
            BSEvents.gameSceneActive -= OnGameSceneActive;
        }

        private void Update()
        {
            if (CurrentSceneState == SceneState.Game)
                IngameTimeSpent = IngameTimeSpent.Add(TimeSpan.FromSeconds(Time.deltaTime));
        }

        public void OnMenuSceneActive()
        {
            if (CurrentSceneState == SceneState.Menu) return;
            CurrentSceneState = SceneState.Menu;
        }

        public void OnGameSceneActive()
        {
            if (CurrentSceneState == SceneState.Game) return;
            CurrentSceneState = SceneState.Game;
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
