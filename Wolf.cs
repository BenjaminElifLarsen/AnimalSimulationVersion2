using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Wolf : Carnivore, ISleep, ITerritorial
    {
        public Wolf(string species, int reproductionAge, float[] location, float maxAge, int[] birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper, animalPublisher)
        {
            //Wolf wolf = new Wolf(null, 1, null, 2, null, 3, 4, null, (0,0,0), null, 1, Helper.Instance, Publisher.GetAnimalInstance);
            //helper.DeepCopy(new int[] { 5 });
            Territory = GenerateTerritory();
        }

        public string[] Targets { get; set; }
        public (int x, int y)[] Territory { get; set; }
        public string Active { get; set; }

        public void AttackOther(string ID)
        {
            throw new NotImplementedException();
        }

        public override void AI()
        {
        }

        protected override void Death()
        {
            if(mateID != null)
            {

            }
            if(foodID != null)
            {

            }
            RemoveSubscriptions();
        }

        protected override void RemoveSubscriptions()
        {
            //also remove those implemented from ITerritorial
            base.RemoveSubscriptions();
        }

        public void FindTargetEventHandler()
        {
            throw new NotImplementedException();
        }

        public void IsAttackedEventHandler()
        {
            throw new NotImplementedException();
        }
        public (int x, int y)[] GenerateTerritory()
        {
            throw new NotImplementedException();
        }

        public bool IsActive(string period, string activePeriod)
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

        protected override void Mating()
        {
            throw new NotImplementedException();
        }

        protected override void Move()
        {
            throw new NotImplementedException();
        }

        public override void AttackPrey()
        {
            throw new NotImplementedException();
        }

        public override void TrackPrey()
        {
            throw new NotImplementedException();
        }
    }
}
