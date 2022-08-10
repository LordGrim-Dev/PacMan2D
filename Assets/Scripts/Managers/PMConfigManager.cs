using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using Game.Common;
using System;

namespace PacMan.config
{
    public class PMConfigManager : SingletonObserverBase
    {
        //SingleTon
        private PMConfigManager() { }
        private static PMConfigManager sConfigManagerInstance = null;
        public static PMConfigManager Instance()
        {
            if (sConfigManagerInstance == null)
                sConfigManagerInstance = new PMConfigManager();
            return sConfigManagerInstance;

        }
        
        private GameSetting m_GameSetting;
        private LocalisationDetailes m_ConfigLocalisation;

        public GameSetting GameSetting { get => m_GameSetting; }
        public LocalisationDetailes ConfigLocalisation { get => m_ConfigLocalisation; }

        
        public void LoadConfig()
        {
            string path = String.Concat("ConfigData/" + PMConstants.CONFIG_FILE_NAME);

            string jsonString = Resources.Load(path).ToString();

            if (jsonString != null)
            {

                ConfigData data = JsonConvert.DeserializeObject<ConfigData>(jsonString);

                m_GameSetting = data.GameSetting;
                m_ConfigLocalisation = data.LocalisationDetails;
            }
            else
            {

#if DEBUG
                GameUtilities.ShowLog("jsonString is NULL!");
# endif
            }

#if DEBUG
            GameUtilities.ShowLog("Json--> " + jsonString);
# endif
        }
        

        public string GetLocalisedStringForKey(string inKey, bool isReverie = false)
        {
            string localisedString;

            if (m_ConfigLocalisation.LocalisationDetails_en.ContainsKey(inKey))
            {
                localisedString = m_ConfigLocalisation.LocalisationDetails_en[inKey];
            }
            else
            {
#if DEBUG
                GameUtilities.ShowLog("Key Not Found--> " + inKey);
#endif
                localisedString = inKey;
            }

            return localisedString;

        }
        

        public LevelDetails GetLevelData(int inLevel)
        {
            LevelDetails returnDetails = null;

            m_GameSetting.Levels.TryGetValue(inLevel, out returnDetails);

            return returnDetails;

        }
        
        public string GetLocalisedStringForKey(string inKey)
        {
            string localisedString = inKey.ToUpper();
            m_ConfigLocalisation.LocalisationDetails_en.TryGetValue(inKey, out localisedString);

            return localisedString;
        }

        
        public override void OnDestroy()
        {
            sConfigManagerInstance = null;
            m_GameSetting = null;
            m_ConfigLocalisation = null; ;
        }
        


        internal void CleanUP()
        {
            m_ConfigLocalisation = null;
            m_GameSetting.Levels.Clear();
            m_GameSetting = null;
        }
        

    }
}