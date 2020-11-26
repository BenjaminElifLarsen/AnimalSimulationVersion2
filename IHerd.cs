using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHerd
    {
        /// <summary>
        /// The size of the herd.
        /// </summary>
        public ushort HerdSize { get; set; }
        /// <summary>
        /// The maximum size of the herd.
        /// </summary>
        public ushort MaxHerdSize { get; set; }
    }
}
