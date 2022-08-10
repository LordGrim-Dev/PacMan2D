using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class PMGhostFrightned : PMGhostBaseBehaviour
    {
        [SerializeField]
        private SpriteRenderer m_GhostBody;

        [SerializeField]
        private SpriteRenderer m_GhostEyes;

        [SerializeField]
        private SpriteRenderer m_BlueGhost;

        [SerializeField]
        private SpriteRenderer m_WhiteGhost;

        private bool m_IsGhostEaten = false;

        public override void Disable()
        {
            base.Disable();
            m_WhiteGhost.enabled = false;
            m_BlueGhost.enabled = false;
            m_GhostBody.enabled = true;
            m_GhostEyes.enabled = true;
        }

        public override void Enable(float duration = 0)
        {
            if (duration == 0)
                duration = m_Duration;

            base.Enable(duration);
#if DEBUG
            GameUtilities.ShowLog("PMGhostFrightned : Enable");
#endif
            m_BlueGhost.enabled = true;
            m_GhostBody.enabled = false;
            m_GhostEyes.enabled = false;
            m_WhiteGhost.enabled = false;

            Invoke(nameof(Flash), duration / 2f);
        }

        private void Eaten()
        {
            m_IsGhostEaten = true;

            m_GhostEyes.enabled = true;
            m_GhostBody.enabled = false;
            m_BlueGhost.enabled = false;
            m_WhiteGhost.enabled = false;
        }

        private void Flash()
        {
            if (m_IsGhostEaten)
                return;

            m_BlueGhost.enabled = false;
            m_WhiteGhost.enabled = true;
            m_WhiteGhost.GetComponent<PMAnimateSprite>().Restart();
        }


        private void OnEnable()
        {
            m_BlueGhost.GetComponent<PMAnimateSprite>().Restart();
            m_Ghost?.GhostMoment?.UpdateSpeedMultiplier(0.5f);
            m_IsGhostEaten = false;
        }

        private void OnDisable()
        {
            m_Ghost.GhostMoment.UpdateSpeedMultiplier(1f);
            m_IsGhostEaten = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PMGhostNodePoint node = other.GetComponent<PMGhostNodePoint>();
            if (!enabled || node == null)
                return;

            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;
            Vector3 tempVector = Vector3.zero;

            Transform target = PMPacManManager.Instance().PacManTransform;

            foreach (Vector2 availableDirection in node.AvailableDirections)
            {
                tempVector.x = availableDirection.x;
                tempVector.y = availableDirection.y;

                Vector3 newPosition = transform.position + tempVector;

                float distance = (target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            m_Ghost.GhostMoment.SetDirection(direction);

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            int pacManInstanceId = PMPacManManager.Instance().GetPackManInstanceID;

            if (pacManInstanceId != collision.gameObject.GetInstanceID() || !enabled)
                return;

            Eaten();
        }

    }
}