using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    sealed class MonoeciousPlant : Monoecious //consider what extra stuff to do
    { //after all, some plants reproduce through fire.
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
        public MonoeciousPlant(string species, Vector location, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation) : base(species, location, helper, lifeformPublisher, drawPublisher, mapInformation)
        {
            spreadRange = 140;
            offspringAmount = (1, 3);

            lengthOfReproduction = 20;
            reproductionCooldown = OneAgeInSeconds*2;

            NutrientValue = 14;

            MaxHealth = 100;
            Health = MaxHealth;
            MaxAge = 4;
            ReproductionAge = 2;

            Colour = new Colour(100, 200, 0);
            Design = new Point[] { new Point(3,0), new Point(6,3), new Point(3,6), new Point(0,3) };

        }
    }
}
