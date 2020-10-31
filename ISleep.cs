using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ISleep
    { //not all animals sleep fully, some species only sleep with one part of the brain at a time.

        public string Active { get; set; } //dayactive, nightactive or both
        public bool IsActive(string period, string activePeriod);
    }
}
