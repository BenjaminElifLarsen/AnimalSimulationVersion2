using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Carnivore : Animalia, IHunt
    {
        //consider moving the implementation in SleepingCarnivore up to this, so it mirror Herbavore.
        public abstract float AttackRange { get; set; }
        public abstract float AttackSpeedMultiplier { get; set; }
        public Vector PreyLastLocation { get; set; }

        public Carnivore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            Design = new Point[] { new Point(0, 0), new Point(8, 0), new Point(8, 8), new Point(0, 8) };
        }

        public abstract void AttackPrey();
        public abstract void TrackPrey();
    
    }
}
