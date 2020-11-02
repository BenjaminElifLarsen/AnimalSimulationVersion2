using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Carnivore : Animalia, IHunt
    {
        public abstract float AttackRange { get; set; }

        public Carnivore(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper, animalPublisher, drawPublisher)
        {
        }

        public abstract void AttackPrey();

        public abstract override void AI();

        public abstract void TrackPrey();

        protected abstract override void Death();

        protected abstract override void Eat();

        protected abstract override void FindFood();

        protected abstract override void FindMate();

        protected abstract override void Mating();

        protected abstract override void Move();
    }
}
