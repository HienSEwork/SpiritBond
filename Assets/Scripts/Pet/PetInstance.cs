using System.Collections.Generic;
using SpiritBond.Skill;
using UnityEngine;

namespace SpiritBond.Pet
{
    [System.Serializable]
    public class PetInstance
    {
        public PetData petData;
        public int level;
        public int currentExp;
        public int currentHP;
        public SkillInstance[] skills;

        public PetInstance(PetData data)
        {
            petData = data;
            level = PetProgression.MinLevel;
            currentExp = 0;
            currentHP = data != null ? data.maxHP : 0;
            skills = BuildSkillLoadout(data);
        }

        public bool IsFainted => currentHP <= 0;

        public int MaxLevelForEvolution
        {
            get
            {
                if (petData == null)
                {
                    return PetProgression.MaxLevelForm1;
                }

                return Mathf.Max(PetProgression.MinLevel, petData.maxLevelForEvolution);
            }
        }

        public int GetExpRequiredForNextLevel()
        {
            return PetProgression.BaseExpToLevelUp + ((level - PetProgression.MinLevel) * PetProgression.ExpStepPerLevel);
        }

        public bool AddExp(int amount)
        {
            if (amount <= 0 || level >= MaxLevelForEvolution)
            {
                return false;
            }

            bool leveledUp = false;
            currentExp += amount;

            while (level < MaxLevelForEvolution)
            {
                int requiredExp = GetExpRequiredForNextLevel();
                if (currentExp < requiredExp)
                {
                    break;
                }

                currentExp -= requiredExp;
                level++;
                leveledUp = true;
            }

            if (level >= MaxLevelForEvolution)
            {
                level = MaxLevelForEvolution;
                currentExp = 0;
            }

            return leveledUp;
        }

        private SkillInstance[] BuildSkillLoadout(PetData data)
        {
            SkillInstance[] loadout = new SkillInstance[4];
            if (data == null || data.skillPool == null || data.skillPool.Length == 0)
            {
                return loadout;
            }

            List<SkillData> availableSkills = new List<SkillData>();
            for (int i = 0; i < data.skillPool.Length; i++)
            {
                if (data.skillPool[i] != null)
                {
                    availableSkills.Add(data.skillPool[i]);
                }
            }

            if (availableSkills.Count == 0)
            {
                return loadout;
            }

            for (int i = 0; i < loadout.Length; i++)
            {
                SkillData selectedSkill = availableSkills[Random.Range(0, availableSkills.Count)];
                loadout[i] = new SkillInstance(selectedSkill);
            }

            return loadout;
        }
    }
}
