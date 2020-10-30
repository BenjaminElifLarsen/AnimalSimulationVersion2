using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Carnivore : Animal, IHunt
    {
        public Carnivore(string species) : base(species)
        {

        }

        public virtual void AttackPrey()
        {
            throw new NotImplementedException();
        }

        public override void Control()
        {
            throw new NotImplementedException();
        }

        public virtual void TrackPrey()
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

        protected override void FindFood()
        {
            throw new NotImplementedException();
        }

        protected override void FindMate()
        {
            throw new NotImplementedException();
        }

        protected override char GenerateGender()
        {
            throw new NotImplementedException();
        }

        protected override string GenerateID()
        {
            throw new NotImplementedException();
        }

        protected override void Mating()
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
