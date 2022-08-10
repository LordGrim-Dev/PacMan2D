using System;
using System.Collections.Generic;
using Game.Common;
using PacMan.config;
using PacMan.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static Game.Common.PacManGameData;

namespace PacMan
{
    public class PMGameManager : MonoBehaviour
    {
        GameState m_CurrentGameState;

        private int m_CurrentLevel;

        private int m_MaxLevel;

        private List<Transform> m_TempLevelObjectHolder;

        public GameState CurrentGameState { get => m_CurrentGameState; }

        private static PMGameManager s_Instance;

        public static PMGameManager Instance => s_Instance;

        private void OnEnable()
        {
            GameEventManager.Instance.OnLevelCompleted += OnLevelCompleted;

            GameEventManager.Instance.OnGameOver += GameOver;
        }

        

        private void OnDisable()
        {
            GameEventManager.Instance.OnLevelCompleted -= OnLevelCompleted;
            GameEventManager.Instance.OnGameOver -= GameOver;
        }
        
        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                Destroy(gameObject);

            m_TempLevelObjectHolder = new List<Transform>();

        }
        

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                ChangeGameState(GameState.eGamePaused);
            }
            else
            {
                if (m_CurrentGameState != GameState.eGameOver && !(PMPacManManager.Instance().TotalPacManLivesLeft < 0))
                {
                    ChangeGameState(GameState.eResume);
                }
            }

        }
        

        internal void InitialiseAndShowIntro()
        {
            m_CurrentLevel = 1;

            var gameData = Resources.Load<PacManGameData>(PMConstants.K_LEVEL_DATA_PATH);

            var configuration = PMConfigManager.Instance().GameSetting;

            LoadLevelData(m_CurrentLevel, gameData);

            m_MaxLevel = PMConfigManager.Instance().GameSetting.Levels.Count + 1; //To make sure level withing max+1

            PMPacManManager.Instance().OnConfigurationUpdate(configuration.MaxLives);

            PMFireBallPoolManager.Instance().OnResourceUpdate(gameData.PacMan.FireBallPrefab);

            GameEventManager.Instance.TriggerPause(true);

            ChangeGameState(GameState.eGamePaused);

            PMUIManager.Instance().ShowIntroUI();
        }
        

        private void LoadLevelGridAndpallet(PacManGameData inGameData, int inIndex)
        {
            PMGameSceneReferanceHolder scenRef = PMGameSceneReferanceHolder.Instance;
            if (inGameData != null)
            {
                var leveldata = inGameData.LevelMapper[inIndex];
                if (leveldata != null)
                {
                    var levelPallets = leveldata.LevelPalletPrefab;
                    var pallets = GameUtilities.InstantiateObjectOfType<GameObject>(levelPallets, Vector3.zero, scenRef.LeveSceneRef.PalletsParent);

                    var levelGhostNodePoint = leveldata.LevelGhostNodePointsPrefab;
                    var ghostNodes = GameUtilities.InstantiateObjectOfType<GameObject>(levelGhostNodePoint, Vector3.zero, scenRef.LeveSceneRef.GhostMovementPointParent);

                    var levelGrid = leveldata.LevelGridParentPrefab;
                    var wallHolder = GameUtilities.InstantiateObjectOfType<GameObject>(levelGrid, Vector3.zero, scenRef.LeveSceneRef.WallParent);

                    m_TempLevelObjectHolder.Add(pallets.transform);
                    m_TempLevelObjectHolder.Add(ghostNodes.transform);
                    m_TempLevelObjectHolder.Add(wallHolder.transform);
                }
            }
        }
        

        private void LoadLevelData(int inCurrentLevel, PacManGameData inLevelGameData)
        {
            ChangeGameState(GameState.eLoading);

            if (inLevelGameData != null)
            {
                var configuration = PMConfigManager.Instance().GetLevelData(inCurrentLevel);

                int index = inCurrentLevel - 1;

                var leveldata = inLevelGameData.LevelMapper[index];

                PMGameSceneReferanceHolder.Instance.UpdateValues(leveldata.GhostHomeInsidePosition, leveldata.GhostHomeOutsidePos);

                LoadLevelGridAndpallet(inLevelGameData, index);

                PMScoreManager.Instance().OnLevelDataUpdate(configuration.ScoreDetails);

                PMGhostsManager.Instance().OnLevelDataUpdate(configuration.GhostData, inLevelGameData);

                PMPacManManager.Instance().OnLevelDataUpdate(inLevelGameData.PacMan.PacManPrefab, leveldata.StartPacManPosition);

                PMConsumableManager.Instance().OnLevelDataUpdate(configuration.BonuseFruiteCount, inLevelGameData.PacMan.BonusFruitPrefab);

                PMUIManager.Instance().OnLevelDataUpdate(configuration.PowerUPTime, configuration.PowerUpCoolDown);

            }
            else
            {
#if DEBUG
                GameUtilities.ShowLog(" NULL Scriptable GameData");

#endif
            }

        }
        

        private void GameOver()
        {
            ChangeGameState(GameState.eGameOver);
            PMUIManager.Instance().ShowGameOverUI();
        }
        

        private void OnLevelCompleted()
        {
            if (m_CurrentGameState == GameState.eGameOver || PMPacManManager.Instance().TotalPacManLivesLeft <= 0)
                return;

            CleanUpCurrentLevel();

            m_CurrentLevel = (m_CurrentLevel + 1) % (m_MaxLevel);

            if (m_CurrentLevel <= 0 || m_CurrentLevel >= m_MaxLevel)
                m_CurrentLevel = 1;

            var gameData = Resources.Load<PacManGameData>(PMConstants.K_LEVEL_DATA_PATH);

            LoadLevelData(m_CurrentLevel, gameData);

            PMUIManager.Instance().ShowCountDownUI();
            //  CoroutineManager.Instance.WaitForSecondsForAction(3, GameEventManager.Instance.TriggerNewRound);

        }
        
        private void CleanUpCurrentLevel()
        {
            foreach (var transform in m_TempLevelObjectHolder)
            {
                Destroy(transform.gameObject);
            }
            m_TempLevelObjectHolder.Clear();
        }

        

        private void ChangeGameState(GameState inState)
        {
            m_CurrentGameState = inState;
        }
        

        internal void StartGame()
        {
            GameEventManager.Instance.TriggerNewRound();

            ChangeGameState(GameState.eResume);

            GameEventManager.Instance.TriggerPause(false);
        }
        


        internal void OnExitGame()
        {
            PMPacManManager.Instance().CleanUP();
            PMGhostsManager.Instance().CleanUP();
            PMConfigManager.Instance().CleanUP();
            PMConsumableManager.Instance().CleanUP();
#if !UNITY_EDITOR
            Application.Quit();
#else
            EditorApplication.isPlaying = false;
#endif
        }
        


    }

}