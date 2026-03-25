using SpiritBond.Skill;
using UnityEngine;

namespace SpiritBond.Pet
{
    public static class SamplePetContent
    {
        public static PetInstance CreateFirefox()
        {
            SkillData[] skillPool =
            {
                CreateSkill("Flame Bite", "A quick fire bite.", 12, PetType.Fire),
                CreateSkill("Ember Rush", "Charges forward with hot embers.", 14, PetType.Fire),
                CreateSkill("Quick Paw", "A fast normal strike.", 8, PetType.Normal),
                CreateSkill("Cinder Burst", "Releases a burst of cinders.", 10, PetType.Fire)
            };

            PetData firefoxData = ScriptableObject.CreateInstance<PetData>();
            firefoxData.petName = "Firefox";
            firefoxData.primaryType = PetType.Fire;
            firefoxData.maxLevelForEvolution = PetProgression.MaxLevelForm1;
            firefoxData.maxHP = 42;
            firefoxData.attack = 13;
            firefoxData.defense = 8;
            firefoxData.skillPool = skillPool;

            SkillInstance[] loadout = new SkillInstance[skillPool.Length];
            for (int i = 0; i < skillPool.Length; i++)
            {
                loadout[i] = new SkillInstance(skillPool[i]);
            }

            return new PetInstance(firefoxData, loadout);
        }

        private static SkillData CreateSkill(string skillName, string description, int power, PetType skillType)
        {
            SkillData skillData = ScriptableObject.CreateInstance<SkillData>();
            skillData.skillName = skillName;
            skillData.description = description;
            skillData.power = power;
            skillData.skillType = skillType;
            return skillData;
        }
    }
}
