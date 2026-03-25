using System.Collections.Generic;
using SpiritBond.Skill;
using UnityEngine;

namespace SpiritBond.Pet
{
    [System.Serializable]
    public class PetInstance
    {
        public string instanceId;
        public PetData petData;
        public int level;
        public int currentExp;
        public int currentHP;
        public SkillInstance[] skills;

        public PetInstance(PetData data)
        {
            petData = data;
            instanceId = System.Guid.NewGuid().ToString("N");
            level = PetProgression.MinLevel;
            currentExp = 0;
            currentHP = data != null ? data.maxHP : 0;
            skills = BuildSkillLoadout(data);
        }

        public PetInstance(PetData data, int startingLevel)
        {
            petData = data;
            instanceId = System.Guid.NewGuid().ToString("N");
            level = Mathf.Max(PetProgression.MinLevel, startingLevel);
            currentExp = 0;
            currentHP = data != null ? data.maxHP : 0;
            skills = BuildSkillLoadout(data);
        }

        public PetInstance(PetData data, SkillInstance[] predefinedSkills)
        {
            petData = data;
            instanceId = System.Guid.NewGuid().ToString("N");
            level = PetProgression.MinLevel;
            currentExp = 0;
            currentHP = data != null ? data.maxHP : 0;
            skills = CloneLoadout(predefinedSkills);
        }

        internal PetInstance(PetData data, string savedInstanceId, int savedLevel, int savedCurrentExp, int savedCurrentHP, SkillInstance[] savedSkills)
        {
            petData = data;
            instanceId = string.IsNullOrWhiteSpace(savedInstanceId) ? System.Guid.NewGuid().ToString("N") : savedInstanceId;
            level = Mathf.Max(PetProgression.MinLevel, savedLevel);
            currentExp = Mathf.Max(0, savedCurrentExp);
            currentHP = Mathf.Max(0, savedCurrentHP);
            skills = CloneLoadout(savedSkills);

            if (skills == null || skills.Length == 0)
            {
                skills = BuildSkillLoadout(data);
            }
        }

        public bool IsFainted => currentHP <= 0;
        public bool CanBattle => petData != null && currentHP > 0;

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

        public void RestoreToFullHealth()
        {
            currentHP = petData != null ? petData.maxHP : 0;
        }

        public void EnsureInstanceId()
        {
            if (string.IsNullOrWhiteSpace(instanceId))
            {
                instanceId = System.Guid.NewGuid().ToString("N");
            }
        }

        public PetInstanceSaveData ToSaveData()
        {
            EnsureInstanceId();

            return new PetInstanceSaveData
            {
                instanceId = instanceId,
                level = level,
                currentExp = currentExp,
                currentHP = currentHP,
                petData = PetDataSaveData.FromPetData(petData),
                skills = SkillDataSaveData.FromSkillInstances(skills)
            };
        }

        public static PetInstance FromSaveData(PetInstanceSaveData saveData)
        {
            if (saveData == null || saveData.petData == null)
            {
                return null;
            }

            PetData petData = saveData.petData.ToPetData();
            SkillInstance[] savedSkills = SkillDataSaveData.ToSkillInstances(saveData.skills);

            return new PetInstance(
                petData,
                saveData.instanceId,
                saveData.level,
                saveData.currentExp,
                saveData.currentHP,
                savedSkills
            );
        }

        private SkillInstance[] BuildSkillLoadout(PetData data)
        {
            SkillInstance[] loadout = new SkillInstance[4];
            if (data == null)
            {
                return loadout;
            }

            List<SkillData> availableSkills = BuildAvailableSkills(data);
            if (availableSkills.Count == 0)
            {
                return loadout;
            }

            for (int i = 0; i < loadout.Length; i++)
            {
                if (i < availableSkills.Count)
                {
                    loadout[i] = new SkillInstance(availableSkills[i]);
                }
            }

            return loadout;
        }

        private List<SkillData> BuildAvailableSkills(PetData data)
        {
            List<SkillData> availableSkills = new List<SkillData>();

            if (data.learnableSkills != null && data.learnableSkills.Length > 0)
            {
                for (int i = 0; i < data.learnableSkills.Length; i++)
                {
                    PetData.LearnableSkill learnableSkill = data.learnableSkills[i];
                    if (learnableSkill != null && learnableSkill.skillData != null && learnableSkill.level <= level)
                    {
                        availableSkills.Add(learnableSkill.skillData);
                        if (availableSkills.Count >= 4)
                        {
                            return availableSkills;
                        }
                    }
                }
            }

            if (data.skillPool != null)
            {
                for (int i = 0; i < data.skillPool.Length; i++)
                {
                    SkillData skillData = data.skillPool[i];
                    if (skillData == null || availableSkills.Contains(skillData))
                    {
                        continue;
                    }

                    availableSkills.Add(skillData);
                    if (availableSkills.Count >= 4)
                    {
                        return availableSkills;
                    }
                }
            }

            return availableSkills;
        }

        private SkillInstance[] CloneLoadout(SkillInstance[] source)
        {
            if (source == null || source.Length == 0)
            {
                return null;
            }

            SkillInstance[] clone = new SkillInstance[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != null && source[i].skillData != null)
                {
                    clone[i] = new SkillInstance(source[i].skillData, source[i].currentPP);
                }
            }

