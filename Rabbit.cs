using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Rabbit : Herbavore, IHide
    {
        public Rabbit(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }

        public int StealthLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AI()
        {
            throw new NotImplementedException();
        }

        public void HideFromPredator()
        {
            throw new NotImplementedException();
        }

        protected override void Death()
        {
            throw new NotImplementedException();
        }

        protected override void Eat()
        {
            throw new NotImplementedException();
        }

        protected override string FindFood()
        {
            throw new NotImplementedException();
        }

        protected override string FindMate()
        {
            throw new NotImplementedException();
        }

        protected override void Mate()
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
