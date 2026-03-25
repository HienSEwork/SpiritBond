using SpiritBond.Skill;
using UnityEngine;

namespace SpiritBond.Pet
{
    [CreateAssetMenu(fileName = "PetData", menuName = "SpiritBond/Pet Data")]
    public class PetData : ScriptableObject
    {
        public string petName;
        public Sprite avatar;
        public Sprite battleSprite;
        public PetType primaryType = PetType.Normal;
        public int maxLevelForEvolution = PetProgression.MaxLevelForm1;
        public int maxHP;
        public int attack;
        public int defense;
        public SkillData[] skillPool;
    }
}
