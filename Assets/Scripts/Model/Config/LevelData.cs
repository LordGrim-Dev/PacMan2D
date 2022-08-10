
using Newtonsoft.Json;

namespace PacMan.config
{

    [JsonObject(MemberSerialization.OptIn)]
    public class LevelDetails
    {

        [JsonProperty(ConfigJsonConstants.kBonusFruiteCount)]
        private int m_BonuseFruiteCount;

        [JsonProperty(ConfigJsonConstants.kPAckManPowerUP)]
        private int m_PowerUPTime;

        [JsonProperty(ConfigJsonConstants.kPacManPowerCoolDown)]
        private int m_PowerUpCoolDown;

        [JsonProperty(ConfigJsonConstants.kGhostData)]
        private LevelGhostData m_GhostData;

        [JsonProperty(ConfigJsonConstants.kScoreData)]
        private ScoreData m_ScoreDetails;

        [JsonConstructor()]
        public LevelDetails(int bonuseFruiteCount, LevelGhostData inGhostData, ScoreData scoreDetails)
        {
            m_GhostData = inGhostData;
            m_BonuseFruiteCount = bonuseFruiteCount;
            m_ScoreDetails = scoreDetails;
        }


        [JsonIgnore] public ScoreData ScoreDetails { get => m_ScoreDetails; }

        [JsonIgnore] public int BonuseFruiteCount { get => m_BonuseFruiteCount; }

        [JsonIgnore] public int PowerUPTime { get => m_PowerUPTime; }

        [JsonIgnore] public int PowerUpCoolDown { get => m_PowerUpCoolDown; }
        [JsonIgnore] public LevelGhostData GhostData { get => m_GhostData; }
    }
}