using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMGhostAtHome : PMGhostBaseBehaviour
    {

        Coroutine m_HomeExit;

        

        public void OnReset()
        {
            if (m_HomeExit != null)
                StopCoroutine(m_HomeExit);
            m_Ghost.GhostMoment.Rigidbody.isKinematic = false;
        }

        

        private void OnEnable()
        {
            if (m_HomeExit != null)
                StopCoroutine(m_HomeExit);
        }
        


        private void OnDisable()
        {
            if (gameObject.activeInHierarchy)
            {
                m_HomeExit = StartCoroutine(ExitTransition());
            }
        }
        


        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Reverse direction everytime the ghost hits a wall to create the
            // effect of the ghost bouncing around the home
            if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                m_Ghost.GhostMoment.SetDirection(-m_Ghost.GhostMoment.CurrentDirection);
            }
        }

        

        private IEnumerator ExitTransition()
        {
            m_Ghost.GhostMoment.SetDirection(Vector2.up, true);
            m_Ghost.GhostMoment.Rigidbody.isKinematic = true;
            m_Ghost.GhostMoment.enabled = false;

            Vector3 position = transform.position;

            float duration = 0.5f;
            float elapsed = 0f;

            PMGameSceneReferanceHolder refHolder = PMGameSceneReferanceHolder.Instance;
            Transform homeInside = refHolder.GhostRef.GhostHomeInside;
            Transform homeOutside = refHolder.GhostRef.OutSide;

            // Animate to the starting point
            while (elapsed < duration)
            {
                Vector3 pos = Vector3.Lerp(position, homeInside.position, elapsed / duration);
                m_Ghost.SetPosition(pos);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            // Animate exiting the Ghost home
            while (elapsed < duration)
            {
                m_Ghost.SetPosition(Vector3.Lerp(homeInside.position, homeOutside.position, elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Pick a random direction left or right and re-enable GhostMoment
            m_Ghost.GhostMoment.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
            m_Ghost.GhostMoment.Rigidbody.isKinematic = false;
            m_Ghost.GhostMoment.enabled = true;
        }
        

        public void OnGhostEaten()
        {
            Transform homeInside = PMGameSceneReferanceHolder.Instance.GhostRef.GhostHomeInside;
            m_Ghost.SetPosition(homeInside.position);
            Enable(m_Duration);
        }

    }
}
