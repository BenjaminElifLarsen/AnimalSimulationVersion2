using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ISleep
    { //not all animals sleep fully, some species only sleep with one part of the brain at a time.
        /// <summary>
        /// True if the lifefrom is sleeping.
        /// </summary>
        public abstract bool Sleeping { get; set; }
        /// <summary>
        /// Time in seconds spent sleeping.
        /// </summary>
        public abstract float TimeSlept { get; set; }
        /// <summary>
        /// The maximum lenght of sleep.
        /// </summary>
        public abstract float SleepLength { get; }
        /// <summary>
        /// The current energy level.
        /// </summary>
        public abstract float EnergyLevel { get; set; } //affects change to fail/succede at something like losing a prey or get away
        /// <summary>
        /// The maximum energy level.
        /// </summary>
        public abstract float MaxEnergyLevel { get;}
        public void Sleep();
        //maybe have a wake up method
    }
}
//for animals that does not sleep with both brain parts at the same time, have a debuff. Maybe just in thier implementation