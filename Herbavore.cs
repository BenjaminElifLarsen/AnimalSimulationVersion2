using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Herbavore : Animalia
    {
        public Herbavore(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher) : base(species, location, foodSource, helper, animalPublisher, drawPublisher)
        {
        }

        public abstract override void AI();

        protected abstract override string FindFood();

        protected abstract override string FindMate();

        protected abstract override void Mate();

        protected abstract override void Move();
    }
}