            return clone;
        }
    }

    [System.Serializable]
    public class PetInstanceSaveData
    {
        public string instanceId;
        public int level;
        public int currentExp;
        public int currentHP;
        public PetDataSaveData petData;
        public SkillDataSaveData[] skills;
    }

    [System.Serializable]
    public class PetDataSaveData
    {
        public string petName;
        public string description;
        public PetType primaryType;
        public int maxLevelForEvolution;
        public int maxHP;
        public int attack;
        public int defense;
        public SkillDataSaveData[] skillPool;
        public LearnableSkillSaveData[] learnableSkills;

        public static PetDataSaveData FromPetData(PetData petData)
        {
            if (petData == null)
            {
                return null;
            }

            return new PetDataSaveData
            {
                petName = petData.petName,
                description = petData.description,
                primaryType = petData.primaryType,
                maxLevelForEvolution = petData.maxLevelForEvolution,
                maxHP = petData.maxHP,
                attack = petData.attack,
                defense = petData.defense,
                skillPool = SkillDataSaveData.FromSkillDataArray(petData.skillPool),
                learnableSkills = LearnableSkillSaveData.FromLearnableSkillArray(petData.learnableSkills)
            };
        }

        public PetData ToPetData()
        {
            PetData data = ScriptableObject.CreateInstance<PetData>();
            data.petName = petName;
            data.description = description;
            data.primaryType = primaryType;
            data.maxLevelForEvolution = maxLevelForEvolution;
            data.maxHP = maxHP;
            data.attack = attack;
            data.defense = defense;
            data.skillPool = SkillDataSaveData.ToSkillDataArray(skillPool);
            data.learnableSkills = LearnableSkillSaveData.ToLearnableSkillArray(learnableSkills);
            return data;
        }
    }

    [System.Serializable]
    public class SkillDataSaveData
    {
        public string skillName;
        public string description;
        public int power;
        public PetType skillType;
        public int pp;
        public int currentPP;

        public static SkillDataSaveData FromSkillData(SkillData skillData)
        {
            if (skillData == null)
            {
                return null;
            }

            return new SkillDataSaveData
            {
                skillName = skillData.skillName,
                description = skillData.description,
                power = skillData.power,
                skillType = skillData.skillType,
                pp = skillData.pp,
                currentPP = skillData.pp
            };
        }

        public SkillData ToSkillData()
        {
            SkillData data = ScriptableObject.CreateInstance<SkillData>();
            data.skillName = skillName;
            data.description = description;
            data.power = power;
            data.skillType = skillType;
            data.pp = pp;
            return data;
        }

        public static SkillDataSaveData[] FromSkillDataArray(SkillData[] skillDataArray)
        {
            if (skillDataArray == null)
            {
                return null;
            }

            SkillDataSaveData[] result = new SkillDataSaveData[skillDataArray.Length];
            for (int i = 0; i < skillDataArray.Length; i++)
            {
                result[i] = FromSkillData(skillDataArray[i]);
            }

            return result;
        }

        public static SkillData[] ToSkillDataArray(SkillDataSaveData[] saveDataArray)
        {
            if (saveDataArray == null)
            {
                return null;
            }

            SkillData[] result = new SkillData[saveDataArray.Length];
            for (int i = 0; i < saveDataArray.Length; i++)
            {
                result[i] = saveDataArray[i] != null ? saveDataArray[i].ToSkillData() : null;
            }

            return result;
        }

        public static SkillDataSaveData[] FromSkillInstances(SkillInstance[] skillInstances)
        {
            if (skillInstances == null)
            {
                return null;
            }

            SkillDataSaveData[] result = new SkillDataSaveData[skillInstances.Length];
            for (int i = 0; i < skillInstances.Length; i++)
            {
                if (skillInstances[i] != null && skillInstances[i].skillData != null)
                {
                    result[i] = FromSkillData(skillInstances[i].skillData);
                    result[i].currentPP = skillInstances[i].currentPP;
                }
            }

            return result;
        }

        public static SkillInstance[] ToSkillInstances(SkillDataSaveData[] saveDataArray)
        {
            if (saveDataArray == null)
            {
                return null;
            }

            SkillInstance[] result = new SkillInstance[saveDataArray.Length];
            for (int i = 0; i < saveDataArray.Length; i++)
            {
                if (saveDataArray[i] != null)
                {
                    SkillData data = saveDataArray[i].ToSkillData();
                    result[i] = new SkillInstance(data, saveDataArray[i].currentPP);
                }
            }

            return result;
        }
    }

    [System.Serializable]
    public class LearnableSkillSaveData
    {
        public SkillDataSaveData skillData;
        public int level;

        public static LearnableSkillSaveData[] FromLearnableSkillArray(PetData.LearnableSkill[] learnableSkills)
        {
            if (learnableSkills == null)
            {
                return null;
            }

            LearnableSkillSaveData[] result = new LearnableSkillSaveData[learnableSkills.Length];
            for (int i = 0; i < learnableSkills.Length; i++)
            {
                if (learnableSkills[i] != null)
                {
                    result[i] = new LearnableSkillSaveData
                    {
                        skillData = SkillDataSaveData.FromSkillData(learnableSkills[i].skillData),
                        level = learnableSkills[i].level
                    };
                }
            }

            return result;
        }

        public static PetData.LearnableSkill[] ToLearnableSkillArray(LearnableSkillSaveData[] saveData)
        {
            if (saveData == null)
            {
                return null;
            }

            PetData.LearnableSkill[] result = new PetData.LearnableSkill[saveData.Length];
            for (int i = 0; i < saveData.Length; i++)
            {
                if (saveData[i] != null)
                {
                    result[i] = new PetData.LearnableSkill
                    {
                        skillData = saveData[i].skillData != null ? saveData[i].skillData.ToSkillData() : null,
                        level = saveData[i].level
                    };
                }
            }

            return result;
        }
    }
}
