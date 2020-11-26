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
        /// The relationship of the lifeform in the pack.
        /// </summary>
        public PackRelationship Relationship { get; set; }
        /// <summary>
        /// True if only alphas can mate.
        /// </summary>
        public bool AlphaMatingOnly { get; set; } 
        /// <summary>
        /// Contains an array that makes up the relationships and IDs for each specific lifeform. 
        /// </summary>
        public (PackRelationship Relationship, string ID, char Gender)[] PackMembers { get; set; }
        /// <summary>
        /// The amount of lifeforms in the pack
        /// </summary>
        public byte PackSize => (byte)PackMembers.Length;
        /// <summary>
        /// The maximum amount of lifeforms that can be in pack.
        /// </summary>
        public byte MaxPackSize { get; set; }
        /// <summary>
        /// The amount of time that has passed since the last fight, in seconds.
        /// </summary>
        public float TimeSinceLastFight { get; set; }
        /// <summary>
        /// The amount of time, in seconds, that has to be between each fight.
        /// </summary>
        public float FightCooldown { get; set; }
        /// <summary>
        /// True if the pack can fight for alpha posistion.
        /// </summary>
        public bool CanFightForAlpha { get; set; } 
        /// <summary>
        /// Contains the ID of all lifeforms that have attacked this lifeform.
        /// </summary>
        public string[] AttackedBy { get; set; }
        /// <summary>
        /// The range which the lifeform can strike another lifeform.
        /// </summary>
        public float StrikeRange { get; }
        /// <summary>
        /// The attack speed in seconds.
        /// </summary>
        public float AttackSpeed { get; }
        /// <summary>
        /// The cooldown between attacks in seconds.
        /// </summary>
        public float AttackCooldown { get; set; }
        /// <summary>
        /// Allows a pack member to fight another pack member.
        /// </summary>
        /// <param name="ID">The ID of the target</param>
        public void Fight();
        /// <summary>
        /// Generates a pack.
        /// </summary>
        /// <returns>The pack array.</returns>
        public void GeneratePack();
        /// <summary>
        /// Makes use of RelationshipCandidateEventHandler to create a new pack.
        /// </summary>
        /// <param name="receiverID"></param>
        public void TransmitPack(string receiverID);
        /// <summary>
        /// Used to receiver data for Pack.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains IDs and Data.</param>
        public void RelationshipEventHandler(object sender, ControlEvents.TransmitDataEventArgs e);
        /// <summary>
        /// Someone is looking for a pack and wants to know if this lifeform can join.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RelationshipCandidateEventHandler(object sender, ControlEvents.RelationshipCandidatesEventArgs e);
    }
}
