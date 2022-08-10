using UnityEngine;
namespace PacMan
{

    [RequireComponent(typeof(Collider2D))]
    public class Passage : MonoBehaviour
    {
        public Transform connection;

        private void OnTriggerEnter2D(Collider2D other)
        {
            int id = other.gameObject.GetInstanceID();

            bool isPacMan = (id == PMPacManManager.Instance().GetPackManInstanceID);
            bool isGhost = PMGhostsManager.Instance().IsIdBelongsToGhost(id);

            if (isPacMan || isGhost)
            {
                Vector3 position = connection.position;
                position.z = other.transform.position.z;
                other.transform.position = position;
            }
        }

    }
}