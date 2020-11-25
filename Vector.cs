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
        /// <summary>
        /// Creates an instance of the Vector class with a posistion of (<paramref name="X"/>, <paramref name="Y"/>, and <paramref name="Z"/>).
        /// </summary>
        /// <param name="X">The X posistion.</param>
        /// <param name="Y">The y posistion.</param>
        /// <param name="Z">The z posistion.</param>
        public Vector(float X, float Y, float Z)
        {
            position[0] = X;
            position[1] = Y;
            position[2] = Z;
        }
        /// <summary>
        /// Creates an instance of the Vector class with a random location on the map.
        /// </summary>
        /// <param name="mapInformation"></param>
        /// <param name="helper"></param>
        public Vector(MapInformation mapInformation, IHelper helper)
        {
            position[0] = helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.width - 1);
            position[1] = helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.height - 1);
            position[2] = 0;
        }
        /// <summary>
        /// Creates an instance of the Vector class with a random location on the map.
        /// </summary>
        /// <param name="mapInformation"></param>
        /// <param name="helper"></param>
        public Vector(MapInformation mapInformation, IHelper helper, float maxHeight)
        {
            position[0] = helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.width - 1);
            position[1] = helper.GenerateRandomNumber(0, mapInformation.GetSizeOfMap.height - 1);
            position[2] = helper.GenerateRandomNumber(0, (int)maxHeight);
        }
        /// <summary>
        /// Get or set the X position.
        /// </summary>
        public float X { get => position[0]; set => position[0] = value; }
        /// <summary>
        /// Get or set the Y position.
        /// </summary>
        public float Y { get => position[1]; set => position[1] = value; }
        /// <summary>
        /// Get or set the Z position.
        /// </summary>
        public float Z { get => position[2]; set => position[2] = value; }

        /// <summary>
        /// Calculates the distance between this vector and <paramref name="otherVector"/> using Pythagorean equation.
        /// </summary>
        /// <param name="otherVector">The vector that the distance too should be calculated for.</param>
        /// <returns>The distance between this vector and <paramref name="otherVector"/>.</returns>
        public float DistanceBetweenVectors(Vector otherVector)
        {
            return (float)Math.Sqrt(Math.Pow(Math.Abs(X - otherVector.X), 2) + Math.Pow(Math.Abs(Y - otherVector.Y), 2) + Math.Pow(Math.Abs(Z - otherVector.Z), 2));
        }
        /// <summary>
        /// Creates a deep copy of <paramref name="vector"/>.
        /// </summary>
        /// <param name="vector">The vector to create a copy of.</param>
        /// <returns>A new instance of Vector with the same values as <paramref name="vector"/>.</returns>
        public static Vector Copy(Vector vector)
        {
            return new Vector(vector.X, vector.Y, vector.Z);
        }
        /// <summary>
        /// Compares the position values in <paramref name="vector1"/> and <paramref name="vector2"/>.
        /// If they are the same it returns true, else false.
        /// </summary>
        /// <param name="vector1">The first vector to compare.</param>
        /// <param name="vector2">The second vector to compare.</param>
        /// <returns>If both vectors' posistion values are the same it returns true, else false.</returns>
        public static bool Compare(Vector vector1, Vector vector2)
        {
            if ((vector1 == null && vector2 != null) || (vector1 != null && vector2 == null))
                return false;
            if (vector1 == null && vector2 == null)
                return true;
            if (vector1.X == vector2.X && vector1.Y == vector2.Y && vector1.Z == vector2.Z)
                return true;
            return false;
        }
    }
}
