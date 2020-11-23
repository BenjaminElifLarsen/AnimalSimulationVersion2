using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    struct Colour
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }
        public Colour(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = 255;
        }
        public Colour(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
