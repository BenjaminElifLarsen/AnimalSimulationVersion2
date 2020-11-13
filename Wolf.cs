using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Wolf : Carnivore, ISleep, ITerritorial //have an interface for pack/herd behavior? Maybe two interfaces, since in pacts normally only alphas mate, while in herd it is all???
    {

        public override float AttackRange { get; set; }
        public string[] Targets { get; set; }
        public (ushort x, ushort y)[] Territory { get; set; }
        public float EnergyLevel { get; set; }
        public override float AttackSpeedMultiplier { get; set; }
        public float MaxEnergyLevel { get; }
        public float TimeSlept { get; set; }
        public float SleepLength { get; }
        public bool Sleeping { get; set; }

        public Wolf(string species, Vector location, string[] foodSource, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            Territory = GenerateTerritory(); //values are not final
            AttackRange = 20; //order these into groups. 
            AttackSpeedMultiplier = 1.5f;
            lengthOfReproduction = 9;
            genderInformation = new (char Gender, byte Weight)[] { ('f', 50), ('m', 50) };
            reproductionCooldown = 20;
            Colour = (200, 10, 10);
            Design = new Point[] { new Point(0, 0), new Point(10, 0), new Point(10, 10), new Point(0, 10) };
            Health = MaxHealth;
            MovementSpeed = 20;
            MaxHunger = 100;
            Hunger = MaxHunger;
            MaxEnergyLevel = 140; //perhaps have things like energy level, hunger etc. be in seconds. 
            EnergyLevel = MaxEnergyLevel;
            SleepLength = 10; 
            BirthAmount = (1, 3);
            MaxAge = 20;
            ReproductionAge = 4;
            HungerFoodSeekingLevel = 0.5f;
            Gender = GenerateGender(genderInformation); //would be better if this could be called in the base, but genderInformation is first set after... maybe move it up the variable
        }

        public void AttackOther(string ID)
        { //transmit out a delegate
            throw new NotImplementedException();
        }

        protected override void TimeUpdate()
        {
            base.TimeUpdate();
            EnergyLevel -= timeSinceLastUpdate;
            if(Sleeping)
                TimeSlept += timeSinceLastUpdate;
        }

        protected override void AI() //maybe move the code in this over to Carnivore or even Animalia.
        {
            TimeUpdate();

            if (Health <= 0 || Age > MaxAge) //nothing is finalised for the AI design.
                Death();
            else
            {
                if (Gender == 'f')
                    if(HasReproduced)
                        if (periodInReproduction >= lengthOfReproduction)
                            Reproduce();
                if (!Sleeping)
                {
                    if ((Hunger < MaxHunger * HungerFoodSeekingLevel && EnergyLevel > 0) || Hunger < MaxHunger * 0.1) //softcode those values later.
                    {
                        if (foodID == null) 
                            foodID = FindFood(); //if they are in a pack/herd they need to stick together.
                        if (foodID != null)
                        {
                            TrackPrey();
                            Move();
                            AttackPrey();
                        }
                        else
                            DefaultMovement();
                    }
                    else if (Age >= ReproductionAge && EnergyLevel > 0 && TimeToReproductionNeed <= 0)
                    {
                        if (mateID == null)
                            mateID = FindMate();
                        if (mateID != null)
                        {
                            CurrentMovementSpeed = MovementSpeed;
                            MateLocation = GetMateLocation(mateID);
                            MoveTo = MateLocation;
                            Move();
                            Mate();
                        }
                        else
                            DefaultMovement();//figure out a good way to lower the amount of calls to Move() in this method.

                    }
                    else if (EnergyLevel <= 0)
                    {
                        Sleep();
                    }
                    else
                    { //set a random location, a wolf should stay close or inside its territory
                        DefaultMovement();
                    }
                }
                else
                {
                    //consider making a method in ISleep for this
                    if (TimeSlept >= SleepLength || Hunger < 10)
                    { 
                        Sleeping = false;
                        EnergyLevel = MaxEnergyLevel * TimeSlept / SleepLength;
                    }
                }

                void DefaultMovement()
                {
                    if (Vector.Compare(Location, MoveTo))
                        MoveTo = GenerateRandomEndLocation();
                    CurrentMovementSpeed = MovementSpeed;
                    Move();
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
            corners[1] = ((ushort)helper.GenerateRandomNumber(leftX + 10, leftX + 100), (ushort)helper.GenerateRandomNumber(topY, topY + 100)); //right top
            corners[2] = ((ushort)helper.GenerateRandomNumber(corners[1].X - 6, corners[1].X + 50), (ushort)helper.GenerateRandomNumber(corners[1].Y, corners[1].Y + 50)); //right bottom 
            corners[3] = ((ushort)helper.GenerateRandomNumber(corners[2].X - 3, corners[2].X + 30), (ushort)helper.GenerateRandomNumber(corners[2].Y + 10, corners[2].Y + 80)); //left bottom
            return corners;
        }



        protected override void Reproduce()
        {
            //if (Gender == 'f') //later on, alter the FindMate() to check if the wolf is alpha, if a pack wolf, and only mate with the other alpha. 
            //{ //maybe have both parents stay together for a while while the female wolf is pregnant. Check up if both parents stay with their children, also if they mate is for life
                 //split this if-statment into another method so the female wolf can give birth even when hungry
                 //generate a random number, need to depedency inject a random generator to ensure the values are not the same for each call if multiple calls happen quickly. Also needed for random movement.
                    byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); //seems like wolves mate for life, but if losing a mate, they will quickly find another one.
                    for (int i = 0; i < childAmount; i++) //perhaps use the Activator.CreateInstance(...) and this method takes a Type argument, then this implementation could be moved up to Animalia
                        new Wolf(Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation);
                    HasReproduced = false;
                
            //}
        }
        

        /// <summary>
        /// Wolf attacking prey. 
        /// </summary>
        public override void AttackPrey()
        {
            Vector preyLocation = animalPublisher.GetLocation(foodID);  //get location via event
            float distance = preyLocation.DistanceBetweenVectors(Location);
            if (distance == 0)
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
            base.TrackPrey();
            Vector preyLocation = animalPublisher.GetLocation(foodID);
            float distance = Math.Abs(preyLocation.X - Location.X) + Math.Abs(preyLocation.Y - Location.Y);
            if (distance > AttackRange)
            {
                (float X, float Y) differene = (preyLocation.X - PreyLastLocation.X, preyLocation.Y - PreyLastLocation.Y);
                Vector possibleNextLocation = new Vector(preyLocation.X + differene.X, preyLocation.Y + differene.Y,0);
                MoveTo = possibleNextLocation;
                CurrentMovementSpeed = MovementSpeed;
                PreyLastLocation = preyLocation;
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
