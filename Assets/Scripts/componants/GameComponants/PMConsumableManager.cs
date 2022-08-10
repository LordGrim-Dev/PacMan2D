
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class PMConsumableManager : SingletonObserverBase
    {

        private PMConsumableManager() { Initialise(); }
        private static PMConsumableManager s_Instance;
        public static PMConsumableManager Instance()
        {

            if (s_Instance == null)
                s_Instance = new PMConsumableManager();
            return s_Instance;
        }

        private int m_ItemCollectedCount;

        private List<PMConsumable> m_Consumable;

        private int m_RemainingFruitesTobeUsedInLevel;

        //For Now Only one active fruit will be there,
        private PMBonusFruit m_BonusFruit;

        private Coroutine m_FruiteDraw;

        WaitForSeconds m_WaitForTenSecod;
        private bool m_IsInitialised = false;

        private void Initialise()
        {
            if (!m_IsInitialised)
            {

                m_IsInitialised = true;
                m_WaitForTenSecod = new WaitForSeconds(10);

                m_ItemCollectedCount = 0;
                m_RemainingFruitesTobeUsedInLevel = 0;

                m_Consumable = new List<PMConsumable>();

                GameEventManager.Instance.OnItemConsumed += OnItemConsumed;

                GameEventManager.Instance.OnNewRoundStart += OnNewRound;

                GameEventManager.Instance.OnLevelCompleted += Reset;

                GameEventManager.Instance.OnGamePause += OnPauseStatus;

                Reset();
            }
        }

        private void OnPauseStatus(bool inStatus)
        {
            if (inStatus)
                CoroutineManager.Instance?.StopMyCoroutine(m_FruiteDraw);
            else
                m_FruiteDraw = CoroutineManager.Instance?.StartCoroutine(WaitAndSpawnNextBonusFruit());
        }

        

        private void PopulateTotalConsumableCountForLevel()
        {
            Transform palletsParent = PMGameSceneReferanceHolder.Instance.LeveSceneRef.PalletsParent;

            Transform holder = palletsParent.GetChild(0);

            foreach (Transform child in holder.transform)
            {
                PMConsumable pallet = child.GetComponent<PMConsumable>();
                if (!m_Consumable.Contains(pallet))
                    m_Consumable.Add(pallet);
            }
        }
        

        private void OnItemConsumed(ItemType inItemType)
        {
            if (inItemType == ItemType.ePallet || inItemType == ItemType.ePowerPallet)
            {
                m_ItemCollectedCount++;
                if (inItemType == ItemType.ePowerPallet)
                    Utility.Vibration.Vibrate(15);
                else
                    Utility.Vibration.Vibrate(10);
            }


            if (inItemType == ItemType.eFruite)
            {
                m_RemainingFruitesTobeUsedInLevel--;
                m_BonusFruit.gameObject.SetActive(false);
            }


            if (m_ItemCollectedCount == m_Consumable.Count)
                GameEventManager.Instance.TriggerLevelCompleted();

#if DEBUG
            GameUtilities.ShowLog("Count :" + m_ItemCollectedCount + " Level comp: " + m_Consumable.Count);

#endif
        }
        


        private void OnNewRound()
        {
            if (m_FruiteDraw != null)
                CoroutineManager.Instance.StopMyCoroutine(m_FruiteDraw);

            m_FruiteDraw = CoroutineManager.Instance.StartCoroutine(WaitAndSpawnNextBonusFruit());

            Transform palletParent = PMGameSceneReferanceHolder.Instance.LeveSceneRef.PalletsParent;

            Transform palletsHolder = palletParent.GetChild(0);

            foreach (Transform pellet in palletsHolder)
            {
                pellet.gameObject.SetActive(true);
            }

            PopulateTotalConsumableCountForLevel();
        }
        
        public void OnLevelDataUpdate(int inNumberofFruitsForTheLevel, PMBonusFruit inBonusFriitPrefab)
        {
            int fruitesToBeused = inNumberofFruitsForTheLevel % PMConstants.MAX_FRUIT_COUNT;

            m_RemainingFruitesTobeUsedInLevel = fruitesToBeused;

            if (m_BonusFruit == null)
            {
                m_BonusFruit = GameObject.Instantiate<PMBonusFruit>(inBonusFriitPrefab, Vector3.zero, Quaternion.identity, PMGameSceneReferanceHolder.Instance.gameObject.transform);
                m_BonusFruit.gameObject.SetActive(false);
            }

            // CheckAndSpawnFruitAtRandomPlace();
        }

        

        private void CheckAndSpawnFruitAtRandomPlace()
        {
            // Fruit Spawn Logic
            PacManState state = PMPacManManager.Instance().CurrentState;

            GameState currentGameState = PMGameManager.Instance.CurrentGameState;

            if (state == PacManState.eRespawning || state == PacManState.eDied)
                return;

            Vector2 position = PMGhostsManager.Instance().GetRandomeNodePosition();

            if (!Vector2.Equals(position, Vector2.zero))
            {
                m_BonusFruit.transform.localPosition = position;
                m_BonusFruit.gameObject.SetActive(true);

            }


        }
        

        void Reset()
        {
            m_ItemCollectedCount = 0;
            m_Consumable.Clear();
        }
        

        private IEnumerator WaitAndSpawnNextBonusFruit()
        {
            while (true)
            {
                yield return m_WaitForTenSecod;
                if (PMGameManager.Instance.CurrentGameState == GameState.eResume)
                    CheckAndSpawnFruitAtRandomPlace();

                if (PMGameManager.Instance.CurrentGameState == GameState.eGameOver || m_RemainingFruitesTobeUsedInLevel <= 0)
                    yield break;
            }
        }

        public override void OnDestroy()
        {
            s_Instance = null;
        }

        internal void CleanUP()
        {
            m_RemainingFruitesTobeUsedInLevel = 0;
            m_WaitForTenSecod = null;
            CoroutineManager.Instance.StopCoroutine(m_FruiteDraw);
        }
        
    }

}