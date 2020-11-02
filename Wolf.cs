using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Wolf : Carnivore, ISleep, ITerritorial //have an interface for pack/herd behavior? Maybe two interfaces, since in pacts normally only alphas mate, while in herd it is all???
    {
        /// <summary>
        /// The length of the pregnacy in seconds.
        /// </summary>
        protected float lengthOfPregnacy; 
        /// <summary>
        /// How long time the current pregnacy has lasted in seconds.
        /// </summary>
        protected float periodInPregnacy;
        protected (char Gender, byte Weight)[] genderInformation = new (char Gender, byte Weight)[] { ('f',50),('m',50) };

        public override float AttackRange { get; set; }
        public string[] Targets { get; set; }
        public (int x, int y)[] Territory { get; set; }
        public string Active { get; set; }
        public float EnergyLevel { get; set; }
        public override float AttackSpeedMultiplier { get; set; }

        public Wolf(string species, int reproductionAge, (float X, float Y) location, float maxAge, (byte Minimum, byte Maximum) birthAmount, float movementSpeed, float hunger, Point[] design, (int Red, int Green, int Blue) colour, string[] foodSource, float nutrienceValue, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher) : base(species, reproductionAge, location, maxAge, birthAmount, movementSpeed, hunger, design, colour, foodSource, nutrienceValue, helper, animalPublisher, drawPublisher)
        {
            //Wolf wolf = new Wolf(null, 1, (1,2), 2, (1,2), 3, 4, null, (0,0,0), null, 1, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance);
            //helper.DeepCopy(new int[] { 5 });
            Territory = GenerateTerritory();
            AttackRange = 20;
            AttackSpeedMultiplier = 1.5f;
            lengthOfPregnacy = 9;

        }


        public void AttackOther(string ID)
        { //transmit out a delegate
            throw new NotImplementedException();
        }

        public override void AI() //maybe move the code in this over to Carnivore or even Animalia.
        {
            if (Health <= 0 || Age > MaxAge) //nothing is finalised for the AI design.
                Death();
            else
            {
                if((Hunger < 50 && EnergyLevel > 0) || Hunger < 10) //softcode those values later.
                {
                    if(foodID == null) //if wolves do not mate for life, maybe discard the mateID if not null if it is hungry
                        foodID = FindFood();
                    if (foodID != null)
                    {
                        TrackPrey();
                        Move();
                        AttackPrey();
                    }
                    else
                        Move();
                }else if (Age >= ReproductionAge && EnergyLevel > 0)
                {
                    if(TimeToReproductionNeed <= 0)
                    {
                        if(mateID == null)
                            mateID = FindMate();
                        if (mateID != null)
                        {
                            MateLocation = GetMateLocation(mateID);
                            Move();
                            Mate();
                        }
                        else
                            Move();
                    }
                }else if (EnergyLevel <= 0)
                {
                    Sleep(); //would still need to update the pregnacy time in this case and when hungry
                }

            }
        }

        protected override void Death() //maybe move up to Animalia 
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
            Hunger = 100; //the nutriedjwdwpd value.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Wolf mating.
        /// </summary>
        protected override void Mate()
        {
            if (MateLocation == Location)
            {
                periodInPregnacy = 0;
                HasMated = true;
            }
            if(HasMated)
            { 
                //either here or before the call, check if the end location and current location is the same.
                if(Gender == 'f') //later on, alter the FindMate() to check if the wolf is alpha, if a pack wolf, and only mate with the other alpha. 
                { //maybe have both parents stay together for a while while the female wolf is pregnant. Check up if both parents stay with their children, also if they mate is for life
                    periodInPregnacy += timeSinceLastUpdate;
                    if(periodInPregnacy >= lengthOfPregnacy)
                    { //generate a random number, need to depedency inject a random generator to ensure the values are not the same for each call if multiple calls happen quickly. Also needed for random movement.
                        byte childAmount = 0; //seems like wolves mate for life, but if losing a mate, they will quickly find another one.
                        for (int i = 0; i < childAmount; i++)
                            new Wolf(Species, ReproductionAge, Location, MaxAge, BirthAmount, MovementSpeed, Hunger, Design, Colour, FoodSource, NutrienValue, helper, animalPublisher, drawPublisher);
                        TimeToReproductionNeed = 200; //keep the new wol(f/ves) in a list for a short period so the IPack methods can be updated to contain the newest family.
                        HasMated = false;
                    }
                }
                else //how to let the father now of the children.
                {
                    periodInPregnacy += timeSinceLastUpdate;
                    if (periodInPregnacy >= lengthOfPregnacy)
                    {
                        TimeToReproductionNeed = 200;
                        HasMated = false;
                    }
                }
            }
            throw new NotImplementedException();
        }

        protected override void Move() //needs to check if the wolf is hungry and go after it, if it is not go for a mate if it can mate else select a random location and go to it
        { //if it goes for a prey, it should call TrackPrey
            throw new NotImplementedException();
        }

        /// <summary>
        /// Wolf attacking prey. 
        /// </summary>
        public override void AttackPrey()
        {
            (float X, float Y) preyLocation = (0,0);  //get location via event
            float distance = Math.Abs(preyLocation.X - Location.X) + Math.Abs(preyLocation.Y - Location.Y);
            if(distance == 0)
            {
                Eat();//have two events for dead animals. One for a prey been eaten and one for an animal died 'normally'. For eaten it should returns the animal's nutrience value.
            }
            //else if(distance <= AttackRange)
            //{
            //    CurrentMovementSpeed = MovementSpeed * AttackSpeedMultiplier;
            //} 
        }

        public override void TrackPrey()
        { //maybe it should try and predict the next location of the prey if it is not in attackRange.
            (float X, float Y) preyLocation = (0, 0);
            float distance = Math.Abs(preyLocation.X - Location.X) + Math.Abs(preyLocation.Y - Location.Y);
            if(distance > AttackRange)
            {
                (float X, float Y) differene = (PreyLastLocation.X - preyLocation.X, PreyLastLocation.Y - preyLocation.Y);
                (float X, float Y) possibleNextLocation = (preyLocation.X + differene.X, preyLocation.Y + differene.Y);
                MoveTo = possibleNextLocation;
                CurrentMovementSpeed = MovementSpeed;
            }
            else
            {
                CurrentMovementSpeed = MovementSpeed * AttackSpeedMultiplier;
                MoveTo = preyLocation; 
            }    
        }

        public void Sleep()
        {
            throw new NotImplementedException();
        }
    }
}
