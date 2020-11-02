using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ISleep
    { //not all animals sleep fully, some species only sleep with one part of the brain at a time.
        public abstract float EnergyLevel { get; set; } //affects change to fail/succede at something like losing a prey or get away
        public abstract string Active { get; set; } //dayactive, nightactive or both
        public bool IsActive(string period, string activePeriod);
        public void Sleep();
    }
}
