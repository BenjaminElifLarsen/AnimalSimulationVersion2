﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Wolf : Carnivore, ISleep, ITerritorial
    {
        public override float AttackRange { get; set; }
        public string[] Targets { get; set; }
        public (int x, int y)[] Territory { get; set; }
        public string Active { get; set; }
        public float EnergyLevel { get; set; }

        public Wolf(string species, int reproductionAge, (float X, float Y) location, float maxAge, (byte Minimum, byte Maximum) birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper, animalPublisher, drawPublisher)
        {
            Wolf wolf = new Wolf(null, 1, (1,2), 2, (1,2), 3, 4, null, (0,0,0), null, 1, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance);
            //helper.DeepCopy(new int[] { 5 });
            Territory = GenerateTerritory();
        }


        public void AttackOther(string ID)
        { //transmit out a delegate
            throw new NotImplementedException();
        }

        public override void AI()
        {
            if (Health <= 0) //nothing is finalised for the AI design.
                Death();
            else
            {
                if(Hunger < 50) //softcode that value later.
                {
                    if(foodID == null) //if wolves do not mate for life, maybe discard the mateID if not null if it is hungry
                        FindFood();
                }else if (Age >= ReproductionAge)
                {
                    if(TimeToReproductionNeed <= 0)
                    {
                        if(mateID == null)
                            mateID = FindMate();
                    }
                }
                Move();
            }
        }

        protected override void Death()
        {
            if(mateID != null)
            {
                animalPublisher.RemoveMate(ID, mateID);
            }
            if(foodID != null)
            {
                animalPublisher.RemovePrey(ID, foodID);
            }
            RemoveSubscriptions();
        }

        protected override void RemoveSubscriptions()
        {
            //also remove those implemented from interfaces
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

        public void Sleep()
        {
            throw new NotImplementedException();
        }
    }
}
