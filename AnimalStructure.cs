using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    public struct AnimalStructure
    {
        public float MaxAge { get; set; }
        public int ReproductionAge { get; set; }
        public float[] StartLocation { get; set; }
        public int[] BirthAmount { get; set; }
        public string Species { get; set; }
        public float MovementSpeed { get; set; }
        public Point[] Design { get; set; }
        public int[] Colour { get; set; }
        public string[] FoodSource { get; set; }
        public string ActivePeriod { get; set; }
        public float NutrienceValue { get; set; }
    }
}
