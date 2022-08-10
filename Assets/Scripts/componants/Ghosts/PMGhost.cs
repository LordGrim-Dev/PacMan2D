using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMGhost : MonoBehaviour
    {
        public PMMovementComponant GhostMoment { get; private set; }
        public PMGhostAtHome GhostAtHome { get; private set; }
        public PMGhostScatter GhostScatter { get; private set; }
        public PMGhostChase GhoseChasing { get; private set; }
        public PMGhostFrightned FrightnedGhost { get; private set; }

        public PMGhostBaseBehaviour InitialBehaviour;

        private bool m_IsPaused;

        

        private void Awake()
        {
            GhostMoment = GetComponent<PMMovementComponant>();
            GhostAtHome = GetComponent<PMGhostAtHome>();
            GhostScatter = GetComponent<PMGhostScatter>();
            GhoseChasing = GetComponent<PMGhostChase>();
            FrightnedGhost = GetComponent<PMGhostFrightned>();

            GhostMoment.OnAwake();
        }
        

        private void Start()
        {
            ResetState();

            GhostMoment.OnStart();
        }
        
        private void Update()
        {
            if (m_IsPaused)
                return;

            GhostMoment.OnUpdate();
        }

        private void FixedUpdate()
        {
            if (m_IsPaused)
                return;

            GhostMoment.OnFixedUpdate();
        }
        

        public void ResetState()
        {
            //Edge condition
            if (PMGameManager.Instance.CurrentGameState == Game.Common.GameState.eGameOver)
                return;

            m_IsPaused = false;

            gameObject.SetActive(true);

            GhostMoment.ResetState();

            FrightnedGhost.Disable();

            GhoseChasing.Disable();

            GhostAtHome.OnReset();

            GhostScatter.Enable();

            if (GhostAtHome != InitialBehaviour)
            {
                GhostAtHome.Disable();
            }

            InitialBehaviour?.Enable();

            Transform homeInside = PMGameSceneReferanceHolder.Instance.GhostRef.GhostHomeInside;
            SetPosition(homeInside.position);

        }
        
        public void OnPauseStatus(bool inPauseStatus)
        {
            m_IsPaused = inPauseStatus;
        }
        
        public void Configure(int Speed)
        {
            GhostMoment.UpdateSpeed(Speed);
        }

        
        public void SetPosition(Vector3 position)
        {
            position.z = transform.position.z;
            transform.position = position;
        }
        

        private void OnCollisionEnter2D(Collision2D collision)
        {
            int id = collision.gameObject.GetInstanceID();
            bool isFireBall = PMPacManManager.Instance().IsFireBall(id);

            if (id != PMPacManManager.Instance().GetPackManInstanceID && !isFireBall)
                return;

            if (FrightnedGhost.enabled || isFireBall)
            {
                ResetState();
                GhostAtHome.OnGhostEaten();
                GameEventManager.Instance.TriggerGhostBeenKilled();
            }
            else
            {
                GameEventManager.Instance.TriggerPacManBeingKilled();
            }

        }
        


    }
}