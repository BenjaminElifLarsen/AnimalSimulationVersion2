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
        public (float X, float Y) PreyLastLocation { get; set; }

        public Carnivore(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher) : base(species, location, foodSource, helper, animalPublisher, drawPublisher)
        {
        }

        public abstract void AttackPrey();

        public abstract override void AI();

        public abstract void TrackPrey();

        protected abstract override void Mate();

        protected abstract override void Move();
    }
}
