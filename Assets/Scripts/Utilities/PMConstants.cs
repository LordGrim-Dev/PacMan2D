
namespace Game.Common
{
    public class PMConstants
    {
        public const int ROW = 4;
        public const int COLUMN = 4;

        public const int MAX_GHOST_COUNT = 5;

        public const int MAX_FRUIT_COUNT = 6;

        //left and right screen to Grid border in percentage
        //100 = 1 means full
        //0.95 = 95% small space
        public const float TILE_SIZE_CONTROLLER = 0.95f;
        public const float GRID_TO_BORDER_CONTROL = 0.90f;

#if UNITY_ANDROID
        public const float SWIPE_THRESHOLD = 5f;
#else
        public const float SWIPE_THRESHOLD = 2f;
#endif

        public const string CONFIG_FILE_NAME = "pac_man_level_data";

        public const string K_LEVEL_DATA_PATH = "LevelData/GameData";

        public const string K_UI_PREFAB_PATH = "Prefabs/UI/";

    }

    public enum SwipeDirection
    {
        none = 0,
        eUp,
        eDown,
        eRight,
        eLeft,
    }

    public enum PacManState
    {
        eNone = 0,
        eRunning,
        eDied,
        eRespawning
    }


    public enum GhostState
    {
        eNone = 0,
        eAtHome,
        eScatter,
        eChase,
        eFrightned
    }


    public enum ItemType
    {
        eNone = 101,
        eFruite,
        ePallet,
        ePowerPallet
    }

    public enum GameState
    {
        eNone = 0,
        eLoading,
        eResume,
        eGamePaused,
        eGameOver
    }
}
