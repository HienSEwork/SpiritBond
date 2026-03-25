using SpiritBond.Skill;
using UnityEngine;

namespace SpiritBond.Pet
{
    [CreateAssetMenu(fileName = "PetData", menuName = "SpiritBond/Pet Data")]
    public class PetData : ScriptableObject
    {
        public string petName;
        public string description;
        public Sprite avatar;
        public Sprite frontSprite;
        public Sprite backSprite;
        public Sprite battleSprite;
        public PetType primaryType = PetType.Normal;
        public int maxLevelForEvolution = PetProgression.MaxLevelForm1;
        public int maxHP;
        public int attack;
        public int defense;
        public SkillData[] skillPool;
        public LearnableSkill[] learnableSkills;

        [System.Serializable]
        public class LearnableSkill
        {
            public SkillData skillData;
            public int level = PetProgression.MinLevel;
        }
    }
}
