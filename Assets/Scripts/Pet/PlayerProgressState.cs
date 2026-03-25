using System.Collections.Generic;

namespace SpiritBond.Pet
{
    [System.Serializable]
    public class PlayerProgressState
    {
        public const int BattleTeamSize = 3;

        private readonly List<PetInstance> allOwnedPets = new List<PetInstance>();
        private readonly string[] battleTeamInstanceIds = new string[BattleTeamSize];

        public IReadOnlyList<PetInstance> AllOwnedPets => allOwnedPets;

        public PetInstance ResolvePet(string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
            {
                return null;
            }

            for (int i = 0; i < allOwnedPets.Count; i++)
            {
                if (allOwnedPets[i] != null && allOwnedPets[i].instanceId == instanceId)
                {
                    return allOwnedPets[i];
                }
            }

            return null;
        }

        public void AddOwnedPet(PetInstance petInstance)
        {
            if (petInstance == null)
            {
                return;
            }

            petInstance.EnsureInstanceId();

            if (ResolvePet(petInstance.instanceId) != null)
            {
                return;
            }

            allOwnedPets.Add(petInstance);
        }

        public void SetBattleTeamSlot(int slotIndex, string instanceId)
        {
            if (slotIndex < 0 || slotIndex >= battleTeamInstanceIds.Length)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(instanceId) && ResolvePet(instanceId) == null)
            {
                return;
            }

            battleTeamInstanceIds[slotIndex] = string.IsNullOrWhiteSpace(instanceId) ? null : instanceId;
        }

        public PetInstance GetBattleTeamPet(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= battleTeamInstanceIds.Length)
            {
                return null;
            }

            return ResolvePet(battleTeamInstanceIds[slotIndex]);
        }

        public PetInstance GetPrimaryBattlePet()
        {
            return GetBattleTeamPet(0);
        }

        public string[] GetBattleTeamSnapshot()
        {
            string[] snapshot = new string[battleTeamInstanceIds.Length];

            for (int i = 0; i < battleTeamInstanceIds.Length; i++)
            {
                snapshot[i] = battleTeamInstanceIds[i];
            }

            return snapshot;
        }

        public void RestoreBattleTeamToFullHealth()
        {
            for (int i = 0; i < battleTeamInstanceIds.Length; i++)
            {
                PetInstance petInstance = GetBattleTeamPet(i);
                petInstance?.RestoreToFullHealth();
            }
        }
    }
}
