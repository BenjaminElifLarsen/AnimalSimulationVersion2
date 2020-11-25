using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Interface for birds. 
    /// </summary>
    interface IBird
    {
        /// <summary>
        /// True if the bird can conduct a dive.
        /// </summary>
        public bool CanDive { get; } //maybe have a property for dive speed.
        /// <summary>
        /// The maximum height the bird can reach.
        /// </summary>
        public float MaximumHeight { get; }
        /// <summary>
        /// The maximum speed which the bird can ascend with. Units per second.
        /// </summary>
        public float AscendSpeed { get; }
        /// <summary>
        /// 
        /// </summary>
        public float DiveSpeed { get; }
        /// <summary>
        /// The maximum speed which the bird can desend with. Units per second.
        /// </summary>
        public float DesendSpeed { get; }
        /// <summary>
        /// Modifier to time depending variables on ascending, e.g. increased or decreased hunger decreasement per second.
        /// </summary>
        public float AscendModifier { get; }
        /// <summary>
        /// Modifier to time depending variables on decending, e.g. increased or decreased hunger decreasement per second.
        /// </summary>
        public float DesendModifier { get; }
        /// <summary>
        /// Modifier to time depending variables on hovering, e.g. increased or decreased hunger decreasement per second.
        /// </summary>
        public float HoverModifier { get; }
        public float CurrentModifier { get; set; }
        /// <summary>
        /// The maximum diameter of the circle. 
        /// </summary>
        public float CircleRange { get; }
        /// <summary>
        /// Bird will circle a location
        /// </summary>
        /// <returns></returns>
        public Vector[] Circle();
        /// <summary>
        /// Update the alpha value of the bird. The heigher up, the lower the alpha.
        /// </summary>
        public void UpdateAlpha();
    }
}
