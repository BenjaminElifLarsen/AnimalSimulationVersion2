using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;

namespace AnimalSimulationVersion2
{
    class SleepingCarnivore : Carnivore, ISleep 
    { 

        public override float AttackRange { get; set; }
        public float EnergyLevel { get; set; }
        public override float AttackSpeedMultiplier { get; set; }
        public float MaxEnergyLevel { get; }
        public float TimeSlept { get; set; }
        public float SleepLength { get; }
        public bool Sleeping { get; set; }
        /// <summary>
        /// Default constructor. Initialises properites and variables to 'default' values.
        /// </summary>
        /// <param name="species">The species of this animal.</param>
        /// <param name="location">The start location of this animal.</param>
        /// <param name="foodSource">The food source of this animal.</param>
        /// <param name="helper">An instance of IHelper.</param>
        /// <param name="lifeformPublisher">An instance of AnimalPublisher.</param>
        /// <param name="drawPublisher">An instance of DrawPublisher.</param>
        /// <param name="mapInformation">An instance of MapInformation.</param>
        public SleepingCarnivore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, lifeformPublisher, drawPublisher, mapInformation)
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
            MaxEnergyLevel = 140;
            EnergyLevel = MaxEnergyLevel;
            SleepLength = 10; 
            BirthAmount = (1, 3);
            MaxAge = 20;
            ReproductionAge = 4;
            HungerFoodSeekingLevel = 0.5f;
            Gender = GenerateGender(genderInformation); 
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

            if(!DeathCheckAI())
            {
                GiveBirthAI();
                if (!Sleeping)
                {
                    if (!HungerAI())
                        if (!ReproductionAI())
                            if (!FallAsleepAI())
                                MovementAI();
                }
                else
                {
                    WakeUpAI();
                }

            }
        }

        /// <summary>
        /// Predator attacks prey if possible. 
        /// If the distance between the predator and the prey is zero, the predator eats the prey and returns to a normal movementspeed.
        /// </summary>
        public override void AttackPrey()
        {
            Vector preyLocation = lifeformPublisher.GetLocation(foodID);  
            float distance = preyLocation.DistanceBetweenVectors(Location);
            if (distance == 0)
            {
                Eat();
                CurrentMovementSpeed = MovementSpeed;
            }
        }

        /// <summary>
        /// Tracks a prey. Gets the location of the prey. If the distance is same or closer than attack range, increae movementspeed.
        /// Else it will calculate the possible next location out from the current and last location. 
        /// Both cases MoveTo is set to a new Vector.
        /// </summary>
        public override void TrackPrey()  
        { 
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
        } 

        protected override bool GiveBirthAI()
        {
            if (Gender == 'f')
                if (HasReproduced)
                    if (periodInReproduction >= lengthOfReproduction)
                        Reproduce();
            return true;
        }

        protected override bool HungerAI()
        {
            if ((Hunger < MaxHunger * HungerFoodSeekingLevel && EnergyLevel > 0) || Hunger < MaxHunger * 0.1) //softcode those values later.
            {
                if (foodID == null && FindFoodCooldown <= 0)
                {
                    foodID = FindFood();
                    FindFoodCooldown = ContactCooldownLength;
                }
                if (foodID != null)
                {
                    TrackPrey();
                    Move();
                    AttackPrey();
                    return true;
                }
            }
            return false;
        }

        protected override bool ReproductionAI()
        {
            if (Age >= ReproductionAge && EnergyLevel > 0 && TimeToReproductionNeed <= 0)
            {
                if (mateID == null && FindMateCooldown <= 0)
                {
                    mateID = FindMate();
                    FindMateCooldown = ContactCooldownLength;
                }
                if (mateID != null)
                {
                    CurrentMovementSpeed = MovementSpeed;
                    MateLocation = GetLifeformLocation(mateID);
                    MoveTo = MateLocation;
                    Move();
                    Mate();
                    return true;
                }
            }
            return false;
        }

        protected override bool MovementAI()
        {
            if (Vector.Compare(Location, MoveTo))
                MoveTo = GenerateRandomEndLocation();
            CurrentMovementSpeed = MovementSpeed;
            Move();
            return true;
        }
        protected virtual bool FallAsleepAI()
        {
            if (EnergyLevel <= 0)
            {
                Sleep();
                return true;
            }
            return false;
        }
        /// <summary>
        /// The animal might wake up.
        /// </summary>
        /// <returns>True if the animal woke up.</returns>
        protected virtual bool WakeUpAI()
        {
            if (TimeSlept >= SleepLength || Hunger < 10)
            {
                Sleeping = false;
                EnergyLevel = MaxEnergyLevel * TimeSlept / SleepLength;
                return true;
            }
            return false;
        }
    } 


}
