using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IPack 
    {
        /// <summary>
        /// Contains the roles a lifeform can have toward another lifeform.
        /// </summary>
        public enum PackRelationship
        {
            Alpha = 1,
            Member = 2,
            NonMember = 0
        }
        /// <summary>
        /// Contains an array that makes up the relationships and IDs for each specific lifeform. 
        /// </summary>
        public abstract (PackRelationship Relationship, string ID)[] PackMembers { get; set; }
        /// <summary>
        /// The amount of lifeforms in the pack
        /// </summary>
        public byte PackSize => (byte)PackMembers.Length;
        /// <summary>
        /// The maximum amount of lifeforms that can be in pack.
        /// </summary>
        public abstract byte MaxPackSize { get; set; }
        /// <summary>
        /// The amount of time that has passed since the last fight.
        /// </summary>
        public abstract float TimeSinceLastFight { get; set; } //update uml
        /// <summary>
        /// The amount of time that has to be between each fight.
        /// </summary>
        public abstract float FightCooldown { get; set; }
        /// <summary>
        /// True if the pack can fight for alpha posistion.
        /// </summary>
        public abstract bool CanFightForAlpha { get; set; } //how to ensure the animals do not fight all the time and also figure out why one animal would fight another for alpha
        /// <summary>
        /// Allows a pack member to fight another pack member.
        /// </summary>
        /// <param name="ID">The ID of the target</param>
        public abstract void Fight(string ID); //transmit delegate. 
        
    }
}
