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
        { //polar bear males and hunt their children, yet a female will not.
            Parent = 1,
            Sibling = 2,
            Child = 3,
            NonFamily = 0
        }

        public abstract (FamilyRelationship Relationship, string ID)[] Family { get; set; }
        public abstract bool CanHuntParents { get; set; }
        public abstract bool CanHuntSiblings { get; set; }
        public abstract bool CanHuntChildren { get; set; }
        public abstract bool CanMateParents { get; set; }
        public abstract bool CanMateSublings { get; set; }
        public abstract bool CanMateChildren { get; set; }
    }
}
