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
            Sleep();
            base.AI();

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
