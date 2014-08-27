/*
 * Default Throttle
 * Created by Xaiier
 * License: MIT
 */

using System;
using UnityEngine;
using KSP.IO;

namespace DefaultThrottle
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class DefaultThrottle : MonoBehaviour
    {
        ConfigNode config;
        private string path = KSPUtil.ApplicationRootPath + "GameData/DefaultThrottle/config.txt";
        private int throttleSetting = 50;

        private void Start()
        {
            config = ConfigNode.Load(path);

            if (config == null)
            {
                config = new ConfigNode();
                config.AddValue("defaultThrottle", throttleSetting);
                config.Save(path);
            }
            else
            {
                try
                {
                    throttleSetting = Mathf.Clamp(int.Parse(config.GetValue("defaultThrottle")), 0, 100);
                }
                catch
                {
                    print("DefaultThrottle: invalid throttle setting");
                    config = new ConfigNode();
                    config.AddValue("defaultThrottle", throttleSetting);
                    config.Save(path);
                }
            }
        }
        private void Awake()
        {
            GameEvents.onVesselGoOffRails.Add(setThrottle);
        }
        private void setThrottle(Vessel v)
        {
            if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH)
            {
                FlightInputHandler.state.mainThrottle = (float)throttleSetting/100;
            }
        }
    }
}
