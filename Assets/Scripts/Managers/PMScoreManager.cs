
using System;
using Game.Common;

namespace PacMan
{
    public class PMScoreManager : SingletonObserverBase
    {
        private PMScoreManager() { Initialise(); }
        private static PMScoreManager s_Instance;
        public static PMScoreManager Instance()
        {
            if (s_Instance == null)
                s_Instance = new PMScoreManager();
            return s_Instance;

        }
        

        private int m_TotalScore;

        private bool m_IsInitialised = false;
        private void Initialise()
        {
            if (!m_IsInitialised)
            {
                m_IsInitialised = true;

                GameEventManager.Instance.OnItemConsumed -= OnItemCollected;
                GameEventManager.Instance.OnItemConsumed += OnItemCollected;

                GameEventManager.Instance.OnGhostEaten -= UpdateGhostEatScore;
                GameEventManager.Instance.OnGhostEaten += UpdateGhostEatScore;

                m_TotalScore = 0;
            }
        }
        

        private config.ScoreData m_CurrentLevelScoreData;

        public int TotalScore { get => m_TotalScore; }

        
        public void OnLevelDataUpdate(config.ScoreData inScoreDat)
        {
            m_CurrentLevelScoreData = inScoreDat;
        }

        
        private void OnItemCollected(ItemType inItemType)
        {
            int score = 0;
            switch (inItemType)
            {
                case ItemType.eFruite:
                    score = m_CurrentLevelScoreData.BonusFruitCollectPoint;
                    break;

                case ItemType.ePallet:
                    score = m_CurrentLevelScoreData.PalletEatPoint;
                    break;

                case ItemType.ePowerPallet:
                    score = m_CurrentLevelScoreData.PalletEatPoint;
                    break;
            }

            OnScoreUpdate(score);
        }
        

        private void UpdateGhostEatScore()
        {
            int score = 0;
            if (m_CurrentLevelScoreData != null)
                score = m_CurrentLevelScoreData.GhostKillPoint;
            UpdateTotalScore(score);
        }

        

        public void OnScoreUpdate(int inScore)
        {
            if (inScore < 0) return;

            int updatedScore = m_TotalScore + inScore;

            UpdateTotalScore(updatedScore);
        }
        

        private void UpdateTotalScore(int inTotalUpdatedScore)
        {
            if (inTotalUpdatedScore > m_TotalScore)
                m_TotalScore = inTotalUpdatedScore;

            UI.UIEventManager.Instance.TriggerPlayerScoreUpdate(m_TotalScore);
        }
        

        public override void OnDestroy()
        {
            s_Instance = null;
            m_TotalScore = 0;
        }
        
    }
}