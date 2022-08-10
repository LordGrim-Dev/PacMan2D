using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMGhostChase : PMGhostBaseBehaviour
    {
        private void OnDisable()
        {
            m_Ghost.GhostScatter.Enable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_Ghost.FrightnedGhost.enabled || !enabled)
            {
                return;
            }
            PMGhostNodePoint node = other.GetComponent<PMGhostNodePoint>();

            if (node == null)
            {
                return;
            }

            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            Transform target = PMPacManManager.Instance().PacManTransform;
            Vector3 tempVector = Vector3.zero;

            foreach (Vector2 availableDirection in node.AvailableDirections)
            {
                tempVector.x = availableDirection.x;
                tempVector.y = availableDirection.y;

                // If the distance in this direction is less than the current
                // min distance then this direction becomes the new closest
                
                Vector3 newPosition = transform.position + tempVector;
                float distance = (target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            m_Ghost.GhostMoment.SetDirection(direction);
        }
    }
}
