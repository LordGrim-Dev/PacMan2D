using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan.UI
{
    public class UIEventManager : SingletonObserverBase
    {
        private UIEventManager() { }
        private static UIEventManager s_Instance;
        public static UIEventManager Instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new UIEventManager();
                return s_Instance;
            }
        }
        

        public event Action<int> OnPlayerLivesUpdate;
        public event Action<int> OnPlayerScoreUpdate;

        public void TriggerPlayerLivesUpdate(int inLivesLeft)
        {
            OnPlayerLivesUpdate?.Invoke(inLivesLeft);
        }
        

        public void TriggerPlayerScoreUpdate(int inScore)
        {
            OnPlayerScoreUpdate?.Invoke(inScore);
        }
        
        public override void OnDestroy()
        {
            s_Instance = null;
        }
        

    }
}
