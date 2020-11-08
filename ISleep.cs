using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ISleep
    { //not all animals sleep fully, some species only sleep with one part of the brain at a time.
        public abstract bool Sleeping { get; set; }
        public abstract float TimeSlept { get; set; }
        public abstract float SleepLength { get; }
        public abstract float EnergyLevel { get; set; } //affects change to fail/succede at something like losing a prey or get away
        public abstract string Active { get;} //dayactive, nightactive or both
        public abstract float MaxEnergyLevel { get;}
        public bool IsActive(string period, string activePeriod);
        public void Sleep();
        
    }
}
//for animals that does not sleep with both brain parts at the same time, have a debuff. Maybe just in thier implementation