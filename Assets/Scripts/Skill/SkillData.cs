using SpiritBond.Pet;
using UnityEngine;

namespace SpiritBond.Skill
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SpiritBond/Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        public string description;
        public PetType skillType = PetType.Normal;
        public int power;
    }
}
