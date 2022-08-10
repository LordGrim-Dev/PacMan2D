using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMGhostNodePoint : MonoBehaviour
    {
        [SerializeField]
        private LayerMask m_obstacleLayer;
        public List<Vector2> AvailableDirections { get; private set; }

        private void Start()
        {
            AvailableDirections = new List<Vector2>();

            CheckAvailableDirection(Vector2.up);
            CheckAvailableDirection(Vector2.down);
            CheckAvailableDirection(Vector2.left);
            CheckAvailableDirection(Vector2.right);
        }

        private void CheckAvailableDirection(Vector2 direction)
        {
            Vector2 rayWidth = Vector2.one * 0.5f;
            float angle = 0;

            RaycastHit2D hit = Physics2D.BoxCast(transform.position, rayWidth, angle, direction, 1f, m_obstacleLayer);

            if (hit.collider != null)
            {
                return;
            }

            AvailableDirections.Add(direction);
        }
    }
}