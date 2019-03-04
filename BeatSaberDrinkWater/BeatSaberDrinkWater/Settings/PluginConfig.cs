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
        }

        public static void SaveConfig()
        {
            ModPrefs.SetBool("BeatSaberDrinkWater", "EnablePlugin", EnablePlugin);
        }
    }
}