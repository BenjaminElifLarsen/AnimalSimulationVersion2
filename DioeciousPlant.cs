using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace AnimalSimulationVersion2
{
    sealed class DioeciousPlant : Dioecious
    {
        public DioeciousPlant(string species, Vector location, IHelper helper, LifeformPublisher animalPublisher, DrawPublisher drawPublisher, MapInformation mapInformation, char gender = (char)0) : base(species, location, helper, animalPublisher, drawPublisher, mapInformation, gender)
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
