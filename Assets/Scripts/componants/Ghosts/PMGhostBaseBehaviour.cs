using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    [RequireComponent(typeof(PMGhost))]
    public abstract class PMGhostBaseBehaviour : MonoBehaviour
    {
        protected PMGhost m_Ghost { get; private set; }
        
        [SerializeField]
        protected float m_Duration;

        private void Awake()
        {
            m_Ghost = GetComponent<PMGhost>();
        }

        public void Enable()
        {
            Enable(m_Duration);
        }

        public virtual void Enable(float duration)
        {
            enabled = true;

            CancelInvoke();

            Invoke(nameof(Disable), duration);
        }

        public virtual void Disable()
        {
            enabled = false;

            CancelInvoke();
        }

    }
}
