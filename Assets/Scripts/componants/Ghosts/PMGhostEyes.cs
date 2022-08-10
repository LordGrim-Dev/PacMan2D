using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PacMan
{

    [RequireComponent(typeof(SpriteRenderer))]

    public class PMGhostEyes : MonoBehaviour
    {

        [SerializeField] private Sprite m_Up;

        [SerializeField] private Sprite m_Down;

        [SerializeField] private Sprite m_Left;

        [SerializeField] private Sprite m_Right;


        [SerializeField] private SpriteRenderer m_SpriteRenderer;

        [SerializeField] private PMMovementComponant m_Movement;


        private void Update()
        {
            bool isUp = IsDirecctionEqualToCurrentMoment(Vector2.up);
            if (isUp)
            {
                m_SpriteRenderer.sprite = m_Up;
            }
            else if (IsDirecctionEqualToCurrentMoment(Vector2.down))
            {
                m_SpriteRenderer.sprite = m_Down;
            }
            else if (IsDirecctionEqualToCurrentMoment(Vector2.left))
            {
                m_SpriteRenderer.sprite = m_Left;
            }
            else if (IsDirecctionEqualToCurrentMoment(Vector2.right))
            {
                m_SpriteRenderer.sprite = m_Right;
            }
        }

        private bool IsDirecctionEqualToCurrentMoment(Vector2 inDir)
        {
            return Vector2.Equals(m_Movement?.CurrentDirection, inDir);
        }

    }

}