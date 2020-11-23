using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// The colour struct. 
    /// </summary>
    struct Colour
    {
        /// <summary>
        /// Get the red colour.
        /// </summary>
        public byte Red { get; }
        /// <summary>
        /// Get the green colour.
        /// </summary>
        public byte Green { get; }
        /// <summary>
        /// Get the blue colour.
        /// </summary>
        public byte Blue { get; }
        /// <summary>
        /// Get teh alpha.
        /// </summary>
        public byte Alpha { get; }
        /// <summary>
        /// Constructor used to set the colours, but not the alpha. Alpha is set to 255.
        /// </summary>
        /// <param name="red">The amount of red.</param>
        /// <param name="green">The amount of green.</param>
        /// <param name="blue">The amount of blue.</param>
        public Colour(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = 255;
        }
        /// <summary>
        /// Constructor used to set the colours and the alpha.
        /// </summary>
        /// <param name="red">The amount of red.</param>
        /// <param name="green">The amount of green.</param>
        /// <param name="blue">The amount of blue.</param>
        /// <param name="alpha">The amount of alpha.</param>
        public Colour(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }
    }
}
