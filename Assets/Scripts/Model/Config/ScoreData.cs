using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace PacMan.config
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ScoreData
    {
        [JsonProperty(ConfigJsonConstants.kEnemyKillPoint)]
        private int m_GhostKillPoint;

        [JsonProperty(ConfigJsonConstants.kBonusFruiteCollect)]
        private int m_BonusFruitCollectPoint;

        [JsonProperty(ConfigJsonConstants.kPowerPalletCollectPoint)]
        private int m_PowerPalletEatPoint;

        [JsonProperty(ConfigJsonConstants.kPalletCollectPoint)]
        private int m_PalletEatPoint;

        [JsonConstructor()]
        public ScoreData(int enemyKilPoint, int bonusFruitCollectPoint, int powerPalletEatPoint, int palletEatPoint)
        {
            m_GhostKillPoint = enemyKilPoint;
            m_BonusFruitCollectPoint = bonusFruitCollectPoint;
            m_PowerPalletEatPoint = powerPalletEatPoint;
            m_PalletEatPoint = palletEatPoint;

        }

        [JsonIgnore] public int GhostKillPoint { get => m_GhostKillPoint; }

        [JsonIgnore] public int BonusFruitCollectPoint { get => m_BonusFruitCollectPoint; }

        [JsonIgnore] public int PowerPalletEatPoint { get => m_PowerPalletEatPoint; }

        [JsonIgnore] public int PalletEatPoint { get => m_PalletEatPoint; }
    }
}
