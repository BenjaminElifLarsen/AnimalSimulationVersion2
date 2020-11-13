using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Carnivore : Animalia, IHunt
    {

        public abstract float AttackRange { get; set; }
        public abstract float AttackSpeedMultiplier { get; set; }
        public Vector PreyLastLocation { get; set; }

        public Carnivore(string species, Vector location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }

        public abstract void AttackPrey();

        public virtual void TrackPrey()
        {
            if (PreyLastLocation == null) 
            {
                PreyLastLocation = animalPublisher.GetLocation(foodID);
            }
        }
    
    }
}
