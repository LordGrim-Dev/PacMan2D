using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class PMPacMan : MonoBehaviour
    {

        [SerializeField]
        SpriteRenderer m_PacManSprite;

        [SerializeField]
        CircleCollider2D m_PacManCollider;
        
        [SerializeField]
        PMPacManGhostDetector m_GhostInLaneDetector;
        
        [SerializeField]
        PMAnimateSprite m_DeathSequence;

        IPacManAnimation m_AnimateComponant;

        IMomentComponant m_Movement;


        PacManState m_PacManState;

        private float m_PrevRotAngle;

        private bool m_GamePaused;

        public PacManState PacManState { get => m_PacManState; }
        public Vector2 CurrentDirection { get => m_Movement.CurrentDirection; }

        

        private void OnEnable()
        {
            ChangePacManState(PacManState.eNone);

            TouchManager.Instance().SubscribeToInput(OnDirectionUpdate);
        }

        

        private void OnDisable()
        {
            TouchManager.Instance().UnSubscribeToInput(OnDirectionUpdate);
        }
        
        private void Awake()
        {
            m_AnimateComponant = GetComponent<IPacManAnimation>();
            m_Movement = GetComponent<IMomentComponant>();
            EnableLaneGhostDetector(false);
            m_Movement.OnAwake();
        }
        
        private void Start()
        {
            ChangePacManState(PacManState.eRunning);
            m_Movement.OnStart();
            m_AnimateComponant.StartAnimation();
        }
        

        private void Update()
        {
#if DEBUG
            if (Input.GetKeyDown(KeyCode.Z))
            {
                PMPacManManager.Instance().ShootFireball();
                // GameEventManager.Instance.TriggerOnPowerUpActivated();
            }
#endif
            if (m_GamePaused)
                return;

            m_Movement.OnUpdate();
            CheckForPacManRotation();
        }

        internal void EnableLaneGhostDetector(bool inSetActive)
        {
            m_GhostInLaneDetector.EnableGhostDetector(inSetActive);
        }

        
        private void FixedUpdate()
        {
            if (m_GamePaused)
                return;

            m_Movement.OnFixedUpdate();

        }

        internal void Pause(bool inPasueStatus)
        {
            m_GamePaused = inPasueStatus;

            m_AnimateComponant.OnPause(inPasueStatus);
        }

        

        private void ChangePacManState(PacManState inState)
        {
            m_PacManState = inState;
        }
        

        private void OnDirectionUpdate(SwipeDirection inDir)
        {
            switch (inDir)
            {
                case Game.Common.SwipeDirection.eUp:
                    m_Movement.SetDirection(Vector2.up);
                    break;

                case Game.Common.SwipeDirection.eDown:
                    m_Movement.SetDirection(Vector2.down);
                    break;

                case Game.Common.SwipeDirection.eLeft:
                    m_Movement.SetDirection(Vector2.left);
                    break;

                case Game.Common.SwipeDirection.eRight:
                    m_Movement.SetDirection(Vector2.right);
                    break;
            }
        }
        

        private void CheckForPacManRotation()
        {
            // Rotate pacman to face the movement direction
            float angle = Mathf.Atan2(m_Movement.CurrentDirection.y, m_Movement.CurrentDirection.x);

            if (m_PrevRotAngle == angle)
                return;

            m_PrevRotAngle = angle;

            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
        

        public void OnPacManDeath()
        {
            ChangePacManState(PacManState.eDied);

            m_PacManCollider.enabled = false;

            m_PacManSprite.enabled = false;

            m_AnimateComponant.StopAnimation();

            DeathSequence();
        }
        

        public void ResetPacMan()
        {
            enabled = true;
            m_PacManSprite.enabled = true;
            m_PacManCollider.enabled = true;
            m_DeathSequence.Enabled = false;
            m_DeathSequence.SpriteEnabled = false;
            m_Movement.ResetState();

            m_AnimateComponant.StartAnimation();

            gameObject.SetActive(true);

            ChangePacManState(PacManState.eRunning);
        }
        

        private void DeathSequence()
        {
            enabled = false;
            m_PacManSprite.enabled = false;
            m_PacManCollider.enabled = false;
            m_Movement.Enabled = false;
            m_DeathSequence.Enabled = true;
            m_DeathSequence.SpriteEnabled = true;
            m_DeathSequence.Restart();

            EnableLaneGhostDetector(false);

            ChangePacManState(PacManState.eRespawning);
        }
        

    }
}
