using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using PacMan.UI;
using UnityEngine;

namespace PacMan
{
    public class PMPacManManager : SingletonObserverBase
    {
        private bool m_IsInitialised = false;

        PMPacMan m_PacMan;
        private int m_TotalPacManLives;
        private PMPacManManager() { Initialise(); }
        private static PMPacManManager s_Instance;


        public static PMPacManManager Instance()
        {
            if (s_Instance == null)
                s_Instance = new PMPacManManager();
            return s_Instance;

        }

        public PacManState CurrentState
        {
            get => m_PacMan.PacManState;
        }

        public int GetPackManInstanceID
        {
            get => m_PacMan.gameObject.GetInstanceID();
        }

        public Transform PacManTransform
        {
            get => m_PacMan.transform;
        }

        public int TotalPacManLivesLeft { get => m_TotalPacManLives; }

        
        private void Initialise()
        {
            if (!m_IsInitialised)
            {

                m_IsInitialised = true;

                GameEventManager.Instance.OnNewRoundStart -= ResetPackMan;
                GameEventManager.Instance.OnNewRoundStart += ResetPackMan;

                GameEventManager.Instance.OnPacManPowerUpActivated -= OnPowerUp;
                GameEventManager.Instance.OnPacManPowerUpActivated += OnPowerUp;

                GameEventManager.Instance.OnLevelCompleted -= OnLevelCompleted;
                GameEventManager.Instance.OnLevelCompleted += OnLevelCompleted;

                GameEventManager.Instance.OnGameOver -= OnGameOver;
                GameEventManager.Instance.OnGameOver += OnGameOver;

                GameEventManager.Instance.OnGamePause -= OnPause;
                GameEventManager.Instance.OnGamePause += OnPause;

                GameEventManager.Instance.OnPacManEaten -= OnPacManDeath;
                GameEventManager.Instance.OnPacManEaten += OnPacManDeath;
            }
        }

        
        private void OnPowerUp(bool inStatus)
        {
            m_PacMan.EnableLaneGhostDetector(inStatus);
        }

        
        private void OnPause(bool inPasueStatus)
        {
            m_PacMan?.Pause(inPasueStatus);
        }

        
        internal void OnConfigurationUpdate(int inMaxLives)
        {
            if (inMaxLives < 0)
                inMaxLives = 3;
            m_TotalPacManLives = inMaxLives;

            UI.UIEventManager.Instance.TriggerPlayerLivesUpdate(m_TotalPacManLives);
        }
        

        private void OnGameOver()
        {
            m_PacMan.gameObject.SetActive(false);

        }
        

        public void OnPacManDeath()
        {
            m_TotalPacManLives--;

            UI.UIEventManager.Instance.TriggerPlayerLivesUpdate(m_TotalPacManLives);

            m_PacMan.OnPacManDeath();

            if (m_TotalPacManLives <= 0)
            {
                GameEventManager.Instance.TriggerGameOver();
            }
            else
            {
                int delay = 3;
                CoroutineManager.Instance.WaitForSecondsForAction(delay, ResetPackMan);
            }
        }
        

        internal void OnLevelDataUpdate(PMPacMan inPrefab, Vector2 inInitPosition)
        {
            if (m_PacMan == null)
                m_PacMan = GameUtilities.InstantiateObjectOfType<PMPacMan>(inPrefab, inInitPosition, PMGameSceneReferanceHolder.Instance.transform);
            else
                m_PacMan.transform.position = inInitPosition;

            m_PacMan.gameObject.SetActive(true);
        }

        

        private void OnLevelCompleted()
        {
            m_PacMan.gameObject.SetActive(false);
        }

        
        public void ResetPackMan()
        {
            m_PacMan.ResetPacMan();
        }
        

        public override void OnDestroy()
        {
            s_Instance = null;
        }
        

        internal void CleanUP()
        {
            m_PacMan = null;
            m_TotalPacManLives = 0;
        }
        

        internal void ShootFireball()
        {
            PMFireBall fireBall = PMFireBallPoolManager.Instance().GetAvailableBullets(m_PacMan.transform.position);
            Utility.Vibration.Vibrate(20);
            fireBall.Show(m_PacMan.CurrentDirection);
        }

        
        internal bool IsFireBall(int inID)
        {
            PMFireBall fireball = PMFireBallPoolManager.Instance().GetFireBallByID(inID);

            return fireball != null;
        }
    }
}