using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ITerritorial
    {
        /// <summary>
        /// Contains the end points of the territory.
        /// </summary>
        public (ushort x, ushort y)[] Territory { get; set; } //update to use Vectors
        /// <summary>
        /// The ID of all lifeforms inside of the territory.
        /// </summary>
        public string[] Targets { get; set; } //targets inside of the territorier
        /// <summary>
        /// Attacks the lifeform with <paramref name="ID"/>.
        /// </summary>
        /// <param name="ID"></param>
        public void AttackOther(string ID); //generate a random value, if above a threshold it does damage to the other
        /// <summary>
        /// Not sure what this should do again. Already got one for getting all locations and such
        /// </summary>
        public void FindTargetEventHandler(); //not all species are territorial toward all genders of its species
        /// <summary>
        /// Generates the end points of the territory of the lifeform.
        /// </summary>
        /// <returns></returns>
        public (ushort X, ushort Y)[] GenerateTerritory(); //update to use Vectors
    }
}
