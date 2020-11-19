using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHerd
    {
        public ushort HerdSize { get; set; }
        public ushort MaxHerdSize { get; set; }
    }
}
