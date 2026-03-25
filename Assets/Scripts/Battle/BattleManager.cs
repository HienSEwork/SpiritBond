using System.Collections.Generic;
using SpiritBond.Pet;
using SpiritBond.Skill;
using SpiritBond.World.Encounter;
using UnityEngine;

namespace SpiritBond.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleUnit playerUnit; // Assign in Inspector
        [SerializeField] private BattleUnit enemyUnit; // Assign in Inspector
        [SerializeField] private PetData playerPetData; // Assign in Inspector

        private bool battleEnded;

        private void Start()
        {
            Debug.Log("[BattleManager] Battle scene started.");
            SetupBattle();
        }

        private void SetupBattle()
        {
            if (playerUnit == null || enemyUnit == null || playerPetData == null || EncounterManager.Instance == null)
            {
                Debug.LogWarning("[BattleManager] SetupBattle failed because references are missing.");
                return;
            }

            PetData enemyPetData = EncounterManager.Instance.PendingEnemyPetData;
            if (enemyPetData == null)
            {
                Debug.LogWarning("[BattleManager] SetupBattle failed because pending enemy pet is null.");
                return;
            }

            Debug.Log($"[BattleManager] Setup battle Player={playerPetData.petName} Enemy={enemyPetData.petName}");
            playerUnit.Setup(new PetInstance(playerPetData));
            enemyUnit.Setup(new PetInstance(enemyPetData));
            EncounterManager.Instance.ClearPendingEncounter();
        }

        public void PlayerUseSkill(int index)
        {
            if (battleEnded || playerUnit == null || enemyUnit == null || playerUnit.PetInstance == null || enemyUnit.PetInstance == null)
            {
                Debug.LogWarning("[BattleManager] PlayerUseSkill blocked because battle state is invalid.");
                return;
            }

            SkillInstance skill = GetSkillAt(playerUnit.PetInstance.skills, index);
            if (skill == null || skill.skillData == null)
            {
                Debug.LogWarning($"[BattleManager] Player skill at index {index} is invalid.");
                return;
            }

            int damage = BattleCalculator.CalculateDamage(playerUnit.PetInstance, enemyUnit.PetInstance, skill);
            Debug.Log($"[BattleManager] Player used {skill.skillData.skillName} and dealt {damage} damage.");
            enemyUnit.TakeDamage(damage);

            if (enemyUnit.PetInstance.IsFainted)
            {
                battleEnded = true;
                Debug.Log("[BattleManager] Enemy fainted. Battle ended.");
                return;
            }

            EnemyTurn();
        }

        public void EnemyTurn()
        {
            if (battleEnded || playerUnit == null || enemyUnit == null || playerUnit.PetInstance == null || enemyUnit.PetInstance == null)
            {
                Debug.LogWarning("[BattleManager] EnemyTurn blocked because battle state is invalid.");
                return;
            }

            SkillInstance skill = GetRandomSkill(enemyUnit.PetInstance.skills);
            if (skill == null || skill.skillData == null)
            {
                Debug.LogWarning("[BattleManager] Enemy does not have a valid skill.");
                return;
            }

            int damage = BattleCalculator.CalculateDamage(enemyUnit.PetInstance, playerUnit.PetInstance, skill);
            Debug.Log($"[BattleManager] Enemy used {skill.skillData.skillName} and dealt {damage} damage.");
            playerUnit.TakeDamage(damage);

            if (playerUnit.PetInstance.IsFainted)
            {
                battleEnded = true;
                Debug.Log("[BattleManager] Player fainted. Battle ended.");
            }
        }

        private SkillInstance GetSkillAt(SkillInstance[] skillSet, int index)
        {
            if (skillSet == null || index < 0 || index >= skillSet.Length)
            {
                return null;
            }

            return skillSet[index];
        }

        private SkillInstance GetRandomSkill(SkillInstance[] skillSet)
        {
            if (skillSet == null || skillSet.Length == 0)
            {
                return null;
            }

            List<SkillInstance> validSkills = new List<SkillInstance>();
            for (int i = 0; i < skillSet.Length; i++)
            {
                if (skillSet[i] != null && skillSet[i].skillData != null)
                {
                    validSkills.Add(skillSet[i]);
                }
            }

            if (validSkills.Count == 0)
            {
                return null;
            }

            return validSkills[Random.Range(0, validSkills.Count)];
        }
    }
}
