namespace EntityAI
{
    public class AbilityNeed : EntityNeed
    {
        public Ability Ability;

        public override string Name
        {
            get { return this.Ability.Name; }
        }

        public AbilityNeed(Ability a)
        {
            this.Ability = a;
        }
    }
}