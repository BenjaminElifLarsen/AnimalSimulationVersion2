using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    abstract class Herbavore : Animalia
    {
        public Herbavore(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, int[] colour, string[] foodSource, float nutrienceValue, IHelper helper) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper)
        {
        }

        public override void AI()
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
