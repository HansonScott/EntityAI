﻿using System;

namespace EntityAI
{
    public class EntityObject
    {
        public Position Position;

        public bool Visibility
        {
            get; set;
        }
        public string Description { get; internal set; }
    }
}