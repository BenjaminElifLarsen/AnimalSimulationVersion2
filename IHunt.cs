using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    interface IHunt //i.e. if range to prey is less than n movementspeed*1.2 or something like that.
    { //perhaps have a sprint duration just like IEscapePredator
        public abstract Vector PreyLastLocation { get; set; }
        public abstract float AttackRange { get; set; }
        public abstract float AttackSpeedMultiplier { get; set; }
        public abstract void TrackPrey();
        public abstract void AttackPrey();

    }
}
