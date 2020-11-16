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

        public Plantae(string species, Vector location, IHelper helper, AnimalPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation)
        {
        }

        protected override void AI()
        {
            if (Age >= MaxAge || Health <= 0)
                Death();
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
            object[] dataObject = new object[6];
            dataObject[0] = Species;
            dataObject[2] = helper;
            dataObject[3] = animalPublisher;
            dataObject[4] = drawPublisher;
            dataObject[5] = mapInformation;
            GenerateChildren(amountOfOffsprings, dataObject);
            HasReproduced = false;
        }
        protected void GenerateChildren(int amountOfOffsprings, object[] objArray)
        {
            for (byte i = 0; i < amountOfOffsprings; i++)
            {
                Type type = this.GetType();
                float xPercentage = (float)(helper.GenerateRandomNumber(0, 100) / 100f);
                float xMaxDistance = xPercentage * spreadRange;
                float yMaxDistance = (1 - xPercentage) * spreadRange;
                float xDistance = helper.GenerateRandomNumber(0, (int)xMaxDistance) - xMaxDistance / 2f;
                float yDistance = helper.GenerateRandomNumber(0, (int)yMaxDistance) - yMaxDistance / 2f;
                (float X, float Y) spawnLocation = (Location.X + xDistance, Location.Y + yDistance);
                if (spawnLocation.X < 0)
                    spawnLocation.X = 0;
                else if (spawnLocation.X > mapInformation.mapSize.width)
                    spawnLocation.X = mapInformation.mapSize.width - 1;
                if (spawnLocation.Y < 0)
                    spawnLocation.Y = 0;
                else if (spawnLocation.Y > mapInformation.mapSize.height)
                    spawnLocation.Y = mapInformation.mapSize.height - 1;
                objArray[1] = new Vector(spawnLocation.X, spawnLocation.Y, 0);
                Activator.CreateInstance(type, objArray);
            }
        }
    }
}
