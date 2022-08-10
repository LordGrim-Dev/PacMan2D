using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class GameEventManager : SingletonObserverBase
    {
        private GameEventManager() { }
        private static GameEventManager s_Instance;
        public static GameEventManager Instance
        {
            get
            {
                if (s_Instance == null)
                    s_Instance = new GameEventManager();
                return s_Instance;
            }
        }

        public event Action OnPacManEaten;
        public event Action OnGhostEaten;

        public event Action<bool> OnGamePause;

        public event Action OnGameOver;
        public event Action OnLevelCompleted;

        public event Action OnNewRoundStart;

        public event Action<bool> OnPacManPowerUpActivated;

        public event Action<ItemType> OnItemConsumed;

        
        public void TriggerNewRound()
        {
            OnNewRoundStart?.Invoke();
        }
        

        internal void TriggerPause(bool pauseStatus)
        {
            OnGamePause?.Invoke(pauseStatus);
        }

        

        public void TriggerPacManBeingKilled()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerPacManBeingKilled");
#endif
            OnPacManEaten?.Invoke();
        }
        

        public void TriggerGhostBeenKilled()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerGhostBeenKilled");
#endif
            OnGhostEaten?.Invoke();
        }
        

        public void TriggerItemConsumed(ItemType inType)
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerItemConsumed :" + inType);
#endif
            OnItemConsumed?.Invoke(inType);
        }
        

        internal void TriggerGameOver()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerGameOver");
#endif
            OnGameOver?.Invoke();
        }
        

        internal void TriggerLevelCompleted()
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerLevelCompleted");
#endif
            OnLevelCompleted?.Invoke();
        }
        

        internal void TriggerOnPowerUpActivated(bool inStatus)
        {
#if DEBUG
            GameUtilities.ShowLog("TriggerOnPowerUpActivated");
#endif
            OnPacManPowerUpActivated?.Invoke(inStatus);
        }
        

        public override void OnDestroy()
        {
#if DEBUG
            GameUtilities.ShowLog("OnDestroy");
#endif
            s_Instance = null;
        }
        
    }
}