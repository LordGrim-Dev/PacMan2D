using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMGhostScatter : PMGhostBaseBehaviour
    {
        private void OnDisable()
        {
            m_Ghost.GhoseChasing.Enable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            PMGhostNodePoint node = other.GetComponent<PMGhostNodePoint>();
            if (node == null || !enabled || m_Ghost.FrightnedGhost.enabled)
                return;

            int index = Random.Range(0, node.AvailableDirections.Count);

            if (node.AvailableDirections[index] == -m_Ghost.GhostMoment.CurrentDirection && node.AvailableDirections.Count > 1)
            {
                index++;

                if (index >= node.AvailableDirections.Count)
                {
                    index = 0;
                }
            }

            m_Ghost.GhostMoment.SetDirection(node.AvailableDirections[index]);

        }


    }
}
