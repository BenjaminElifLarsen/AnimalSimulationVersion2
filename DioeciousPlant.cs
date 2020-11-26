using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    sealed class DioeciousPlant : Dioecious
    {
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
        public DioeciousPlant(string species, Vector location, IHelper helper, LifeformPublisher lifeformPublisher, DrawPublisher drawPublisher, MapInformation mapInformation, char gender = (char)0) : base(species, location, helper, lifeformPublisher, drawPublisher, mapInformation, gender)
        {
            spreadRange = 240;
            offspringAmount = (2, 3);

            lengthOfReproduction = 8;
            reproductionCooldown = OneAgeInSeconds * 3;

            NutrientValue = 40;

            MaxHealth = 100;
            Health = MaxHealth;
            MaxAge = 7;
            ReproductionAge = 3;
            distanceDivider = 25;

            Colour = new Colour(200, 100, 0);
            Design = new Point[] { new Point(3, 0), new Point(6, 3), new Point(3, 6), new Point(0, 3) };

        }
    }
}
