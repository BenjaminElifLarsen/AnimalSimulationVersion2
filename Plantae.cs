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
        /// <summary>
        /// The spread range of the plant. Used to minimize the likelihood of offspring spaawning on the same posistion.
        /// </summary>
        protected float spreadRange;
        /// <summary>
        /// The minimum and maximum amount of offsprings.
        /// </summary>
        protected (byte Minimum, byte Maximum) offspringAmount; //maybe move this up to Eukaryote.
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
        public Plantae(string species, Vector location, IHelper helper, LifeformPublisher lifeform, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, lifeform, drawPublisher, mapInformation)
        { //could properly overwrite IsPossiblePreyEvnethandler to make (some specific) plants that if to old, cannot be eated, e.g. a big tree
        }

        /// <summary>
        /// The basic AI of any plants. Will only check if Health is below or equal 0 or if Age is above or equal max health.
        /// If either is true it will run Death().
        /// </summary>
        protected override void AI()
        {
            DeathCheckAI();
        }
        protected override bool DeathCheckAI()
        {
            if (Age >= MaxAge || Health <= 0)
            {
                Death();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Contains the code of the reproduction part of the AI.
        /// </summary>
        /// <returns></returns>
        protected abstract bool ReproductionAI();
        /// <summary>
        /// Contains the code of the generate offspring part of the AI.
        /// </summary>
        /// <returns></returns>
        protected abstract bool GiveOffspringsAI();

        /// <summary>
        /// Polinating the plant.
        /// </summary>
        protected abstract void Polinate();
        /// <summary>
        /// Basic implementation of reproduction
        /// </summary>
        protected override void Reproduce() 
        {
            byte amountOfOffsprings = (byte)helper.GenerateRandomNumber(offspringAmount.Minimum, offspringAmount.Maximum);
            object[] dataObject = new object[6];
            dataObject[0] = Species;
            dataObject[2] = helper;
            dataObject[3] = lifeformPublisher;
            dataObject[4] = drawPublisher;
            dataObject[5] = mapInformation;
            GenerateOffspring(amountOfOffsprings, dataObject);
            HasReproduced = false;
        }
        /// <summary>
        /// Generates the spawn location of each offspring and creates a new instance of the object that is calling the function.
        /// </summary>
        /// <param name="amountOfOffsprings">The amount of offsprings.</param>
        /// <param name="objArray">An array of objects that mirror, the parameter types, of a constructor of the class to create an instance of with values in each index set, but index 1.</param>
        protected void GenerateOffspring(int amountOfOffsprings, object[] objArray)
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
                else if (spawnLocation.X > mapInformation.GetSizeOfMap.width)
                    spawnLocation.X = mapInformation.GetSizeOfMap.width - 1;
                if (spawnLocation.Y < 0)
                    spawnLocation.Y = 0;
                else if (spawnLocation.Y > mapInformation.GetSizeOfMap.height)
                    spawnLocation.Y = mapInformation.GetSizeOfMap.height - 1;
                objArray[1] = new Vector(spawnLocation.X, spawnLocation.Y, 0);
                Activator.CreateInstance(type, objArray);
            }
        }
    }
}
