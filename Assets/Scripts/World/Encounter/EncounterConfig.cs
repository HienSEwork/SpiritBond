using System.Collections.Generic;
using SpiritBond.Pet;
using UnityEngine;

namespace SpiritBond.World.Encounter
{
    [CreateAssetMenu(fileName = "EncounterConfig", menuName = "SpiritBond/Encounter Config")]
    public class EncounterConfig : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)] private float spawnRate = 0.2f;
        [SerializeField] private List<PetData> petList = new List<PetData>();

        public float SpawnRate => spawnRate;

        public PetData GetRandomPet()
        {
            if (petList == null || petList.Count == 0)
            {
                return null;
            }

            int randomIndex = Random.Range(0, petList.Count);
            return petList[randomIndex];
        }
    }
}
