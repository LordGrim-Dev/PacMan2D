
using Game.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PacMan.UI
{
    public class IntroUI : BaseUI
    {

        [SerializeField]
        TextMeshProUGUI m_IntroText;

        [SerializeField]
        Button m_ClickToStartGame;

        private void OnEnable()
        {
            m_ClickToStartGame.onClick.AddListener(OnClickStartGame);
        }
        

        private void Start()
        {
            m_IntroText.text = config.PMConfigManager.Instance().GetLocalisedStringForKey(config.ConfigJsonConstants.kClickToContinue);
        }
        

        private void OnClickStartGame()
        {
#if DEBUG
            GameUtilities.ShowLog("OnClickStartGame");

#endif

            PMUIManager.Instance().OnClickContinue();
        }
    }
}
