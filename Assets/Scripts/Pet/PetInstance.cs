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
                    clone[i] = new SkillInstance(source[i].skillData);
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
        public PetType primaryType;
        public int maxLevelForEvolution;
        public int maxHP;
        public int attack;
        public int defense;
        public SkillDataSaveData[] skillPool;

        public static PetDataSaveData FromPetData(PetData petData)
        {
            if (petData == null)
            {
                return null;
            }

            return new PetDataSaveData
            {
                petName = petData.petName,
                primaryType = petData.primaryType,
                maxLevelForEvolution = petData.maxLevelForEvolution,
                maxHP = petData.maxHP,
                attack = petData.attack,
                defense = petData.defense,
                skillPool = SkillDataSaveData.FromSkillDataArray(petData.skillPool)
            };
        }

        public PetData ToPetData()
        {
            PetData data = ScriptableObject.CreateInstance<PetData>();
            data.petName = petName;
            data.primaryType = primaryType;
            data.maxLevelForEvolution = maxLevelForEvolution;
            data.maxHP = maxHP;
            data.attack = attack;
            data.defense = defense;
            data.skillPool = SkillDataSaveData.ToSkillDataArray(skillPool);
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
                skillType = skillData.skillType
            };
        }

        public SkillData ToSkillData()
        {
            SkillData data = ScriptableObject.CreateInstance<SkillData>();
            data.skillName = skillName;
            data.description = description;
            data.power = power;
            data.skillType = skillType;
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
                result[i] = skillInstances[i] != null ? FromSkillData(skillInstances[i].skillData) : null;
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
                    result[i] = new SkillInstance(saveDataArray[i].ToSkillData());
                }
            }

            return result;
        }
    }
}
