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
        public int maxHP;
        public int attack;
        public int defense;
        public SkillData[] skillPool;
    }
}
