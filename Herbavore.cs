using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Herbavore : Animalia
    {
        public Herbavore(string species, int reproductionAge, (float X, float Y) location, float maxAge, (byte Minimum, byte Maximum) birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher, DrawdrawPublisher drawPublisher) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper, animalPublisher, drawPublisher)
        {
        }

        public abstract override void AI();

        protected abstract override void Death();

        protected abstract override void Eat();

        protected abstract override void FindFood();

        protected abstract override void FindMate();

        //protected abstract override char GenerateGender();

        protected abstract override void Mating();

        protected abstract override void Move();
    }
}
