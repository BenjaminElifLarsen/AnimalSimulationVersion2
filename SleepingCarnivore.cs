using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;

namespace AnimalSimulationVersion2
{ //maybe have two wolf classes, one there is territorial and one that moves in pack. Both inheriting from this one. This class could be renamed to TerrestrialCarnivore
    //Wolf becomes abstract then and keeps ISleep (bird class could be called AvesCarnivore). This class could also be called SleepingCarnivore, since codewise the biggest different between a bird and non-bird would be Z. Then again bird could implement something like IDive
    class SleepingCarnivore : Carnivore, ISleep //, ITerritorial //have an interface for pack/herd behavior? Maybe two interfaces, since in pacts normally only alphas mate, while in herd it is all???
    { 

        public override float AttackRange { get; set; }
        public float EnergyLevel { get; set; }
        public override float AttackSpeedMultiplier { get; set; }
        public float MaxEnergyLevel { get; }
        public float TimeSlept { get; set; }
        public float SleepLength { get; }
        public bool Sleeping { get; set; }

        public SleepingCarnivore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            AttackRange = 20; //order these into groups. 
            AttackSpeedMultiplier = 1.5f;
            lengthOfReproduction = 9;
            genderInformation = new (char Gender, byte Weight)[] { ('f', 50), ('m', 50) };
            reproductionCooldown = 20;
            Colour = new Colour(200, 10, 10);
            Health = MaxHealth;
            MovementSpeed = 20;
            CurrentMovementSpeed = MovementSpeed;
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

        /// <summary>
        /// Overridden version that updates properties of ISleep.
        /// </summary>
        protected override void TimeUpdate()
        {
            base.TimeUpdate();
            EnergyLevel -= timeSinceLastUpdate;
            if(Sleeping)
                TimeSlept += timeSinceLastUpdate;
        }

        /// <summary>
        /// Overridden version that uses ISleep and IHunt.
        /// </summary>
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
                            MateLocation = GetLifeformLocation(mateID);
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

        ///// <summary>
        ///// Generates a territory with four corners. 
        ///// </summary>
        ///// <returns></returns>
        //public (ushort X, ushort Y)[] GenerateTerritory() //Maybe return Vector[] instead of
        //{
        //    byte maxLength = 200;
        //    (ushort X, ushort Y)[] corners = new (ushort X, ushort Y)[4];
        //    (ushort width, ushort height) mapSize = mapInformation.GetSizeOfMap;
        //    ushort mostLeftValue = Location.X - maxLength < 0 ? (ushort)0 : (ushort)(Location.X - maxLength); //ensures that the wolf will spawns its territory inside near itself
        //    ushort mostTopValue = Location.Y - maxLength < 0 ? (ushort)0 : (ushort)(Location.Y - maxLength);
        //    ushort leftX = (ushort)helper.GenerateRandomNumber(mostLeftValue, mapSize.width - maxLength);
        //    ushort topY = (ushort)helper.GenerateRandomNumber(mostTopValue, mapSize.height - maxLength);
        //    corners[0] = (leftX, topY); //left top
        //    corners[1] = ((ushort)helper.GenerateRandomNumber(leftX + 10, leftX + 100), (ushort)helper.GenerateRandomNumber(topY, topY + 100)); //right top
        //    corners[2] = ((ushort)helper.GenerateRandomNumber(corners[1].X - 6, corners[1].X + 50), (ushort)helper.GenerateRandomNumber(corners[1].Y, corners[1].Y + 50)); //right bottom 
        //    corners[3] = ((ushort)helper.GenerateRandomNumber(corners[2].X - 3, corners[2].X + 30), (ushort)helper.GenerateRandomNumber(corners[2].Y + 10, corners[2].Y + 80)); //left bottom
        //    return corners;
        //}

        ///// <summary>
        ///// Animal produces one or multiple offsprings at the same location as it.
        ///// Sets HasReproduced to false. 
        ///// </summary>
        //protected override void Reproduce()
        //{
        //    object[] dataObject = new object[] { Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation };

        //    byte childAmount = (byte)helper.GenerateRandomNumber(BirthAmount.Minimum, BirthAmount.Maximum); //seems like wolves mate for life, but if losing a mate, they will quickly find another one.
        //    for (int i = 0; i < childAmount; i++) //perhaps use the Activator.CreateInstance(...) and this method takes a Type argument, then this implementation could be moved up to Animalia
        //        Activator.CreateInstance(GetType(), dataObject);//new SleepingCarnivore(Species, Location, FoodSource, helper, animalPublisher, drawPublisher, mapInformation);
        //    HasReproduced = false;
        //}
        

        /// <summary>
        /// Predator attacks prey if possible. 
        /// If the distance between the predator and the prey is zero, the predator eats the prey and returns to a normal movementspeed.
        /// </summary>
        public override void AttackPrey()
        {
            Vector preyLocation = lifeformPublisher.GetLocation(foodID);  //get location via event
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

        /// <summary>
        /// Tracks a prey. Gets the location of the prey. If the distance is same or closer than attack range, increae movementspeed.
        /// Else it will calculate the possible next location out from the current and last location. 
        /// Both cases MoveTo is set to a new Vector.
        /// </summary>
        public override void TrackPrey()  //move this and the others into a single class and give an instance to it 
        { //maybe it should try and predict the next location of the prey if it is not in attackRange.
            if (PreyLastLocation == null)
            {
                PreyLastLocation = lifeformPublisher.GetLocation(foodID);
                MoveTo = PreyLastLocation;
            }
            else
            {
                Vector preyLocation = lifeformPublisher.GetLocation(foodID);
                float distance = preyLocation.DistanceBetweenVectors(Location);
                if (distance > AttackRange)
                {
                    (float X, float Y, float Z) differene = (preyLocation.X - PreyLastLocation.X, preyLocation.Y - PreyLastLocation.Y, preyLocation.Z - PreyLastLocation.Z);
                    Vector possibleNextLocation = new Vector(preyLocation.X + differene.X, preyLocation.Y + differene.Y, differene.Z);
                    MoveTo = possibleNextLocation;
                    //CurrentMovementSpeed = MovementSpeed;
                    PreyLastLocation = preyLocation;
                }
                else
                {
                    CurrentMovementSpeed = MovementSpeed * AttackSpeedMultiplier;
                    MoveTo = preyLocation;
                }
            }
        }

        /// <summary>
        /// Sets Sleeping to true and TimeSlept to 0.
        /// </summary>
        public void Sleep()
        {
            Sleeping = true;
            TimeSlept = 0;
        } //(IPack.PackRelationship, string) test = ((IPack.PackRelationship)1, "");
    } //(object, string) test = ((IPack.PackRelationship)1, "");


}
