using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Interface for handling family. Contains properties for parents, family, siblings and children and whether any of these can be mated with and/or hunted.
    /// </summary>
    interface IFamily
    { //how to get the IDs...
        public abstract string[] Parents { get; set; }
        public abstract string[] Family { get; set; }
        public abstract string[] Siblings { get; set; }
        public abstract string[] Children { get; set; }
        public abstract bool CanHuntParents { get; set; }
        public abstract bool CanHuntFamily { get; set; }
        public abstract bool CanHuntSublings { get; set; }
        public abstract bool CanHuntChildren { get; set; }
        public abstract bool CanMateParents { get; set; }
        public abstract bool CanMateFamily { get; set; }
        public abstract bool CanMateSublings { get; set; }
        public abstract bool CanMateChildren { get; set; }
    }
}
