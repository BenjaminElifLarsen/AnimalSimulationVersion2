using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Herbavores that can sleep. However, in this case the animal will not sleep fully, but rather be weaken while tired, e.g. slow movement.
    /// </summary>
    class SleepingHerbavore : Herbavore, ISleep
    {
        protected byte baseDiscoverChance;
        protected float baseDiscoverRange;
        public bool Sleeping { get; set; }
        public float TimeSlept { get ; set; }

        public float SleepLength { get; }

        public float EnergyLevel { get; set; }

        public float MaxEnergyLevel { get; }
        public float SleepModifer { get; }

        public SleepingHerbavore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, animalPublisher, drawPublisher, mapInformation)
        {
            MaxEnergyLevel = 30;
            EnergyLevel = MaxEnergyLevel;
            SleepLength = 6;
            SleepModifer = 0.4f;
            baseDiscoverChance = DiscoverChance;
            baseDiscoverRange = DiscoverRange;

            BirthAmount = (2, 3);

            Colour = new Colour(0, 0, 255);
        } //if wanting to add the variable values in using a constructor, you will need to do something similar to Dioecious(Plant) for Activator.CreateInstance(type,object[])
        //maybe lower the amount of parameters using a struct... will need a struct for each different constructor since they need to have different variables.
        protected override void TimeUpdate()
        {
            if (Sleeping)
                TimeSlept += timeSinceLastUpdate;
            if (!Sleeping)
                EnergyLevel -= timeSinceLastUpdate;
            base.TimeUpdate();
        }
        /// <summary>
        /// Overriden to use Sleep().
        /// </summary>
        protected override void AI()
        {
            TimeUpdate();
            Sleep();
            if (Age >= MaxAge || Health <= 0)
                Death();
            else
            {
                #region Female Reproduction
                if (Gender == 'f')
                    if (HasReproduced)
                        if (periodInReproduction >= lengthOfReproduction)
                            Reproduce();
                #endregion
                #region Escaping
                if (!IsRunning && HuntedBy.Length > 0 && TimeSprinted < EscapeSprintTime && Hunger > 0)
                {
                    IsRunning = DiscoveredPredator(DiscoverRange, DiscoverChance);
                    if (IsRunning)
                    {
                        CurrentMovementSpeed *= EscapeSpeedMultiplier;
                        MoveTo = EscapeLocation(PredatorID);
                    }
                }
                else if (TimeSprinted > EscapeSprintTime)
                {
                    IsRunning = false;
                    CurrentMovementSpeed = Sleeping ? MovementSpeed * SleepModifer : MovementSpeed;
                }
                if (IsRunning && Hunger > 0)
                {
                    Move();
                    if (!HasRolled)
                    {
                        HasRolled = true;
                        TimeSinceLastRoll = 0;
                        if (TryLosePredator(PredatorID))
                        {
                            LostPredator(PredatorID);
                            IsRunning = false;
                            CurrentMovementSpeed = Sleeping ? MovementSpeed * SleepModifer : MovementSpeed;
                        }
                        else
                            if (Vector.Compare(MoveTo, Location))
                            MoveTo = EscapeLocation(PredatorID);
                    }
                }
                #endregion
                #region Hunger
                else if (Hunger < MaxHunger * HungerFoodSeekingLevel)
                {
                    if (mateID != null) //can have a property that decided if an animal mates for life or not.
                    {
                        lifeformPublisher.RemoveMate(ID, mateID);
                        mateID = null;
                    }
                    if (foodID == null)
                        foodID = FindFood();
                    if (foodID != null)
                    {
                        MoveTo = lifeformPublisher.GetLocation(foodID);
                        Move();
                        if (Vector.Compare(Location, MoveTo))
                            Eat();
                    }
                    else
                        DefaultMovement();
                }
                #endregion
                #region Mating
                else if (Age >= ReproductionAge && TimeToReproductionNeed <= 0)
                {
                    if (mateID == null)
                        mateID = FindMate();
                    if (mateID != null)
                    {
                        MoveTo = MateLocation = GetLifeformLocation(mateID);
                        Move();
                        Mate();
                    }
                    else
                        DefaultMovement();
                }
                #endregion
                #region Default Movement
                else
                    DefaultMovement();
                #endregion
            }

            void DefaultMovement()
            {
                if (Vector.Compare(Location, MoveTo))
                    MoveTo = GenerateRandomEndLocation();
                CurrentMovementSpeed = Sleeping ? MovementSpeed * SleepModifer : MovementSpeed;
                Move();
            }
        }

        public void Sleep() //got a very different implementation than SleepingCarnivore.Sleep()
        {
            if (!Sleeping)
            {
                if (EnergyLevel <= 0)
                {
                    TimeSlept = 0;
                    Sleeping = true;
                    CurrentMovementSpeed = MovementSpeed * SleepModifer;
                    DiscoverChance = (byte)(baseDiscoverChance * SleepModifer);
                    DiscoverRange = baseDiscoverRange * SleepModifer;
                }
            }
            else if (TimeSlept >= SleepLength)
            {
                EnergyLevel = MaxEnergyLevel;
                Sleeping = false;
            }
            else if (Hunger <= 0)
            {
                Sleeping = false;
                EnergyLevel = MaxEnergyLevel * (TimeSlept / SleepLength);
            }
            if (!Sleeping)
                StatsNormal();

            void StatsNormal()
            {
                CurrentMovementSpeed = MovementSpeed;
                DiscoverChance = baseDiscoverChance;
                DiscoverRange = baseDiscoverRange;
            }
        }
    }
}
