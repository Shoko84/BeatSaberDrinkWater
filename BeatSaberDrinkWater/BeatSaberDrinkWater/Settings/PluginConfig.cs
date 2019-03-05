using IllusionPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BeatSaberDrinkWater.Settings
{
    public class PluginConfig
    {
        public static bool EnablePlugin = true;
        public static bool ShowGIFs = true;
        public static int WaitDuration = 5;
        public static bool EnableByPlaytime = true;
        public static bool EnableByPlaycount = false;
        public static int PlaytimeBeforeWarning = 5;
        public static int PlaycountBeforeWarning = 2;

        public static void LoadOrCreateConfig()
        {
            if (!Directory.Exists("UserData"))
                Directory.CreateDirectory("UserData");

            //EnablePlugin property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "EnablePlugin"))
            {
                ModPrefs.SetBool("BeatSaberDrinkWater", "EnablePlugin", true);
                Console.WriteLine("Created config");
            }
            else
                EnablePlugin = ModPrefs.GetBool("BeatSaberDrinkWater", "EnablePlugin", true, true);

            //ShowGIFs property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "ShowGIFs"))
            {
                ModPrefs.SetBool("BeatSaberDrinkWater", "ShowGIFs", true);
                Console.WriteLine("Created config");
            }
            else
                EnablePlugin = ModPrefs.GetBool("BeatSaberDrinkWater", "ShowGIFs", true, true);

            //WaitDuration property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "WaitDuration"))
            {
                ModPrefs.SetInt("BeatSaberDrinkWater", "WaitDuration", 5);
                Console.WriteLine("Created config");
            }
            else
                WaitDuration = ModPrefs.GetInt("BeatSaberDrinkWater", "WaitDuration", 5, true);

            //EnableByPlaytime property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "EnableByPlaytime"))
            {
                ModPrefs.SetBool("BeatSaberDrinkWater", "EnableByPlaytime", true);
                Console.WriteLine("Created config");
            }
            else
                EnableByPlaytime = ModPrefs.GetBool("BeatSaberDrinkWater", "EnableByPlaytime", true, true);

            //EnableByPlaycount property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "EnableByPlaycount"))
            {
                ModPrefs.SetBool("BeatSaberDrinkWater", "EnableByPlaycount", false);
                Console.WriteLine("Created config");
            }
            else
                EnableByPlaycount = ModPrefs.GetBool("BeatSaberDrinkWater", "EnableByPlaycount", false, true);

            //PlaytimeBeforeWarning property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "PlaytimeBeforeWarning"))
            {
                ModPrefs.SetInt("BeatSaberDrinkWater", "PlaytimeBeforeWarning", 5);
                Console.WriteLine("Created config");
            }
            else
                PlaytimeBeforeWarning = ModPrefs.GetInt("BeatSaberDrinkWater", "PlaytimeBeforeWarning", 5, true);

            //PlaycountBeforeWarning property
            if (!ModPrefs.HasKey("BeatSaberDrinkWater", "PlaycountBeforeWarning"))
            {
                ModPrefs.SetInt("BeatSaberDrinkWater", "PlaycountBeforeWarning", 2);
                Console.WriteLine("Created config");
            }
            else
                PlaycountBeforeWarning = ModPrefs.GetInt("BeatSaberDrinkWater", "PlaycountBeforeWarning", 2, true);
        }

        public static void SaveConfig()
        {
            ModPrefs.SetBool("BeatSaberDrinkWater", "EnablePlugin", EnablePlugin);
            ModPrefs.SetBool("BeatSaberDrinkWater", "ShowGIFs", ShowGIFs);
            ModPrefs.SetInt("BeatSaberDrinkWater", "WaitDuration", WaitDuration);
            ModPrefs.SetBool("BeatSaberDrinkWater", "EnableByPlaytime", EnableByPlaytime);
            ModPrefs.SetBool("BeatSaberDrinkWater", "EnableByPlaycount", EnableByPlaycount);
            ModPrefs.SetInt("BeatSaberDrinkWater", "PlaytimeBeforeWarning", PlaytimeBeforeWarning);
            ModPrefs.SetInt("BeatSaberDrinkWater", "PlaycountBeforeWarning", PlaycountBeforeWarning);
        }
    }
}