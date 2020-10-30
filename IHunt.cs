using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHunt //i.e. if range to prey is less than n movementspeed*1.2 or something like that.
    {
        public abstract void TrackPrey();

        public abstract void AttackPrey();

    }
}
