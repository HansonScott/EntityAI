namespace EntityAI
{
    public class AbilityNeed : EntityNeed
    {
        public Ability Ability;

        public AbilityNeed(Ability a)
        {
            this.Ability = a;
        }
    }
}