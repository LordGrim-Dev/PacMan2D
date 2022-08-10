using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PacMan
{
    public class PMAnimateSprite : MonoBehaviour, IAnimateSprite
    {
        public SpriteRenderer m_SpriteRenderer { get; private set; }
        public bool Enabled { set => enabled = value; }
        public bool SpriteEnabled { set => m_SpriteRenderer.enabled = value; }

        [SerializeField]
        private Sprite[] m_AnimationSprites = new Sprite[0];

        [SerializeField]
        private float m_AnimationTime = 0.25f;

        [SerializeField]
        private float m_RepeatRate = 0.20f;

        [SerializeField]
        private bool m_isLoopEnabled = true;

        int m_SpriteIndex;

        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void StartAnimation()
        {
            m_SpriteIndex = 0;
            InvokeRepeating(nameof(Animate), m_AnimationTime, m_RepeatRate);
        }

        public void StopAnimation()
        {
            CancelInvoke(nameof(Animate));
        }
      
        private void Animate()
        {
            if (!m_SpriteRenderer.enabled)
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
                m_SpriteRenderer.sprite = m_AnimationSprites[m_SpriteIndex];
            }
        }
        

        public void Restart()
        {
            m_SpriteIndex = -1;
            StartAnimation();
        }
    }
}