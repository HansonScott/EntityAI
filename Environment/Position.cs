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

        internal Position GetNewPositionForSpeedToTarget(Position pTarget, double speed)
        {
            // find the angle we want to go
            // adjust the entity's position by the change
            double changeX = pTarget.X - X;
            double changeY = pTarget.Y - Y;
            double changeZ = pTarget.Z - Z;

            // future: apply the distance cos/sin to get the distance on each coordinate
            // apply the change on each axis to the current position (move towards target)

            // for now, just use the speed
            double distX = 0;
            double distY = 0;
            double distZ = 0;

            // don't overshoot,
            if (changeX > 0) { distX = Math.Min(speed, changeX); } else { distX = Math.Max(-speed, changeX); }
            if (changeY > 0) { distY = Math.Min(speed, changeY); } else { distY = Math.Max(-speed, changeY); }
            if (changeZ > 0) { distZ = Math.Min(speed, changeZ); } else { distZ = Math.Max(-speed, changeZ); }

            // return the new postion
            Position result = new Position(X + changeX, Y + changeY, Z + changeZ);
            return result;
        }
    }
}