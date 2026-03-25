namespace SpiritBond.Skill
{
    [System.Serializable]
    public class SkillInstance
    {
        public SkillData skillData;
        public int currentPP;

        public SkillInstance(SkillData data)
        {
            skillData = data;
            currentPP = data != null ? data.pp : 0;
        }

        public SkillInstance(SkillData data, int savedCurrentPP)
        {
            skillData = data;
            currentPP = data != null ? UnityEngine.Mathf.Clamp(savedCurrentPP, 0, data.pp) : 0;
        }
    }
}
