
using Newtonsoft.Json;

namespace PacMan.config
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LevelGhostData
    {
        [JsonProperty(ConfigJsonConstants.kGhostCount)]
        private int m_GhostCount;

        [JsonProperty(ConfigJsonConstants.KGhosMoveSpeed)]
        private int[] m_GhostMomentSpeed;


        [JsonIgnore] public int[] GhostMomentSpeed { get => m_GhostMomentSpeed; }

        [JsonIgnore] public int GhostCount { get => m_GhostCount; }
    }
}
