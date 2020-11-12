using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    /// <summary>
    /// Abstract class for all plants, by default methods assume any plants inheriting are monoecious.
    /// </summary>
    /// <remarks>
    /// This means functions like void Polinate() and void Reproduce() needs to be overwritten for non-monoecious plants.
    /// </remarks>
    abstract class Plantae : Eukaryote
    {
        protected float spreadRange;
        protected (byte Minimum, byte Maximum) offspringAmount;

        public Plantae(string species, (float X, float Y) location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }

        /// <summary>
        /// Baisc implementation of polinating the plant.
        /// </summary>
        protected abstract void Polinate();
        /// <summary>
        /// Basic implementation of reproduction
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
