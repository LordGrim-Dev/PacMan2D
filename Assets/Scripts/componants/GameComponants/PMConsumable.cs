
using Game.Common;
using UnityEngine;

namespace PacMan
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class PMConsumable : MonoBehaviour
    {
        [SerializeField]
        ItemType m_ItemType;

        int m_PacManInstanceID;

        private void Start()
        {
            m_PacManInstanceID = PMPacManManager.Instance().GetPackManInstanceID;
        }

        public abstract void OnPacManEncountered();

        private void OnTriggerEnter2D(Collider2D other)
        {
            int id = other.gameObject.GetInstanceID();

            if (id != m_PacManInstanceID)
            {
                return;
            }

            gameObject.SetActive(false);

            OnPacManEncountered();
        }
    }
}
