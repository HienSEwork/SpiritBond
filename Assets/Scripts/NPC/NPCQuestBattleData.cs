using SpiritBond.Pet;
using UnityEngine;

namespace SpiritBond.NPC
{
    [DisallowMultipleComponent]
    public sealed class NPCQuestBattleData : MonoBehaviour
    {
        [Header("NPC Info")]
        [SerializeField] private string npcDisplayName = "NPC";
        [SerializeField] private string questId = string.Empty;

        [Header("Battle")]
        [SerializeField] private PetData enemyPetData;
        [SerializeField, Min(1)] private int baseEnemyLevel = 1;
        [SerializeField, Min(1)] private int npcOrderIndex = 1;
        [SerializeField] private bool oneTimeOnly;

        [Header("Quest / Interaction")]
        [TextArea]
        [SerializeField] private string interactionLogText = "NPC challenge started.";

        public string NpcDisplayName => string.IsNullOrWhiteSpace(npcDisplayName) ? gameObject.name : npcDisplayName;
        public string QuestId => questId;
        public PetData EnemyPetData => enemyPetData;
        public bool OneTimeOnly => oneTimeOnly;
        public string InteractionLogText => interactionLogText;

        public int GetEnemyLevel()
        {
            int levelOffset = Mathf.Max(0, npcOrderIndex - 1);
            return Mathf.Max(PetProgression.MinLevel, baseEnemyLevel + levelOffset);
        }
    }
}
