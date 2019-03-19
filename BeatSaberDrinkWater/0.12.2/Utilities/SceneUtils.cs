using System;
using UnityEngine.SceneManagement;

namespace BeatSaberDrinkWater.Utilities
{
    class SceneUtils
    {
        public static bool IsMenuScene(Scene scene)
        {
            return CheckSceneByName(scene, "Menu");
        }

        public static bool IsGameScene(Scene scene)
        {
            return CheckSceneByName(scene, "GameCore");
        }

        private static bool CheckSceneByName(Scene scene, String sceneName)
        {
            try
            {
                return (scene.name == sceneName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error getting " + sceneName + " scene:" + e);
            }
            return false;
        }
    }
}