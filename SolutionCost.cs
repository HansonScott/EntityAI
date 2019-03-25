using System;

namespace EntityAI
{
    public class SolutionCost
    {
        public Type ObjectTypeAffected;
        public object ObjectTypeCategoryAffected;

        public double ValueChange;

        public SolutionCost(Type t, object o, double change)
        {
            ObjectTypeAffected = t;
            ObjectTypeCategoryAffected = o;
            this.ValueChange = change;
        }
    }
}