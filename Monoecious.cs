using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    class Monoecious : Plantae
    {
        public Monoecious(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }
        protected override void AI()
        {
            TimeUpdate();
            if (TimeToReproductionNeed <= 0)
                Polinate();
            if (HasReproduced && periodInReproduction >= lengthOfReproduction)
                Reproduce();
        }

        /// <summary>
        /// Baisc monoecious implementation of polinating the plant.
        /// </summary>
        protected override void Polinate()
        {
            TimeToReproductionNeed = reproductionCooldown + lengthOfReproduction;
            periodInReproduction = 0;
            HasReproduced = true;
        }
        /// <summary>
        /// Basic implementation of reproduction, assumes the plant is monoecious
        /// </summary>
        protected override void Reproduce() //needs testing
        {
            byte amountOfOffsprings = (byte)helper.GenerateRandomNumber(offspringAmount.Minimum, offspringAmount.Maximum);
            Type type = this.GetType();
            object[] dataObject = new object[6];
            dataObject[0] = Species;
            dataObject[2] = helper;
            dataObject[3] = animalPublisher;
            dataObject[4] = drawPublisher;
            dataObject[5] = mapInformation;
            for (byte i = 0; i < amountOfOffsprings; i++)
            {
                float xPercentage = (float)(helper.GenerateRandomNumber(0, 100) / 100f);
                float xMaxDistance = xPercentage * spreadRange;
                float yMaxDistance = (1 - xPercentage) * spreadRange;
                float xDistance = helper.GenerateRandomNumber(0, (int)xMaxDistance) - xMaxDistance / 2f;
                float yDistance = helper.GenerateRandomNumber(0, (int)yMaxDistance) - yMaxDistance / 2f;
                (float X, float Y) spawnLocation = (Location.X + xDistance, Location.Y + yDistance);
                dataObject[1] = spawnLocation;
                Activator.CreateInstance(type, dataObject);
            }
            HasReproduced = false;
        }

    }
}
