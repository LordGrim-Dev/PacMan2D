
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class PMGameSceneUpdater : MonoBehaviour
    {
        private void Awake()
        {
            config.PMConfigManager.Instance().LoadConfig();

            PMGameManager.Instance.InitialiseAndShowIntro();
        }
        

        void Start()
        {
            PMScoreManager.Instance();

            PMPacManManager.Instance();

            PMConsumableManager.Instance();

            TouchManager.Instance();

            PMGhostsManager.Instance();
        }
        

        void Update()
        {
            TouchManager.Instance().OnUpdate();
        }
        
        private void OnDestroy()
        {

        }
        
    }
}