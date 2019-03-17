using System;

namespace EntityAI
{
    public class Position
    {
        public double X;
        public double Y;
        public double Z;

        public double Longitude
        {
            get { return X; }
            set { X = value; }
        }

        public double Latitude
        {
            get { return Y; }
            set { Y = value; }
        }

        public double Altitude
        {
            get { return Z; }
            set { Z = value; }
        }

        public Position(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        internal double DistanceFrom(object position)
        {
            throw new NotImplementedException();
        }

        public Position()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        internal double DistanceFrom(Position origin)
        {
            // result = SQRT ( (x2 - x1)^2 + (y2 - y1)^2 + (z2 - z1)^2 )

            double deltaX = (origin.X - this.X)*(origin.X - this.X);
            double deltaY = (origin.Y - this.Y) * (origin.Y - this.Y);
            double deltaZ = (origin.Y - this.Z) * (origin.Z - this.Z);
            double deltas = deltaX + deltaY + deltaZ;
            return Math.Sqrt(deltas);
        }
    }
}