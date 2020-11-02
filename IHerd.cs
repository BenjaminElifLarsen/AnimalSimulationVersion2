using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHerd
    {
        public abstract ushort HerdSize { get; set; }
        public abstract ushort MaxHerdSize { get; set; }
    }
}
