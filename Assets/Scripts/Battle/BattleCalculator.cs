using SpiritBond.Pet;
using SpiritBond.Skill;
using UnityEngine;

namespace SpiritBond.Battle
{
    public static class BattleCalculator
    {
        public static int CalculateDamage(PetInstance attacker, PetInstance defender, SkillInstance skill)
        {
            if (attacker == null || defender == null || skill == null || skill.skillData == null)
            {
                return 0;
            }

            int attackerLevel = Mathf.Max(PetProgression.MinLevel, attacker.level);
            int defenderLevel = Mathf.Max(PetProgression.MinLevel, defender.level);
            int attack = attacker.petData != null ? attacker.petData.attack + attackerLevel : attackerLevel;
            int defense = defender.petData != null ? defender.petData.defense + defenderLevel : defenderLevel;
            int power = Mathf.Max(1, skill.skillData.power);

            int baseDamage = (attack * power) / (defense + 1);
            return Mathf.Max(1, baseDamage);
        }

        public static int CalculateExpReward(PetInstance defeatedPet)
        {
            if (defeatedPet == null)
            {
                return PetProgression.BaseExpToLevelUp;
            }

            int defeatedLevel = Mathf.Max(PetProgression.MinLevel, defeatedPet.level);
            int attack = defeatedPet.petData != null ? defeatedPet.petData.attack : 0;
            int defense = defeatedPet.petData != null ? defeatedPet.petData.defense : 0;

            return Mathf.Max(PetProgression.BaseExpToLevelUp, (defeatedLevel * 10) + ((attack + defense) / 2));
        }
    }
}
