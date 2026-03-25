namespace SpiritBond.Skill
{
    [System.Serializable]
    public class SkillInstance
    {
        public SkillData skillData;

        public SkillInstance(SkillData data)
        {
            skillData = data;
        }
    }
}
