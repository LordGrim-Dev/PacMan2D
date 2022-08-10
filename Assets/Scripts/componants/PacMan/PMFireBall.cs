using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace PacMan
{
    public class PMFireBall : MonoBehaviour, IPoolMember
    {

        [SerializeField]
        Collider2D m_FireBallCollider;

        [SerializeField]
        Transform m_FireBallTransform;

        [SerializeField]
        Rigidbody2D m_FireBallRigidBody;
        private bool m_IsInUse = false;

        private bool m_EncounteredWall = false;

        public bool IsinUse => m_IsInUse;


        
        public void Hide()
        {
            m_FireBallCollider.enabled = false;
            m_EncounteredWall = false;
            m_IsInUse = false;
            m_FireBallTransform.gameObject.SetActive(false);
            m_FireBallTransform.position = Vector2.zero;
        }
        

        public void Show(Vector2 inDirection)
        {
            m_EncounteredWall = false;
            m_IsInUse = true;
            m_FireBallTransform.gameObject.SetActive(true);
            FireBullet(inDirection);
        }
        

        private void FireBullet(Vector2 inPacManfaceDir)
        {
            Vector2 targetPos = PMPacManManager.Instance().PacManTransform.position;

            targetPos = GetTargetmaxBound(targetPos, inPacManfaceDir);

            m_FireBallTransform.DOKill();
            m_FireBallRigidBody.DOMove(targetPos, 0.4f, false)
            .OnStart(() =>
            {
                m_FireBallCollider.enabled = true;
            }).OnComplete(() =>
            {
                Hide();
            }).SetEase(Ease.InOutFlash);
        }
        

        private Vector2 GetTargetmaxBound(Vector2 inCurrentPos, Vector2 inPacManDir)
        {
            Boundaryelements elements = PMGameSceneReferanceHolder.Instance.BoundaryElements;

            Vector2 targetFinal = inCurrentPos;

            float left = Vector2.Dot(inPacManDir, Vector2.left);

            float right = Vector2.Dot(inPacManDir, Vector2.right);

            float down = Vector2.Dot(inPacManDir, Vector2.down);

            if (left >= 1)
            {
                //Left
                targetFinal.x = elements.Left.position.x;
            }
            else if (right >= 1)
            {
                // RIght
                targetFinal.x = elements.Right.position.x;
            }
            else if (down >= 1)
            {
                //Down
                targetFinal.y = elements.Down.position.y;
            }
            else
            {
                //UP
                targetFinal.y = elements.Up.position.y;
            }
            return targetFinal;
        }
        

        private void OnCollisionEnter2D(Collision2D other)
        {
            Hide();
        }
    }
}