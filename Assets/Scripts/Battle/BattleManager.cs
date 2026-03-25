using System.Collections.Generic;
using SpiritBond.Core;
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

        private bool battleEnded;

        private void Start()
        {
            Debug.Log("[BattleManager] Battle scene started.");
            SetupBattle();
        }

        private void SetupBattle()
        {
            PlayerProgressState progressState = SaveGameService.LoadOrCreate();
            if (playerUnit == null || enemyUnit == null || EncounterManager.Instance == null || progressState == null)
            {
                Debug.LogWarning("[BattleManager] SetupBattle failed because references are missing.");
                return;
            }

            PetInstance playerPetInstance = progressState.GetPrimaryBattlePet();
            if (playerPetInstance == null)
            {
                Debug.LogWarning("[BattleManager] SetupBattle failed because primary team pet is missing.");
                battleEnded = true;
                if (EncounterManager.Instance != null)
                {
                    EncounterManager.Instance.ClearPendingEncounter();
                }
                SaveGameService.ReturnToLatestCheckpoint();
                return;
            }

            PetData enemyPetData = EncounterManager.Instance.ConsumePendingEnemyPetData();
            if (enemyPetData == null)
            {
                Debug.LogWarning("[BattleManager] SetupBattle failed because pending enemy pet is null.");
                battleEnded = true;
                if (EncounterManager.Instance != null)
                {
                    EncounterManager.Instance.ClearPendingEncounter();
                }
                SaveGameService.ReturnToLatestCheckpoint();
                return;
            }

            Debug.Log($"[BattleManager] Setup battle Player={playerPetInstance.petData.petName} Enemy={enemyPetData.petName}");
            playerUnit.Setup(playerPetInstance);
            enemyUnit.Setup(new PetInstance(enemyPetData));
        }

        public void PlayerUseSkill(int index)
        {
            if (battleEnded || playerUnit == null || enemyUnit == null || playerUnit.PetInstance == null || enemyUnit.PetInstance == null)
            {
                Debug.LogWarning("[BattleManager] PlayerUseSkill blocked because battle state is invalid.");
                return;
            }

            if (!playerUnit.PetInstance.CanBattle)
            {
                Debug.LogWarning("[BattleManager] PlayerUseSkill blocked because active pet cannot battle.");
                EndBattle(false);
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
                EndBattle(true);
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
                EndBattle(false);
            }
        }

        private void EndBattle(bool playerWon)
        {
            if (battleEnded)
            {
                return;
            }

            battleEnded = true;

            PlayerProgressState progressState = SaveGameService.LoadOrCreate();
            if (progressState == null)
            {
                Debug.LogWarning("[BattleManager] EndBattle aborted because progress state is missing.");
                return;
            }

            if (playerWon && playerUnit != null && enemyUnit != null && playerUnit.PetInstance != null)
            {
                int expReward = BattleCalculator.CalculateExpReward(enemyUnit != null ? enemyUnit.PetInstance : null);
                bool leveledUp = playerUnit.PetInstance.AddExp(expReward);
                Debug.Log($"[BattleManager] Player won and gained {expReward} EXP. LeveledUp={leveledUp}");
            }
            else
            {
                Debug.Log("[BattleManager] Player lost the battle.");
            }

            progressState.RestoreBattleTeamToFullHealth();
            SaveGameService.Save();

            if (EncounterManager.Instance != null)
            {
                EncounterManager.Instance.ClearPendingEncounter();
            }

            SaveGameService.ReturnToLatestCheckpoint();
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
