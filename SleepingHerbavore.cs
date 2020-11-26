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
        public SleepingHerbavore(string species, Vector location, string[] foodSource, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, foodSource, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            MaxEnergyLevel = 30;
            EnergyLevel = MaxEnergyLevel;
            SleepLength = 6;
            SleepModifer = 0.4f;
            baseDiscoverChance = DiscoverChance;
            baseDiscoverRange = DiscoverRange;

            BirthAmount = (2, 3);

            Colour = new Colour(0, 0, 255);
        }

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
            Sleep();
            base.AI();

        }
        /// <summary>
        /// Checks if a lifeform should fall asleep or wake up.
        /// </summary>
        public void Sleep() 
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
