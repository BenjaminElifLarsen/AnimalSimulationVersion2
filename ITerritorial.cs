using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface ITerritorial
    {
        public string[] Targets { get; set; }
        public void AttackOtherEventHandler(string ID); //generate a random value, if above a threshold it does damage to the other, else damage to itself

        public void IsAttackedEventHandler();

        public void FindTargetEventHandler();

        public void RemoveTerritoralSubscriptions() { }
    }
}
