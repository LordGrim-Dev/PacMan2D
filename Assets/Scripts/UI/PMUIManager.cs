using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan.UI
{
    public class PMUIManager : SingletonObserverBase
    {
        //SingleTon
        private PMUIManager() { Initialise(); }

        private static PMUIManager sInstance = null;
        public static PMUIManager Instance()
        {
            if (sInstance == null)
                sInstance = new PMUIManager();
            return sInstance;

        }

        private int m_PowerUPTime, m_PowerUpCoolDown;

        public int PowerUPTime { get => m_PowerUPTime; }
        public int PowerUpCoolDown { get => m_PowerUpCoolDown; }

        

        private void Initialise()
        {

        }
        

        internal void OnCounterZero()
        {
            var ui = UserInterfaceSystem.Instance.LoadUI<GamePlayUI>((int)UserInterface.eGamePlayUI);

            UserInterfaceSystem.Instance.HideUI((uint)UserInterface.eCounterUI);

            UserInterfaceSystem.Instance.ShowUi(ui);

            PMGameManager.Instance.StartGame();
        }

        

        public void OnClickExitGame()
        {
            UserInterfaceSystem.Instance.CleanUP();
            PMGameManager.Instance.OnExitGame();
        }
        

        public void OnClickContinue()
        {
            UserInterfaceSystem.Instance.HideUI((uint)UserInterface.eIntroUI);
            ShowCountDownUI();
        }
        

        public override void OnDestroy()
        {
            sInstance = null;
        }

        

        internal void ShowIntroUI()
        {
            var ui = UserInterfaceSystem.Instance.LoadUI<IntroUI>((int)UserInterface.eIntroUI);

            UserInterfaceSystem.Instance.ShowUi(ui);

        }
        

        internal void ShowGameOverUI()
        {
            UserInterfaceSystem.Instance.HideUI((uint)UserInterface.eGamePlayUI);

            var ui = UserInterfaceSystem.Instance.LoadUI<GameOverUI>((int)UserInterface.eGameOverUI);

            UserInterfaceSystem.Instance.ShowUi(ui);
        }
        

        internal void OnLevelDataUpdate(int powerUPTime, int powerUpCoolDown)
        {
            m_PowerUPTime = powerUPTime;
            m_PowerUpCoolDown = powerUpCoolDown;
        }
        


        internal void ShowCountDownUI()
        {
            CounterUI ui = UserInterfaceSystem.Instance.LoadUI<CounterUI>((uint)UserInterface.eCounterUI);

            UserInterfaceSystem.Instance.ShowUi(ui);

        }
        

    }
}