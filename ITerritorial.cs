using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ITerritorial
    {
        public (int x, int y)[] Territory { get; set; }
        public string[] Targets { get; set; } //targets inside of the territorier
        public void AttackOther(string ID); //generate a random value, if above a threshold it does damage to the other, else damage to itself

        public void IsAttackedEventHandler();

        public void FindTargetEventHandler(); //not all species are territorial toward all genders of its species

        public (int x, int y)[] GenerateTerritory();
    }
}
