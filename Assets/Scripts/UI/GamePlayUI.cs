using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PacMan.UI
{
    [System.Serializable]
    public class GamePlayUI : BaseUI
    {
        [UnityEngine.SerializeField]
        TextMeshProUGUI m_ScoreText;

        [UnityEngine.SerializeField]
        TextMeshProUGUI m_PlayerLivesLeft;

        [SerializeField]
        Slider m_PowerUpSlider;

        Sequence m_SliderSeq;

        public TextMeshProUGUI ScoreText { get => m_ScoreText; }
        public TextMeshProUGUI PlayerLivesLeft { get => m_PlayerLivesLeft; }

        
        private void OnEnable()
        {
            UIEventManager.Instance.OnPlayerScoreUpdate += OnPlayerScoreUpdate;

            UIEventManager.Instance.OnPlayerLivesUpdate += OnLivesLeftUpdate;

            GameEventManager.Instance.OnGamePause += OnGamePause;

            GameEventManager.Instance.OnGameOver += OnGameOver;
            
            GameEventManager.Instance.OnNewRoundStart += OnStarted;

        }

        private void OnDisable()
        {
            UIEventManager.Instance.OnPlayerScoreUpdate -= OnPlayerScoreUpdate;

            UIEventManager.Instance.OnPlayerLivesUpdate -= OnLivesLeftUpdate;

            GameEventManager.Instance.OnGamePause -= OnGamePause;

            GameEventManager.Instance.OnGameOver -= OnGameOver;
            GameEventManager.Instance.OnNewRoundStart -= OnStarted;

        }

        
        private void OnStarted()
        {
            StartPowerUpSliderAnimation();
        }

        private void OnLivesLeftUpdate(int inLivesLeft)
        {
            string lives = inLivesLeft.ToString();
            m_PlayerLivesLeft.text = lives;

            float duration = 0.5f;
            AnimateScaleUp(m_PlayerLivesLeft.transform, duration);
        }
        

        private void OnPlayerScoreUpdate(int inScore)
        {
            m_ScoreText.text = inScore.ToString();
        }
        

        private void AnimateScaleUp(Transform inTransform, float inDur)
        {
            Vector3 originalScale = inTransform.localScale;
            Vector3 scaleUp = originalScale * 1.25f;
            inTransform.DOKill();
            inTransform.DOScale(scaleUp, inDur).SetEase(Ease.Linear).OnComplete(() =>
            {
                inTransform.localScale = originalScale;
            });
        }
        

        private void Start()
        {
            m_ScoreText.text = "0";
            string lives = PacMan.PMPacManManager.Instance().TotalPacManLivesLeft.ToString();
            m_PlayerLivesLeft.text = lives;
        }
        

        private void StartPowerUpSliderAnimation()
        {
            m_PowerUpSlider.value = 0;

            int powerUpTime = PMUIManager.Instance().PowerUPTime;
            int coolDown = PMUIManager.Instance().PowerUpCoolDown;

            m_PowerUpSlider?.DOKill();

            m_SliderSeq.Kill();
            m_SliderSeq = DOTween.Sequence();
            m_SliderSeq.Append(m_PowerUpSlider.DOValue(1, powerUpTime, false).SetEase(Ease.Linear).
            OnComplete(() =>
            {
                GameEventManager.Instance.TriggerOnPowerUpActivated(true);
            }));
            m_SliderSeq.Append(m_PowerUpSlider.DOValue(0, coolDown, false).SetEase(Ease.Linear).OnComplete(() =>
            {
                GameEventManager.Instance.TriggerOnPowerUpActivated(false);
            }));

            m_SliderSeq.SetLoops(-1);
            m_SliderSeq.Play();
        }
        
        private void OnGameOver()
        {
            m_SliderSeq.Pause();
            m_SliderSeq.Kill();
        }
        

        private void OnGamePause(bool inPause)
        {
            if (inPause)
                m_SliderSeq.Pause();
            else
                m_SliderSeq?.Play();
        }
        



    }
}