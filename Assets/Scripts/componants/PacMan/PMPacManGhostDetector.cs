using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMPacManGhostDetector : MonoBehaviour, IGhostDetector
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            int id = other.gameObject.GetInstanceID();

            if (!IsGhostsID(id))
                return;

            PMPacManManager.Instance().ShootFireball();
        }
        

        private bool IsGhostsID(int id)
        {
            bool isGhostid = false;

            isGhostid = PMGhostsManager.Instance().IsIdBelongsToGhost(id);

            return isGhostid;
        }

        public void EnableGhostDetector(bool inActivate)
        {
            gameObject.SetActive(inActivate);
        }
        

    }
}
