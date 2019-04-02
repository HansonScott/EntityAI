using System;

namespace EntityAI
{
    public class EntityResource: EntityObject
    {
        public enum ResourceType
        {
            Unknown = 0,
            Air = 1,
            Water = 2,

            Container = 10,
        }

        public ResourceType RType;

        public EntityResource(ResourceType t, Position P): base(GetAppearanceForType(t), GetSoundForType(t), P)
        {
            this.RType = t;
        }

        private static Sound.RecognitionFootPrint GetSoundForType(ResourceType t)
        {
            switch(t)
            {
                case ResourceType.Air:
                    return EntityAI.Sound.RecognitionFootPrint.Wind;
                case ResourceType.Water:
                    return EntityAI.Sound.RecognitionFootPrint.Water;
                case ResourceType.Container:
                    return EntityAI.Sound.RecognitionFootPrint.Container;
                case ResourceType.Unknown:
                default:
                    return EntityAI.Sound.RecognitionFootPrint.Unknown;
            }
        }
        private static Sight.RecognitionFootPrint GetAppearanceForType(ResourceType t)
        {
            switch(t)
            {
                case ResourceType.Air:
                    return Sight.RecognitionFootPrint.Wind;
                case ResourceType.Water:
                    return Sight.RecognitionFootPrint.Water;
                case ResourceType.Container:
                    return Sight.RecognitionFootPrint.Container;
                case ResourceType.Unknown:
                default:
                    return Sight.RecognitionFootPrint.Unknown;
            }
        }

        internal bool IsConsumedOnUse()
        {
            switch(this.RType)
            {
                case ResourceType.Air:
                case ResourceType.Water:
                    return true;
                default:
                    return false;
            }
        }
        internal bool IsContainer()
        {
            switch(this.RType)
            {
                case ResourceType.Container:
                    return true;
                default:
                    return false;
            }
        }
        internal bool RequiresContainer()
        {
            // database lookup?
            switch(this.RType)
            {
                case ResourceType.Air:
                case ResourceType.Water:
                    return true;
                default:
                    return false;
            }
        }
    }
}