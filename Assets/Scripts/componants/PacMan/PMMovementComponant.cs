
using System;
using UnityEngine;

namespace PacMan
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PMMovementComponant : MonoBehaviour, IMomentComponant
    {
        [SerializeField]
        protected Rigidbody2D m_RigidBody;

        [SerializeField]
        private float m_Speed;

        [SerializeField]
        private float m_SpeedMultiplier = 1f;

        [SerializeField]
        private LayerMask m_ObstacleLayer;

        [SerializeField]
        private Vector2 m_InitialDirection;

        private Vector2 m_CurrentDirection;
        private Vector2 m_NextDirection;
        private Vector3 m_StartingPosition;
        public Vector2 CurrentDirection { get => m_CurrentDirection; }

        public bool Enabled { get => enabled; set => enabled = value; }

        public Rigidbody2D Rigidbody
        {
            get
            {
                return m_RigidBody;
            }
        }
        
        public void OnAwake()
        {
            m_StartingPosition = transform.position;
        }
        

        public void OnStart()
        {
            ResetState();
        }
        

        public void OnUpdate()
        {

            bool isZero = Vector2.Equals(m_NextDirection, Vector2.zero);
            bool isSameDir = Vector2.Equals(m_CurrentDirection, m_NextDirection);
            if (isZero || isSameDir)
            {
                return;
            }

            SetDirection(m_NextDirection);

        }
        

        public void OnFixedUpdate()
        {

            Vector2 translation = m_CurrentDirection * m_Speed * m_SpeedMultiplier * Time.fixedDeltaTime;
            Vector2 destination = m_RigidBody.position + translation;

            m_RigidBody.MovePosition(destination);
        }

        
        public void UpdateSpeedMultiplier(float inSpeedMult)
        {
            m_SpeedMultiplier = inSpeedMult;
        }
        
        public void UpdateSpeed(float inSpeed)
        {
            if (inSpeed == 0)
                inSpeed = 6;
            m_Speed = inSpeed;
        }
        
        public void SetDirection(Vector2 direction, bool inForceUpdate = false)
        {
            if (inForceUpdate || !Occupied(direction))
            {
                m_CurrentDirection = direction;
                m_NextDirection = Vector2.zero;
            }
            else
            {
                m_NextDirection = direction;
            }
        }
        

        private bool Occupied(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, m_ObstacleLayer);
            return hit.collider != null;
        }
        

        public void ResetState()
        {
            m_SpeedMultiplier = 1f;
            m_CurrentDirection = m_InitialDirection;
            m_NextDirection = Vector2.zero;
            transform.position = m_StartingPosition;
            enabled = true;
        }
        
    }
}