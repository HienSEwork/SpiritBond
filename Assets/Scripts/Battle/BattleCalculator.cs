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

            int attack = attacker.petData != null ? attacker.petData.attack : 0;
            int defense = defender.petData != null ? defender.petData.defense : 0;
            int power = skill.skillData.power;

            return Mathf.Max(0, (attack * power) / (defense + 1));
        }
    }
}
