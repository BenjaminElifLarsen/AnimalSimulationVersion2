using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHunt //i.e. if range to prey is less than n movementspeed*1.2 or something like that.
    { //perhaps have a sprint duration just like IEscapePredator
        /// <summary>
        /// The last location of the prey.
        /// </summary>
        public abstract Vector PreyLastLocation { get; set; }
        /// <summary>
        /// The attack range of the predator.
        /// </summary>
        public abstract float AttackRange { get; set; }
        /// <summary>
        /// The speed multiplier when the predator attacks.
        /// </summary>
        public abstract float AttackSpeedMultiplier { get; set; }
        /// <summary>
        /// Lets the predator track the prey.
        /// </summary>
        public abstract void TrackPrey();
        /// <summary>
        /// Lets the predator attack the prey.
        /// </summary>
        public abstract void AttackPrey();

    }
}
