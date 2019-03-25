using System;

namespace EntityAI
{
    /// <summary>
    /// Represents a possible change type to performed by the entity as an action
    /// </summary>
    public class Ability: EntityAttribute
    {
        public enum AbilityType
        {
            unknown = 0,

            // within self
            Wake = 1,
            Sleep = 2,

            Stand = 10,
            Crouch = 11,
            Sit = 12,
            Lie_Down = 13,

            Walk = 20,
            Run = 21,
            Duck = 22,
            Jump = 23,

            // with senses
            Use_Sensor = 25,

            // with object
            Pick_Up = 30,
            Carry = 31,
            Place = 32,
            Drop = 33,
            Throw = 34,

            Don = 40,
            Doff = 41,

            Use = 50,

            // with other entity
            Greet = 100,
            Speak_Statement = 110,
            Speak_Inquire = 120,

            Speak_Intimidate = 200,
            Speak_Persuade = 210,
            Speak_Deceive = 220,
        }

        public AbilityType AType;
        public double Effectiveness_Current;

        public string Name
        {
            get
            {
                return Enum.GetName(typeof(AbilityType), AType);
            }
        }

        public Ability(): this(AbilityType.unknown){}

        public Ability(AbilityType T)
        {
            this.AType = T;
        }
    }
}