using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PacMan
{
    public class PMAnimateComponant : MonoBehaviour, IPacManAnimation
    {
        [SerializeField]
        protected SpriteRenderer m_Sprite;

        [SerializeField]
        private Sprite[] m_AnimationSprites = new Sprite[0];

        [SerializeField]
        private float m_AnimationTime = 0.25f;

        [SerializeField]
        private float m_RepeatRate = 0.20f;

        [SerializeField]
        private bool m_isLoopEnabled = true;

        private bool m_IsPaused;

        int m_SpriteIndex;
        

        public void StartAnimation()
        {
            m_IsPaused = false;
            StopAnimation();
            m_SpriteIndex = 0;
            InvokeRepeating(nameof(Animate), m_AnimationTime, m_RepeatRate);
        }
        

        public void StopAnimation()
        {
            CancelInvoke(nameof(Animate));
        }

        public void OnPause(bool inStatus)
        {
            m_IsPaused = inStatus;
        }
        

        private void Animate()
        {
            if (!m_Sprite.enabled || m_IsPaused)
            {
                return;
            }

            m_SpriteIndex++;

            if (m_SpriteIndex >= m_AnimationSprites.Length && m_isLoopEnabled)
            {
                m_SpriteIndex = 0;
            }

            if (m_SpriteIndex >= 0 && m_SpriteIndex < m_AnimationSprites.Length)
            {
                m_Sprite.sprite = m_AnimationSprites[m_SpriteIndex];
            }
        }
        

        public void Restart()
        {
            m_SpriteIndex = -1;
            StartAnimation();
        }
        
    }
}