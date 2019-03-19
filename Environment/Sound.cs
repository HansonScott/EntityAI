using System;

namespace EntityAI
{
    public class Sound
    {
        public const double HearingThreashhold = 0.8; // only things that are X% as loud as the loudest thing are noticed
        public const int SoundCountCausingAmbiance = 6;

        public enum RecognitionFootPrint
        {
            Unknown = 0,

            Water = 1,
            Wind = 2,
            Fire = 3,

            Footfall = 10,

            Animal_Call = 20,
        }

        /// <summary>
        /// the Unique, recognizable sound pattern
        /// </summary>
        public RecognitionFootPrint FootPrint;

        /// <summary>
        /// decibals
        /// </summary>
        public double Loudness;

        public Position Origin;

        public string Description
        {
            get
            {
                string thing = Enum.GetName(typeof(RecognitionFootPrint), FootPrint);

                // add adjective for distance or loudness, or distance?

                return thing;
            }
        }

        public Sound(RecognitionFootPrint FootPrint, double Loudness, Position Origin)
        {
            this.FootPrint = FootPrint;
            this.Loudness = Loudness;
            this.Origin = Origin;
        }

        internal bool IsHeard(double effectiveness_Current, Position p, double ambientLoudness)
        {
            // take into account distance and loudness

            double dist = p.DistanceFrom(this.Origin);
            double distanceLoudness = AdjustLoudnessForDistance(this.Loudness, dist);
            double perceivedLoudness = distanceLoudness * effectiveness_Current; // adjust by percentage

            return (perceivedLoudness >= (ambientLoudness * HearingThreashhold)); // only if it is perceived relative to the ambient noise.
        }

        private double AdjustLoudnessForDistance(double loudness, double dist)
        {
            // complex calculation, can be found at this site: http://www.sengpielaudio.com/calculator-distance.htm
            // in general, assume 5 ft to be the reference point of full loudness, then adjust for distance.
            // table of common values:
            /*
             * dist = perceived loudness
             * 5 = 50 - base (100%)
             * 10 = 44
             * 15 = 41 - 3x dist =~ 80%
             * 20 = 38
             * 25 = 36 - 5x dist = 70% 
             * 30 = 34
             * 40 = 32
             * 50 = 30 - 10x dist = 60%
             * 60 = 28
             * 70 = 27
             * 80 = 26
             * 90 = 25
             * 100 = 24 - 20x dist =~ 50%
             * 125 = 22
             * 150 = 20 - 30x dist = ~40%
             * 175 = 19
             * 200 = 18
             * 250 = 16
             * 300 = 15 - 60x dist = 30%
             * 350 = 13
             * 400 = 12
             * 500 = 10 - 100x dist = 20%
             * 750 = 7
             * 1000 = 4 - 200x dist =~ 10%
             */

            // else, calcuate
            if (dist / 5 > 200) { return loudness * .1; }
            else if (dist / 5 > 100) { return loudness * .2; }
            else if (dist / 5 > 60) { return loudness * .3; }
            else if (dist / 5 > 30) { return loudness * .4; }
            else if (dist / 5 > 20) { return loudness * .5; }
            else if (dist / 5 > 10) { return loudness * .6; }
            else if (dist / 5 > 5) { return loudness * .7; }
            else if (dist / 5 > 3) { return loudness * .8; }
            else if (dist / 5 > 1) { return loudness * .9; }
            else return loudness;
        }
    }
}