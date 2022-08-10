using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using PacMan;
using System.Collections.Generic;

namespace Game.Common
{
    [CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/CreatLevelData", order = 1)]

    public class PacManGameData : ScriptableObject
    {
        [SerializeField]
        List<PacManLevelAssetData> m_LevelMapper = new List<PacManLevelAssetData>();

        [SerializeField]
        PacManAssetData m_PacMan = new PacManAssetData();

        [SerializeField]
        GhostsAssetData m_AllGhosts = new GhostsAssetData();

        public List<PacManLevelAssetData> LevelMapper { get => m_LevelMapper; }
        public PacManAssetData PacMan { get => m_PacMan; }
        public GhostsAssetData AllGhosts { get => m_AllGhosts; }

        [System.Serializable]
        public class PacManLevelAssetData
        {
            [SerializeField]
            private GameObject m_LevelPalletsParent;

            [SerializeField]
            private GameObject m_LevelGhostNodePointsHolder;

            [SerializeField]
            private GameObject m_LevelGridParent;

            [SerializeField]
            private Vector2 m_StartPacManPosition;

            [SerializeField]
            private Vector2 m_GhostHomeInsidePosition;

            [SerializeField]
            private Vector2 m_GhostHomeOutsidePos;

            

            public GameObject LevelGridParentPrefab { get => m_LevelGridParent; }
            public GameObject LevelGhostNodePointsPrefab { get => m_LevelGhostNodePointsHolder; }
            public GameObject LevelPalletPrefab { get => m_LevelPalletsParent; }
            public Vector2 GhostHomeInsidePosition { get => m_GhostHomeInsidePosition; }
            public Vector2 StartPacManPosition { get => m_StartPacManPosition; }
            public Vector2 GhostHomeOutsidePos { get => m_GhostHomeOutsidePos; }
            

        }


        [System.Serializable]
        public class PacManAssetData
        {
            [SerializeField]
            PMPacMan m_PacManPrefab;
            [SerializeField]
            PMFireBall m_FireBallPrefab;

            [SerializeField]
            PMBonusFruit m_BonusFruitPrefab;

            public PMPacMan PacManPrefab { get => m_PacManPrefab; }
            public PMFireBall FireBallPrefab { get => m_FireBallPrefab; }
            public PMBonusFruit BonusFruitPrefab { get => m_BonusFruitPrefab; }
        }
        



        [System.Serializable]
        public class GhostsAssetData
        {
            [SerializeField]
            List<PMGhost> m_Ghosts;

            public List<PMGhost> Ghosts { get => m_Ghosts; }
        }
        

    }
}