using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class PMGhostsManager : SingletonObserverBase
    {

        private PMGhostsManager() { Initialise(); }
        private static PMGhostsManager s_Instance;
        public static PMGhostsManager Instance()
        {

            if (s_Instance == null)
                s_Instance = new PMGhostsManager();
            return s_Instance;
        }
        

        private List<PMGhostNodePoint> m_LevelGhostMomentPoints;
        private List<PMGhost> m_LevelGhostsRef;

        private bool m_IsInitialised = false;

        

        private void Initialise()
        {
            if (!m_IsInitialised)
            {

                m_IsInitialised = true;

                m_LevelGhostMomentPoints = new List<PMGhostNodePoint>();

                m_LevelGhostsRef = new List<PMGhost>();

                GameEventManager.Instance.OnGameOver += OnGameover;

                GameEventManager.Instance.OnGamePause += OnGamePause;

                GameEventManager.Instance.OnItemConsumed += OnPacManConsumedPallet;

                GameEventManager.Instance.OnPacManEaten += OnPacManDied;

                GameEventManager.Instance.OnLevelCompleted += OnLevelCompleted;

                GameEventManager.Instance.OnNewRoundStart += OnNewRoundStart;
            }
        }

        private void OnGamePause(bool inPasueStatus)
        {
            foreach (var ghost in m_LevelGhostsRef)
            {
                ghost.OnPauseStatus(inPasueStatus);
            }
        }

        

        private void OnNewRoundStart()
        {
            int totalLevelGhost = m_LevelGhostsRef.Count;
            for (int i = 0; i < totalLevelGhost; i++)
            {
                m_LevelGhostsRef[i].ResetState();
                m_LevelGhostsRef[i].gameObject.SetActive(true);
            }

            PopulateGostMomentNodePoints();
        }

        
        internal void OnLevelDataUpdate(config.LevelGhostData inGostData, PacManGameData inGridData)
        {
            int maxEnemyCountForTheLevel = inGostData.GhostCount % PMConstants.MAX_GHOST_COUNT;

            var prefabList = inGridData.AllGhosts.Ghosts;

            SpawnGhost(maxEnemyCountForTheLevel, inGostData, prefabList);

        }
        

        private void SpawnGhost(int inMaxEnemyCountForTheLevel, config.LevelGhostData inGostData, List<PMGhost> inGhosts)
        {
            // To do.,
            // Ghost which not used need to be disabled and removed from list
            // Create Pool and disable all Ghost

#if DEBUG
            GameUtilities.ShowLog($"SpawnGhost : {inMaxEnemyCountForTheLevel} : inGhosts : Count :{inGhosts.Count}");

#endif

            int existingGhostCount = m_LevelGhostsRef.Count;
            int newGhostYetCreat = Mathf.Abs(inMaxEnemyCountForTheLevel - existingGhostCount);

            Transform parent = PMGameSceneReferanceHolder.Instance.transform;

            Transform homeInside = PMGameSceneReferanceHolder.Instance.GhostRef.GhostHomeInside;

            if (newGhostYetCreat > 0)
            {
                for (int i = newGhostYetCreat - 1; i >= 0; i--)
                {
                    PMGhost newGhost = GameObject.Instantiate<PMGhost>(inGhosts[i], homeInside.position, Quaternion.identity, parent);
                    newGhost.Configure(inGostData.GhostMomentSpeed[i]);
                    m_LevelGhostsRef.Add(newGhost);
                    newGhost.gameObject.SetActive(false);
                }
            }
            else
            {
                int index = 0;
                int maxIndex = inGostData.GhostMomentSpeed.Length;
                foreach (PMGhost ghost in m_LevelGhostsRef)
                {
                    ghost.Configure(inGostData.GhostMomentSpeed[index]);
                    index = (index + 1) % maxIndex;

                }
            }
        }

        

        private void OnPacManConsumedPallet(ItemType inItemType)
        {
            if (inItemType != ItemType.ePowerPallet)
                return;

            int ghostCount = m_LevelGhostsRef.Count;
            for (int i = 0; i < ghostCount; i++)
            {
                m_LevelGhostsRef[i].FrightnedGhost.Enable();
            }

        }
        

        private void OnGameover()
        {
            int ghostCount = m_LevelGhostsRef.Count;
            for (int i = 0; i < ghostCount; i++)
            {
                m_LevelGhostsRef[i].gameObject.SetActive(false);
            }
        }
        

        private void PopulateGostMomentNodePoints()
        {
            Transform ghostNodeParent = PMGameSceneReferanceHolder.Instance.LeveSceneRef.GhostMovementPointParent;

            // All childrens of parent need to be populated
            Transform nodesHolder = ghostNodeParent.GetChild(0);

            foreach (Transform nodeChildren in nodesHolder)
            {
                PMGhostNodePoint child = nodeChildren.GetComponent<PMGhostNodePoint>();

                if (child != null)
                    m_LevelGhostMomentPoints.Add(child);
            }

#if DEBUG
            GameUtilities.ShowLog("PopulateGostMomentNodePoints TotalNode Points: " + m_LevelGhostMomentPoints.Count);

#endif
        }
        

        public void OnLevelCompleted()
        {
#if DEBUG
            GameUtilities.ShowLog("TotalNode Points: " + m_LevelGhostMomentPoints.Count);

#endif

            m_LevelGhostMomentPoints.Clear();

            int totalGhost = m_LevelGhostsRef.Count;

            for (int i = 0; i < totalGhost; i++)
            {
                m_LevelGhostsRef[i].gameObject.SetActive(false);
            }
        }
        

        private void OnPacManDied()
        {
            if (PMGameManager.Instance.CurrentGameState != GameState.eGameOver)
                CoroutineManager.Instance.WaitForSecondsForAction(3, ResteAllGhosts);
        }
        

        private void ResteAllGhosts()
        {
            int ghostsCount = m_LevelGhostsRef.Count;
            for (int i = 0; i < ghostsCount; i++)
            {
                m_LevelGhostsRef[i]?.ResetState();
            }
        }
        

        public Vector2 GetRandomeNodePosition()
        {
            int randomeIndex = UnityEngine.Random.Range(0, m_LevelGhostMomentPoints.Count);
#if DEBUG
            GameUtilities.ShowLog($"TotalNode Points: {m_LevelGhostMomentPoints.Count} :Index :{randomeIndex}");
#endif 
            return m_LevelGhostMomentPoints[randomeIndex].transform.position;
        }

        public bool IsIdBelongsToGhost(int inInstanceId)
        {
            bool yes = false;
            foreach (var ghost in m_LevelGhostsRef)
            {
                if (ghost.gameObject.GetInstanceID() == inInstanceId)
                {
                    yes = true;
                    break;
                }
            }

            return yes;
        }


        public override void OnDestroy()
        {
            s_Instance = null;
            m_LevelGhostMomentPoints = null;
        }

        internal void CleanUP()
        {
            m_LevelGhostMomentPoints?.Clear();
            m_LevelGhostMomentPoints = null;
            m_LevelGhostsRef.Clear();
            m_LevelGhostsRef = null;
        }
        

    }
}