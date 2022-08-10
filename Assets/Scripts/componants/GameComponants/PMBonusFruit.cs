using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class PMBonusFruit : PMConsumable
    {
        [SerializeField]
        Sprite[] m_RandomFruits;

        private void OnEnable()
        {
            int randomIndex = Random.Range(0, m_RandomFruits.Length);
            SpriteRenderer spriteRen = GetComponent<SpriteRenderer>();
            spriteRen.sprite = m_RandomFruits[randomIndex];
        }

        public override void OnPacManEncountered()
        {
#if DEBUG
            Game.Common.GameUtilities.ShowLog(" Fruit Consumed:");
#endif

            GameEventManager.Instance.TriggerItemConsumed(Game.Common.ItemType.eFruite);
        }
    }
}