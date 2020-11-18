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
        /// <summary>
        /// Contains the roles a lifeform can have toward another lifeform.
        /// </summary>
        public enum FamilyRelationship
        { //polar bear males and hunt their children, yet a female will not. Can always do a gender check
            Parent = 1,
            Sibling = 2,
            Child = 3,
            NonFamily = 0
        }
        /// <summary>
        /// Contains an array that makes up the relationships and IDs for each specific lifeform. 
        /// </summary>
        public abstract (FamilyRelationship Relationship, string ID)[] Family { get; set; }
        /// <summary>
        /// True if the lifeform can hurt its parents.
        /// </summary>
        public abstract bool CanHuntParents { get; set; }
        /// <summary>
        /// True if the lifeform can hurt its siblings
        /// </summary>
        public abstract bool CanHuntSiblings { get; set; }
        /// <summary>
        /// True if the lifeform can hurt its children
        /// </summary>
        public abstract bool CanHuntChildren { get; set; }
        /// <summary>
        /// True if the lifeform can mate with its parents
        /// </summary>
        public abstract bool CanMateParents { get; set; }
        /// <summary>
        /// True if the lifeform can mate with its siblings
        /// </summary>
        public abstract bool CanMateSublings { get; set; }
        /// <summary>
        /// True if the lifeform can mate with its children.
        /// </summary>
        public abstract bool CanMateChildren { get; set; }
        /// <summary>
        /// Used to receiver data for Family.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains IDs and Data.</param>
        public abstract void RelationshipEventHandler(object sender, ControlEvents.TransmitDataEventArgs e);
    }
}
