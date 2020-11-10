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
        public enum FamilyRelationship
        {
            Parent = 1,
            Sibling = 2,
            Child = 3,
            NonFamily = 0
        }

        public abstract (FamilyRelationship Relationship, string ID)[] Family { get; set; }
        public abstract bool CanHuntParents { get; set; }
        public abstract bool CanHuntSublings { get; set; }
        public abstract bool CanHuntChildren { get; set; }
        public abstract bool CanMateParents { get; set; }
        public abstract bool CanMateSublings { get; set; }
        public abstract bool CanMateChildren { get; set; }
        class TransmitIDsEventArgs : EventArgs //does not seem like a good idea, perhaps it is fine to just implement the broker in the classes themselves
        { //ask Thomas
            public TransmitIDsEventArgs()
            { //transmit something like (relationship, id)[] family

            }
        }
    }
}
