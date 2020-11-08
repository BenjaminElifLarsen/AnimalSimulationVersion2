using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Wolf : Carnivore, ISleep, ITerritorial //have an interface for pack/herd behavior? Maybe two interfaces, since in pacts normally only alphas mate, while in herd it is all???
    {

        public override float AttackRange { get; set; }
        public string[] Targets { get; set; }
        public (ushort x, ushort y)[] Territory { get; set; }
        public string Active { get; }
        public float EnergyLevel { get; set; }
        public override float AttackSpeedMultiplier { get; set; }
        public float MaxEnergyLevel { get;}
        public float TimeSlept { get; set; }
        public float SleepLength { get; }
        public bool Sleeping { get; set; }


        public Wolf(string species, (float X, float Y) location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            //Wolf wolf = new Wolf(null, 1, (1,2), 2, (1,2), 3, 4, null, (0,0,0), null, 1, Helper.Instance, Publisher.GetAnimalInstance, Publisher.GetDrawInstance);
            //helper.DeepCopy(new int[] { 5 });
            Territory = GenerateTerritory();
            AttackRange = 20;
            AttackSpeedMultiplier = 1.5f;
            lengthOfPregnacy = 9;
            genderInformation = new (char Gender, byte Weight)[] { ('f', 50), ('m', 50) };
            MaxEnergyLevel = 300;
            reproductionCooldown = 200;
            Colour = (200, 10, 10);
            Design = new Point[] { new Point(0,0), new Point(10, 0), new Point(10, 10), new Point(0, 10) };
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
                if(periodInPregnacy < lengthOfPregnacy && HasMated)
                    periodInPregnacy += timeSinceLastUpdate;
                if (!Sleeping)
                {
                    if ((Hunger < 50 && EnergyLevel > 0) || Hunger < 10) //softcode those values later.
                    {//if hungry, drop the mate
                        if (mateID != null)
                            animalPublisher.RemoveMate(ID, mateID);
                        if (foodID == null) //figure out what to do if the animal got no need for food, mating and such and should just go to a random location
                            foodID = FindFood(); //also if they are in a pack/herd they need to stick together.
                        if (foodID != null)
                        {
                            TrackPrey(); 
                            Move();
                            AttackPrey();
                        }
                        else
                            Move();
                    }
                    else if (Age >= ReproductionAge && EnergyLevel > 0 && TimeToReproductionNeed <= 0)
                    {
                            if (mateID == null)
                                mateID = FindMate();
                            if (mateID != null)
                            {
                                MateLocation = GetMateLocation(mateID);
                                Move();
                                Mate();
                            }
                            else
                                Move(); //figure out a good way to lower the amount of calls to Move() in this method.
                        
                    }
                    else if (EnergyLevel <= 0)
                    {
                        Sleep(); 
                    }
                    else
                    { //set a random location, a wolf should stay close or inside its territory
                        if (Location == MoveTo)
                            MoveTo = GenerateRandomEndLocation();
                        Move();
                    }
                }
                else
                {
                    TimeSlept += timeSinceLastUpdate; //consider making a method in ISleep for this
                    if (TimeSlept >= SleepLength || Hunger < 10) 
                    { //maybe allow for an ealy wake up if it is to hungry
                        Sleeping = false;
                        EnergyLevel = MaxEnergyLevel * TimeSlept / SleepLength;
                    }//have an function to decrease energy, TimeToReproductionNeed, hunger etc.
                }

            }
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
        public (ushort X, ushort Y)[] GenerateTerritory()
        {
            byte maxLength = 200;
            (ushort X, ushort Y)[] corners = new (ushort X, ushort Y)[4];
            (ushort width, ushort height) mapSize = mapInformation.GetSizeOfMap;
            ushort mostLeftValue = Location.X - maxLength < 0 ? (ushort)0 : (ushort)(Location.X - maxLength); //ensures that the wolf will spawns its territory inside near itself
            ushort mostTopValue = Location.Y - maxLength < 0 ? (ushort)0 : (ushort)(Location.Y - maxLength);
            ushort leftX = (ushort)helper.GenerateRandomNumber(mostLeftValue, mapSize.width - maxLength);
            ushort topY = (ushort)helper.GenerateRandomNumber(mostTopValue, mapSize.height - maxLength);
            corners[0] = (leftX, topY); //left top
            corners[1] = ((ushort)helper.GenerateRandomNumber(leftX+10,leftX+100),(ushort)helper.GenerateRandomNumber(topY,topY+100)); //right top
            corners[2] = ((ushort)helper.GenerateRandomNumber(corners[1].X - 6, corners[1].X + 50), (ushort)helper.GenerateRandomNumber(corners[1].Y, corners[1].Y + 50)); //right bottom 
            corners[3] = ((ushort)helper.GenerateRandomNumber(corners[2].X - 3, corners[2].X + 30), (ushort)helper.GenerateRandomNumber(corners[2].Y+10, corners[2].Y + 80)); //left bottom
            return corners;
        }

        public bool IsActive(string period, string activePeriod)
        {
            return period == activePeriod;
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
                if(Gender == 'f') //later on, alter the FindMate() to check if the wolf is alpha, if a pack wolf, and only mate with the other alpha. 
                { //maybe have both parents stay together for a while while the female wolf is pregnant. Check up if both parents stay with their children, also if they mate is for life
                    if(periodInPregnacy >= lengthOfPregnacy)
                    { //generate a random number, need to depedency inject a random generator to ensure the values are not the same for each call if multiple calls happen quickly. Also needed for random movement.
                        byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); //seems like wolves mate for life, but if losing a mate, they will quickly find another one.
                        for (int i = 0; i < childAmount; i++)
                            new Wolf(Species, Location, FoodSource, helper, animalPublisher, drawPublisher,mapInformation);
                        TimeToReproductionNeed = reproductionCooldown; //keep the new wol(f/ves) in a list for a short period so the IPack methods can be updated to contain the newest family.
                        HasMated = false;
                    }
                }
                else //how to let the father now of the children. Maybe the female should raise an event to let the father know of the children
                {
                    TimeToReproductionNeed = reproductionCooldown + lengthOfPregnacy;
                    HasMated = false;
                }
            }
        }

        protected override void Move() //maybe move this up to Animalia
        {
            float distanceToEndLocation = Math.Abs(MoveTo.X - Location.X) + Math.Abs(MoveTo.Y - Location.Y);
            //calculates the %s of the move distance that belong to x and y and then multiply those numbers with the current movement speed. 
            float xPercentage = Math.Abs(MoveTo.X - Location.X) / distanceToEndLocation;
            float xCurrentSpeed = xPercentage * CurrentMovementSpeed;
            float yCurrentSpeed = (1 - xPercentage) * CurrentMovementSpeed;
            //need to find the direction to move in. 
            float amountOfXToMove = (MoveTo.X - Location.X) * xCurrentSpeed;
            float amountOfYToMove = (MoveTo.Y - Location.Y) * (yCurrentSpeed);
            //set the new location
            Location = (Location.X + amountOfXToMove, Location.Y + amountOfYToMove);
        }

        /// <summary>
        /// Wolf attacking prey. 
        /// </summary>
        public override void AttackPrey()
        {
            (float X, float Y) preyLocation = animalPublisher.GetLocation(foodID);  //get location via event
            float distance = Math.Abs(preyLocation.X - Location.X) + Math.Abs(preyLocation.Y - Location.Y);
            if(distance == 0)
            {
                Eat();//have two events for dead animals. One for a prey been eaten and one for an animal died 'normally'. For eaten it should returns the animal's nutrience value.
                CurrentMovementSpeed = MovementSpeed;
            }
            //else if(distance <= AttackRange)
            //{
            //    CurrentMovementSpeed = MovementSpeed * AttackSpeedMultiplier;
            //} 
        }

        public override void TrackPrey()  //move this and the others into a single class and give an instance to it 
        { //maybe it should try and predict the next location of the prey if it is not in attackRange.
            (float X, float Y) preyLocation = animalPublisher.GetLocation(foodID);
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
            Sleeping = true;
            TimeSlept = 0;
        }
    }
}
