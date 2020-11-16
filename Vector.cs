using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// The vector posistion class.
    /// </summary>
    class Vector
    {
        private float[] position = new float[3];
        public Vector(float X, float Y, float Z)
        {
            position[0] = X;
            position[1] = Y;
            position[2] = Z;
        }
        public Vector(MapInformation mapInformation, IHelper helper)
        {
            position[0] = helper.GenerateRandomNumber(0, mapInformation.mapSize.width - 1);
            position[1] = helper.GenerateRandomNumber(0, mapInformation.mapSize.height - 1);
            position[2] = 0;
        }
        public float X { get => position[0]; set => position[0] = value; }
        public float Y { get => position[1]; set => position[1] = value; }
        public float Z { get => position[2]; set => position[2] = value; }

        public float DistanceBetweenVectors(Vector otherVector)
        {
            return (float)Math.Sqrt(Math.Pow(Math.Abs(X - otherVector.X), 2) + Math.Pow(Math.Abs(Y - otherVector.Y), 2) + Math.Pow(Math.Abs(Z - otherVector.Z), 2));
        }
        public static Vector Copy(Vector vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z);
        }
        public static bool Compare(Vector vector1, Vector vector2)
        {
            if (vector1.X == vector2.X && vector1.Y == vector2.Y && vector1.Z == vector2.Z)
                return true;
            return false;
        }
    }
}
