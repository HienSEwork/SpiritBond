using SpiritBond.Core;
using SpiritBond.Battle;
using SpiritBond.Pet;
using SpiritBond.Skill;
using SpiritBond.World.Encounter;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleHud playerHud;
    [SerializeField] private BattleUnit enemyUnit;
    [SerializeField] private BattleHud enemyHud;
    [SerializeField] private BattleDialogBox dialogBox;

    private bool battleEnded;

    private void Start()
    {
        SetupBattle();
    }

    public void SetupBattle()
    {
        PlayerProgressState progressState = SaveGameService.LoadOrCreate();
        if (playerUnit == null || enemyUnit == null || EncounterManager.Instance == null || progressState == null)
        {
            Debug.LogWarning("[BattleSystem] SetupBattle failed because references are missing.");
            return;
        }

        PetInstance playerPetInstance = progressState.GetPrimaryBattlePet();
        if (playerPetInstance == null)
        {
            Debug.LogWarning("[BattleSystem] SetupBattle failed because primary team pet is missing.");
            battleEnded = true;
            EncounterManager.Instance.ClearPendingEncounter();
            SaveGameService.ReturnToLatestCheckpoint();
            return;
        }

        PetData enemyPetData = EncounterManager.Instance.ConsumePendingEnemyPetData();
        int enemyLevel = EncounterManager.Instance.ConsumePendingEnemyLevel();
        if (enemyPetData == null)
        {
            Debug.LogWarning("[BattleSystem] SetupBattle failed because pending enemy pet is null.");
            battleEnded = true;
            EncounterManager.Instance.ClearPendingEncounter();
            SaveGameService.ReturnToLatestCheckpoint();
            return;
        }

        playerUnit.Setup(playerPetInstance);
        enemyUnit.Setup(new PetInstance(enemyPetData, enemyLevel));

        if (playerHud != null)
        {
            playerHud.SetData(playerUnit.PetInstance);
        }

        if (enemyHud != null)
        {
            enemyHud.SetData(enemyUnit.PetInstance);
        }

        if (dialogBox != null && enemyUnit.PetInstance != null && enemyUnit.PetInstance.petData != null)
        {
            dialogBox.SetDialog("A wild " + enemyUnit.PetInstance.petData.petName + " Lv" + enemyUnit.PetInstance.level + " appeared.");
        }
    }

    public void PlayerUseSkill(int index)
    {
        if (battleEnded || playerUnit == null || enemyUnit == null || playerUnit.PetInstance == null || enemyUnit.PetInstance == null)
        {
            Debug.LogWarning("[BattleSystem] PlayerUseSkill blocked because battle state is invalid.");
            return;
        }

        if (!playerUnit.PetInstance.CanBattle)
        {
            EndBattle(false);
            return;
        }

        SkillInstance skill = GetSkillAt(playerUnit.PetInstance.skills, index);
        if (skill == null || skill.skillData == null)
        {
            Debug.LogWarning($"[BattleSystem] Player skill at index {index} is invalid.");
            return;
        }

        int damage = BattleCalculator.CalculateDamage(playerUnit.PetInstance, enemyUnit.PetInstance, skill);
        enemyUnit.TakeDamage(damage);
        RefreshHud();

        if (dialogBox != null && playerUnit.PetInstance.petData != null)
        {
            dialogBox.SetDialog(playerUnit.PetInstance.petData.petName + " used " + skill.skillData.skillName + "!");
        }

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
            return;
        }

        SkillInstance skill = GetRandomSkill(enemyUnit.PetInstance.skills);
        if (skill == null || skill.skillData == null)
        {
            Debug.LogWarning("[BattleSystem] Enemy does not have a valid skill.");
            return;
        }

        int damage = BattleCalculator.CalculateDamage(enemyUnit.PetInstance, playerUnit.PetInstance, skill);
        playerUnit.TakeDamage(damage);
        RefreshHud();

        if (dialogBox != null && enemyUnit.PetInstance.petData != null)
        {
            dialogBox.SetDialog(enemyUnit.PetInstance.petData.petName + " used " + skill.skillData.skillName + "!");
        }

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
            Debug.LogWarning("[BattleSystem] EndBattle aborted because progress state is missing.");
            return;
        }

        if (playerWon && playerUnit != null && enemyUnit != null && playerUnit.PetInstance != null)
        {
            int expReward = BattleCalculator.CalculateExpReward(enemyUnit.PetInstance);
            bool leveledUp = playerUnit.PetInstance.AddExp(expReward);

            if (dialogBox != null && playerUnit.PetInstance.petData != null)
            {
                dialogBox.SetDialog(playerUnit.PetInstance.petData.petName + " won and gained " + expReward + " EXP!");
            }

            Debug.Log($"[BattleSystem] Player won and gained {expReward} EXP. LeveledUp={leveledUp}");
        }
        else if (dialogBox != null)
        {
            dialogBox.SetDialog("You lost the battle.");
        }

        progressState.RestoreBattleTeamToFullHealth();
        SaveGameService.Save();

        if (EncounterManager.Instance != null)
        {
            EncounterManager.Instance.ClearPendingEncounter();
        }

        SaveGameService.ReturnToLatestCheckpoint();
    }

    private void RefreshHud()
    {
        if (playerHud != null)
        {
            playerHud.RefreshHP(playerUnit.PetInstance);
        }

        if (enemyHud != null)
        {
            enemyHud.RefreshHP(enemyUnit.PetInstance);
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

        int validCount = 0;
        for (int i = 0; i < skillSet.Length; i++)
        {
            if (skillSet[i] != null && skillSet[i].skillData != null)
            {
                validCount++;
            }
        }

        if (validCount == 0)
        {
            return null;
        }

        int targetIndex = Random.Range(0, validCount);
        for (int i = 0; i < skillSet.Length; i++)
        {
            if (skillSet[i] != null && skillSet[i].skillData != null)
            {
                if (targetIndex == 0)
                {
                    return skillSet[i];
                }

                targetIndex--;
            }
        }

        return null;
    }
}
