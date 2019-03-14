using System;

namespace EntityAI
{
    /// <summary>
    /// represents the quantified positive effect of a solution
    /// </summary>
    public class SolutionBenefit
    {
        Type ObjectTypeAffected;
        object ObjectTypeCategoryAffected;

        double ValueChange;
    }
}