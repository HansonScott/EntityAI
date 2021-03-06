﻿using System;

namespace EntityAI
{
    /// <summary>
    /// represents the quantified positive effect of a solution
    /// </summary>
    public class SolutionBenefit
    {
        public Type ObjectTypeAffected;
        public object ObjectTypeCategoryAffected;

        public double ValueChange;

        public SolutionBenefit(Type t, object o, double change)
        {
            ObjectTypeAffected = t;
            ObjectTypeCategoryAffected = o;
            this.ValueChange = change;
        }
    }
}