using SpiritBond.Pet;
using UnityEngine;

namespace SpiritBond.Battle
{
    public class BattleUnit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer; // Assign in Inspector

        public PetInstance PetInstance { get; private set; }

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        public void Setup(PetInstance petInstance)
        {
            PetInstance = petInstance;

            if (spriteRenderer != null && petInstance != null && petInstance.petData != null)
            {
                spriteRenderer.sprite = petInstance.petData.battleSprite;
            }

            if (petInstance != null && petInstance.petData != null)
            {
                Debug.Log($"[BattleUnit] Setup unit {gameObject.name} with pet {petInstance.petData.petName} | HP={petInstance.currentHP}");
            }
        }

        public void TakeDamage(int damage)
        {
            if (PetInstance == null)
            {
                return;
            }

            PetInstance.currentHP = Mathf.Max(0, PetInstance.currentHP - Mathf.Max(0, damage));
            string petName = PetInstance.petData != null ? PetInstance.petData.petName : gameObject.name;
            Debug.Log($"[BattleUnit] {petName} took {damage} damage. Current HP: {PetInstance.currentHP}");
        }
    }
}
