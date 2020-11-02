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

        public Carnivore(string species, int reproductionAge, (float X, float Y) location, float maxAge, (byte Minimum, byte Maximum) birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper, animalPublisher, drawPublisher)
        {
        }

        public abstract void AttackPrey();

        public abstract override void AI();

        public abstract void TrackPrey();

        protected abstract override void Death();

        protected abstract override void Eat();

        protected abstract override void Mate();

        protected abstract override void Move();
    }
}
